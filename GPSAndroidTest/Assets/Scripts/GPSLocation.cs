using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GPSLocation : MonoBehaviour
{
	public static GPSLocation Instance;

	public bool PCVersion = false;

	public float mapSizeX = 45;
	public float mapSizeY = 80;

	public float startLon = -1;
	public float startLat = -1;

	public float GPSTimeWait = 5;

	[SerializeField] private Text GPSDataText = null;
	[SerializeField] private Text recentDebugInformation = null;

	[SerializeField] private Transform compass = null;

	private PhotonView photonView;

	private float currentLon = -1;
	private float currentLat = -1;

	private bool GPSReady = false; //Even if the location service is running, the GPS might not be reporting correctly yet

	void Start()
	{
		Instance = this;
		photonView = GetComponent<PhotonView>();

		if (PCVersion)
		{
			recentDebugInformation.text = "GPS not started since this is a PC version.";
		} 
		else
		{
			StartCoroutine(StartGPS());
		}
	}

	void Update()
	{
		//Note: The code below synchronises startLon and startLat by setting them in MasterClients update
		//		and having the clients get them in their update. This has to happen in update because we don't
		//		know when anyone connects/disconnects. This is not an amazing way of doing it, but it will do for now.
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			photonView.RPC("SetStartLat", RpcTarget.Others, startLon);
			photonView.RPC("SetStartLon", RpcTarget.Others, startLat);
		}
		else
		{
			startLon = GetStartLon();
			startLat = GetStartLat();
		}

		if (Input.location.status == LocationServiceStatus.Failed)
		{
			recentDebugInformation.text = "GPS Connection failed. Restart/reconnect the game and make sure that the GPS is active.";
		}
		else if (IsGPSReady())
		{
			recentDebugInformation.text = "GPS is running";

			//UPDATE COORDINATES OF DEVICE
			UpdateCurrentCoordinates();

			//ROTATION OF COMPASS
			float north = Input.compass.trueHeading;
			compass.rotation = Quaternion.Slerp(compass.rotation, Quaternion.Euler(0, 0, north), Time.deltaTime * 2);
		}
	}

	private void UpdateCurrentCoordinates()
	{
		currentLon = Input.location.lastData.longitude;
		currentLat = Input.location.lastData.latitude;
	}

	public bool IsGPSReady()
	{
		return Input.location.status == LocationServiceStatus.Running && GPSReady;
	}

	public Vector3 DeviceCurrentPosition()
	{
		return ConvertToXY(startLon, currentLon, startLat, currentLat);
	}

	public float GetCurrentLon()
	{
		return currentLon;
	}

	public float GetCurrentLat()
	{
		return currentLat;
	}

	IEnumerator StartGPS()
	{
		//Start off with giving the app some time to start
		yield return new WaitForSeconds(1);

#if PLATFORM_ANDROID
		if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
		{
			Permission.RequestUserPermission(Permission.FineLocation);
			print("Asking for permission");
			recentDebugInformation.text = "Asking for permission";
		}
#endif

		yield return new WaitForSeconds(1);

		//Check if the device has location service enabled
		if (!Input.location.isEnabledByUser)
		{
			print("Location not enabled by user");
			recentDebugInformation.text = "Location not enabled by user";
			yield break;
		}

		Input.location.Start(0.1f, 0.1f);
		Input.compass.enabled = true;

		//It takes some time for the GPS to activate and return a location
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			print("Waiting...");
			recentDebugInformation.text = "Waiting...";
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		//If the service failed to initialize
		if (maxWait < 1)
		{
			print("Timed out");
			recentDebugInformation.text = "Timed out. Restart/reconnect the app";
			yield break;
		}

		yield return new WaitForSeconds(GPSTimeWait);

		recentDebugInformation.text = "Getting GPS ready. Please wait for " + GPSTimeWait * 6 + " seconds...";

		yield return new WaitForSeconds(GPSTimeWait);

		yield return new WaitForSeconds(GPSTimeWait);

		yield return new WaitForSeconds(GPSTimeWait);

		yield return new WaitForSeconds(GPSTimeWait);

		yield return new WaitForSeconds(GPSTimeWait);

		recentDebugInformation.text = "Found center";
	
		startLon = Input.location.lastData.longitude;
		startLat = Input.location.lastData.latitude;
		Debug.Log("StartLon, StartLat: " + startLon + ", " + startLat);

		GPSReady = true;
	}

	//Returns the distance between two points in meters
	public float Haversine(float longitude1, float longitude2, float latitude1, float latitude2)
	{
		//using the haversine formula
		float R = 6371000.0f; //Earths radius
		float F1 = latitude1 * Mathf.Deg2Rad; //latitude1 in radians
		float F2 = latitude2 * Mathf.Deg2Rad; //latitude2 in radians
		float deltaF = (latitude2 - latitude1) * Mathf.Deg2Rad; //the delta of latitudes
		float deltaL = (longitude2 - longitude1) * Mathf.Deg2Rad; //the delta of longitudes

		float a = Mathf.Pow(Mathf.Sin(deltaF / 2), 2) + Mathf.Cos(F1) * Mathf.Cos(F2) * Mathf.Pow(Mathf.Sin(deltaL / 2), 2); //Haversine - the great-circle distance between two points

		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

		float d = R * c; //distance between the two points in meters

		return d;
	}

	public Vector3 ConvertToXY(float longitude1, float longitude2, float latitude1, float latitude2)
	{
		float lonDistanceFromCenter = Haversine(longitude1, longitude2, latitude1, latitude1);
		float latDistanceFromCenter = Haversine(longitude1, longitude1, latitude1, latitude2);

		float scaleToX = mapSizeX;
		float scaleToY = mapSizeY;

		if (longitude1 > longitude2)
		{
			scaleToX = -scaleToX;
		}
		if (latitude1 > latitude2)
		{
			scaleToY = -scaleToY;
		}

		float xPos = Map(lonDistanceFromCenter, 0, mapSizeX, 0, scaleToX);
		float yPos = Map(latDistanceFromCenter, 0, mapSizeY, 0, scaleToY);

		return new Vector3(xPos, yPos, 1);
	}

	private float Map(float v, float from1, float to1, float from2, float to2)
	{
		return (v - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	[PunRPC]
	public void SetStartLat(float _startLat)
	{
		startLat = _startLat;
	}
	[PunRPC]
	public void SetStartLon(float _startLon)
	{
		startLon = _startLon;
	}

	public float GetStartLat()
	{
		return startLat;
	}

	public float GetStartLon()
	{
		return startLon;
	}
}
