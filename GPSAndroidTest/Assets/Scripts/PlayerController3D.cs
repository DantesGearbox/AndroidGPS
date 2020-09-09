using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviourPun
{
	private Vector3 currentPlayerPosition = Vector2.zero;
	private Vector3 previousPlayerPosition = Vector2.zero;
	private Vector3 nonZeroLookingDirection;

	public float GPSMovementSpeed = 5;

	public bool mouseControls = true;
	public float mouseControlMovementSpeed = 5;

	public Transform mousePosDebug;

	public LayerMask groundLayer;

	// Update is called once per frame
	void Update()
	{
		if (!photonView.IsMine && PhotonNetwork.IsConnected)
		{
			return;
		}

		if (mouseControls)
		{
			Vector3 mousePosition = new Vector3(0, 0.5f, 0);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000, groundLayer))
			{
				mousePosDebug.position = hit.point + new Vector3(0, 0.5f, 0);
				mousePosition = new Vector3(hit.point.x, 0.5f, hit.point.z);
			}

			previousPlayerPosition = transform.position;
			transform.position = Vector3.Lerp(transform.position, mousePosition, mouseControlMovementSpeed * Time.deltaTime);

			Vector3 currentLookingDirection = transform.position - previousPlayerPosition;

			if (currentLookingDirection != Vector3.zero)
			{
				nonZeroLookingDirection = currentLookingDirection;
			}

			float lookAngle = Mathf.Atan2(nonZeroLookingDirection.z, nonZeroLookingDirection.x) * Mathf.Rad2Deg;
			Quaternion targetRotation = Quaternion.Euler(90, 0, lookAngle);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 12);
		}

		if (!mouseControls && GPSLocation3D.Instance.IsGPSReady()) //Restrict movement if the GPS is not ready
		{
			//POSITION OF PLAYER
			previousPlayerPosition = currentPlayerPosition;
			currentPlayerPosition = GPSLocation3D.Instance.DeviceCurrentPosition();

			transform.position = Vector3.Lerp(transform.position, currentPlayerPosition, Time.deltaTime * GPSMovementSpeed);

			//ROTATION OF PLAYER
			Vector3 currentLookingDirection = currentPlayerPosition - previousPlayerPosition;

			if (currentLookingDirection != Vector3.zero)
			{
				nonZeroLookingDirection = currentLookingDirection;
			}

			float lookAngle = Mathf.Atan2(nonZeroLookingDirection.z, nonZeroLookingDirection.x) * Mathf.Rad2Deg;
			Quaternion target = Quaternion.Euler(90, 0, lookAngle);
			transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 12);
		}
	}
}