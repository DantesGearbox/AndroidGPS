using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
	public float playAreaX = 45f;
	public float playAreaY = 80f;

	public float foodSpawnTimer = 0f;
	private float foodSpawnTime = 5f;

	public List<Food> foodTypes;

	void Start()
	{
		transform.localScale = new Vector3(playAreaX, playAreaY, 1);
	}

	void Update()
	{
		if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
		{
			return;
		}
		foodSpawnTimer += Time.deltaTime;
		if(foodSpawnTimer > foodSpawnTime)
		{
			SpawnFood();
			foodSpawnTimer = 0;
		}
	}

	private void SpawnFood()
	{
		int i = Random.Range(0, foodTypes.Count);
		Food toSpawn = foodTypes[i];

		float spawnX = Random.Range(-playAreaX / 2, playAreaX / 2);
		float spawnZ = Random.Range(-playAreaY / 2, playAreaY / 2);

		Quaternion spawnAngle = Quaternion.Euler(0, 0, 0);
		Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);
		PhotonNetwork.Instantiate("Foods/" + toSpawn.name, spawnPosition, spawnAngle, 0);

		Debug.Log("Spawned food: " + toSpawn.name);
	}
}
