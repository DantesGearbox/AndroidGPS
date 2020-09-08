using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPropertyStartCoords : MonoBehaviour
{
	public float startLon;
	public float startLat;

	public Player player;

	private ExitGames.Client.Photon.Hashtable roomStartCoords = new ExitGames.Client.Photon.Hashtable();

    public void OnSetStartCoords(float startLon, float startLat)
	{
		this.startLon = startLon;
		this.startLat = startLat;

		roomStartCoords["StartLon"] = startLon;
		roomStartCoords["StartLat"] = startLat;

		PhotonNetwork.LocalPlayer.CustomProperties = roomStartCoords;

		//PhotonNetwork.LocalPlayer.CustomProperties.Add("StartLon", startLon);
	}

	private void Start()
	{
		//An example of how to set the custom properties
		OnSetStartCoords(0, 0);
	}

	private void GrabStartCoords()
	{
		float startLon = -1;
		if (player.CustomProperties.ContainsKey("StartLon"))
		{
			startLon = (float)player.CustomProperties["StartLon"];
		}

		float startLat = -1;
		if (player.CustomProperties.ContainsKey("StartLat"))
		{
			startLat = (float)player.CustomProperties["StartLat"];
		}
	}
}