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

	public float mapSizeX = 45;
	public float mapSizeY = 80;

	[SerializeField] private Text GPSDataText = null;
	[SerializeField] private Text recentDebugInformation = null;

	[SerializeField] private Transform centerOfMap = null;
	[SerializeField] private Transform player = null;

	[SerializeField] private Transform compass = null;

	[SerializeField] private Transform blue = null;
	[SerializeField] private Transform purple = null;
	[SerializeField] private Transform red = null;
	[SerializeField] private Transform green = null;

	private ExitGames.Client.Photon.Hashtable roomStartCoords = new ExitGames.Client.Photon.Hashtable();

	private bool GPSReady = false; //Even if the location service is running, the GPS might not be reporting correctly yet

	public bool IsGPSReady()
	{
		return Input.location.status == LocationServiceStatus.Running && GPSReady;
	}

	public Vector3 DeviceCurrentPosition()
	{
		float currentLon = Input.location.lastData.longitude;
		float currentLat = Input.location.lastData.latitude;

		Vector2 startLonLat = GrabStartCoords();

		return ConvertToXY(startLonLat.x, currentLon, startLonLat.y, currentLat);
	}

	private Vector2 DeviceCurrentCoordinates()
	{
		float currentLon = Input.location.lastData.longitude;
		float currentLat = Input.location.lastData.latitude;

		Vector2 coords = new Vector2(currentLon, currentLat);

		return coords;
	}

	private Vector2 GrabStartCoords()
	{
		float startLon = -1;
		if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("StartLon"))
		{
			Debug.Log("IsMasterClient: " + PhotonNetwork.IsMasterClient);
			startLon = (float)PhotonNetwork.MasterClient.CustomProperties["StartLon"];
		}

		float startLat = -1;
		if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("StartLat"))
		{
			Debug.Log("IsMasterClient: " + PhotonNetwork.IsMasterClient);
			startLat = (float)PhotonNetwork.MasterClient.CustomProperties["StartLat"];
		}

		return new Vector2(startLon, startLat);
	}

	void Start()
	{
		Instance = this;
		StartCoroutine(StartGPS());
	}

	void Update()
	{
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			recentDebugInformation.text = "GPS Connection failed. Restart/reconnect the game and make sure that the GPS is active.";
		}
		else if (IsGPSReady())
		{
			recentDebugInformation.text = "GPS is running";
			//ROTATION OF COMPASS
			float north = Input.compass.trueHeading;
			compass.rotation = Quaternion.Slerp(compass.rotation, Quaternion.Euler(0, 0, north), Time.deltaTime * 2);

			Vector2 startLonLat = GrabStartCoords();
			GPSDataText.text = "Custom properties: " + startLonLat.x + ", " + startLonLat.y;
		}
		else
		{

		}
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

		float timeWait = 1f;

		yield return new WaitForSeconds(timeWait);

		recentDebugInformation.text = "Getting GPS ready. Please wait for " + timeWait * 6 + " seconds...";

		yield return new WaitForSeconds(timeWait);

		yield return new WaitForSeconds(timeWait);

		yield return new WaitForSeconds(timeWait);

		yield return new WaitForSeconds(timeWait);

		yield return new WaitForSeconds(timeWait);

		recentDebugInformation.text = "Found center";

		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			float startLon = Input.location.lastData.longitude;
			float startLat = Input.location.lastData.latitude;

			Debug.Log("Saving custom properties: " + startLon + ", " + startLat);
			OnSetStartCoords(startLon, startLat);
		}

		GPSReady = true;
	}
	public void OnSetStartCoords(float startLon, float startLat)
	{
		roomStartCoords["StartLon"] = startLon;
		roomStartCoords["StartLat"] = startLat;

		PhotonNetwork.MasterClient.CustomProperties = roomStartCoords;
		//PhotonNetwork.LocalPlayer.CustomProperties.Add("StartLon", startLon);
		//PhotonNetwork.LocalPlayer.CustomProperties.Add("StartLat", startLat);
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
}
