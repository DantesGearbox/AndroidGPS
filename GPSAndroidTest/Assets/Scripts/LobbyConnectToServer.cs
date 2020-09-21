using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyConnectToServer : MonoBehaviourPunCallbacks
{
	public string PIN = "1234pin";

	public void SetPin(string pin)
	{
		PIN = pin;
	}

	public string GetPin()
	{
		return PIN;
	}

	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRoom(PIN);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(PIN);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Created room with name: " + PhotonNetwork.CurrentRoom.Name 
			+ ". Room has " + PhotonNetwork.CurrentRoom.PlayerCount + " players.");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room with name: " + PhotonNetwork.CurrentRoom.Name 
			+ ". Room has " + PhotonNetwork.CurrentRoom.PlayerCount + " players.");
	}
}
