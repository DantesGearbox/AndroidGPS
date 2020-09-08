using UnityEngine;
using System.Collections;

public class GPSLocationService : MonoBehaviour
{
	//private bool GPSActive;
	//private float timer;

	//[SerializeField]
	//PhotonView photonView;

	//public float StartLon
	//{
	//	get { return startLon; }
	//	set { startLon = value; }
	//}
	//public float StartLat
	//{
	//	get { return startLat; }
	//	set { startLat = value; }
	//}
	//public float startLon;
	//public float startLat;

	//[SerializeField]
	//float[] mapSize;

	//public float lon;
	//public float lat;

	//GameData.CitySet _citySet;

	//void Start()
	//{
	//	timer = 0;
	//	photonView = GetComponent<PhotonView>();
	//	StartCoroutine(StartGPS());
	//	//MapSize should be regulated due to the cityset selected, as it determins the size of the map
	//}

	//void Update()
	//{
	//	if (StateManager.Instance.CurrentActiveState == GameData.GameStates.Play)
	//	{
	//		if (PhotonNetwork.isMasterClient)
	//		{
	//			photonView.RPC("SetStartLat", PhotonTargets.Others, startLon);
	//			photonView.RPC("SetStartLon", PhotonTargets.Others, startLat);
	//		}
	//		else
	//		{
	//			StartLat = GetStartLat();
	//			startLon = GetStartLon();
	//		}
	//	}
	//	timer += Time.deltaTime;
	//	//If connection fails
	//	if (Input.location.status == LocationServiceStatus.Failed)
	//	{
	//		print("GPS Connection failed");
	//	}
	//	else if (Input.location.status == LocationServiceStatus.Running)
	//	{
	//		//If the connection is granted and location is returned
	//		lon = Input.location.lastData.longitude;
	//		lat = Input.location.lastData.latitude;
	//	}
	//	else
	//	{
	//		print("GPS not running");
	//	}
	//}

	//IEnumerator StartGPS()
	//{
	//	//Check if the device has location service enabled
	//	if (!Input.location.isEnabledByUser)
	//	{
	//		yield break;
	//	}

	//	Input.location.Start(0.1f, 0.1f);

	//	//It takes some time for the GPS to activate and return a location
	//	int maxWait = 20;
	//	while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
	//	{
	//		yield return new WaitForSeconds(1);
	//		maxWait--;
	//	}

	//	//If the service failed to initialize
	//	if (maxWait < 1)
	//	{
	//		print("Timed out");
	//		yield break;
	//	}
	//	startLon = Input.location.lastData.longitude;
	//	startLat = Input.location.lastData.latitude;
	//}

	//public float Haversine(float longitude1, float longitude2, float latitude1, float latitude2)
	//{
	//	//using the haversine formula
	//	float R = 6371000.0f; //Earths radius
	//	float F1 = latitude1 * Mathf.Deg2Rad; //latitude1 in radians
	//	float F2 = latitude2 * Mathf.Deg2Rad; //latitude2 in radians
	//	float deltaF = (latitude2 - latitude1) * Mathf.Deg2Rad; //the delta of latitudes
	//	float deltaL = (longitude2 - longitude1) * Mathf.Deg2Rad; //the delta of longitudes

	//	float a = Mathf.Pow(Mathf.Sin(deltaF / 2), 2) + Mathf.Cos(F1) * Mathf.Cos(F2) * Mathf.Pow(Mathf.Sin(deltaL / 2), 2); //Haversine - the great-circle distance between two points

	//	float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

	//	float d = R * c; //distance between the two points in meters

	//	return d;
	//}

	//public Vector3 ConvertToXY(float longitude1, float longitude2, float latitude1, float latitude2)
	//{
	//	float lonDistanceFromCenter = Haversine(longitude1, longitude2, latitude1, latitude1);
	//	float latDistanceFromCenter = Haversine(longitude1, longitude1, latitude1, latitude2);

	//	float scaleToX = 80.0f;
	//	float scaleToY = 45.0f;

	//	if (longitude1 > longitude2)
	//	{
	//		scaleToX = -80.0f;
	//	}
	//	if (latitude1 > latitude2)
	//	{
	//		scaleToY = -45.0f;
	//	}

	//	float xPos = map(lonDistanceFromCenter, 0.0f, mapSize[0], 0, scaleToX);
	//	float yPos = map(latDistanceFromCenter, 0.0f, mapSize[1], 0, scaleToY);
	//	return new Vector3(xPos, 1, yPos);
	//}

	//float map(float v, float from1, float to1, float from2, float to2)
	//{
	//	return (v - from1) / (to1 - from1) * (to2 - from2) + from2;
	//}

	//[PunRPC]
	//public void SetStartLat(float _startLat)
	//{
	//	StartLat = _startLat;
	//}
	//[PunRPC]
	//public void SetStartLon(float _startLon)
	//{
	//	StartLon = _startLon;
	//}

	//public float GetStartLat()
	//{
	//	return StartLat;
	//}

	//public float GetStartLon()
	//{
	//	return StartLon;
	//}
}
