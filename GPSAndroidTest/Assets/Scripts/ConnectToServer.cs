using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
	public static ConnectToServer Instance;

	public GameObject playerPrefab;
	public GameObject mousePlayerPrefab;

	public bool useMousePlayer = false;

	public Text text;

	public static GameObject LocalPlayerInstance;

	private string gameVersion = "1";
	
	//[SerializeField] private byte maxPlayerPerRoom = 8;

	void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;

		Instance = this;

		text.text = PhotonNetwork.BestRegionSummaryInPreferences;
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(null, new RoomOptions());
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnected because: " + cause);
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left Room");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("Player Left Room");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room");

		if (playerPrefab == null)
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
					LocalPlayerInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnAngle, 0);
				}

				CameraMovement.Instance.SetLookAt(LocalPlayerInstance.transform);
				//CameraMovement.Instance.SetFollow(LocalPlayerInstance.transform);
			}
		}
	}
}
