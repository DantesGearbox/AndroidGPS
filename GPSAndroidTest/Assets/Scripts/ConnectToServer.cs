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

	public Text text;

	public static GameObject LocalPlayerInstance;

	private string gameVersion = "1";
	
	//[SerializeField] private byte maxPlayerPerRoom = 8;

	void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;

		Instance = this;
	}

	private void Update()
	{
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
				PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
			}
		}
	}
}
