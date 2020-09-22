using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
	public static ConnectToServer Instance;

	public string PIN = "1234pin";

	public GameObject GPSPlayerPrefab;
	public GameObject mousePlayerPrefab;

	public bool useMousePlayer = false;

	public Text text;
	public Text connectionText;

	public RectTransform gameUI;
	public RectTransform waitUI;
	public RectTransform enterUI;

	public static GameObject LocalPlayerInstance;

	private bool isConnected = false;
	private int numOfPlayers = 0;

	private string gameVersion = "1";

	void Start()
	{
		Instance = this;
	}

	public void Connect()
	{
		PhotonNetwork.GameVersion = gameVersion;
		PhotonNetwork.ConnectUsingSettings();

		text.text = PhotonNetwork.BestRegionSummaryInPreferences;
	}

	public void SetPin(string pin)
	{
		PIN = pin;
	}

	public string GetPin()
	{
		return PIN;
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRoom(PIN);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(PIN);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnected because: " + cause);
		isConnected = false;

		connectionText.text = "Disconnected because: " + cause;
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left Room");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		numOfPlayers--;

		connectionText.text = "Joined room with name: " + PhotonNetwork.CurrentRoom.Name
			+ ". Room has " + numOfPlayers + " player(s).";
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		numOfPlayers++;

		connectionText.text = "Joined room with name: " + PhotonNetwork.CurrentRoom.Name
			+ ". Room has " + numOfPlayers + " player(s).";

	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Created room with name: " + PhotonNetwork.CurrentRoom.Name
			+ ". Room has " + PhotonNetwork.CurrentRoom.PlayerCount + " player.");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room with name: " + PhotonNetwork.CurrentRoom.Name
			+ ". Room has " + PhotonNetwork.CurrentRoom.PlayerCount + " player(s).");

		isConnected = true;
		numOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
		connectionText.text = "Joined room with name: " + PhotonNetwork.CurrentRoom.Name
			+ ". Room has " + numOfPlayers + " players.";

		if (GPSPlayerPrefab == null)
		{
			Debug.LogError("Missing playerPrefab Reference", this);
		}
		else
		{
			if (LocalPlayerInstance == null)
			{
				Quaternion spawnAngle = Quaternion.Euler(90, 0, 0);
				Vector3 spawnPosition = new Vector3(0, 0.5f, 0);
				if (useMousePlayer)
				{
					LocalPlayerInstance = PhotonNetwork.Instantiate(mousePlayerPrefab.name, spawnPosition, spawnAngle, 0);
				} else
				{
					LocalPlayerInstance = PhotonNetwork.Instantiate(GPSPlayerPrefab.name, spawnPosition, spawnAngle, 0);
				}

				//CameraMovement.Instance.SetLookAt(LocalPlayerInstance.transform);
				//CameraMovement.Instance.SetFollow(LocalPlayerInstance.transform);
			}
		}
	}
}
