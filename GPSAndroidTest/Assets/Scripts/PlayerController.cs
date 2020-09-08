using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
	private Vector3 currentPlayerPosition = Vector2.zero;
	private Vector3 previousPlayerPosition = Vector2.zero;
	private Vector3 nonZeroLookingDirection;

	public float GPSMovementSpeed = 5;

	public bool mouseControls = false;
	public float mouseControlMovementSpeed = 5;

	// Update is called once per frame
	void Update()
	{
		if (!photonView.IsMine && PhotonNetwork.IsConnected)
		{
			return;
		}

		if (mouseControls)
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;
			Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

			previousPlayerPosition = transform.position;
			transform.position = Vector2.Lerp(transform.position, target, mouseControlMovementSpeed * Time.deltaTime);

			Vector3 currentLookingDirection = transform.position - previousPlayerPosition;

			if (currentLookingDirection != Vector3.zero)
			{
				nonZeroLookingDirection = currentLookingDirection;
			}

			float lookAngle = Mathf.Atan2(nonZeroLookingDirection.y, nonZeroLookingDirection.x) * Mathf.Rad2Deg;
			Quaternion targetRotation = Quaternion.Euler(0, 0, lookAngle);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 12);
		}

		if (!mouseControls && GPSLocation.Instance.IsGPSReady()) //Restrict movement if the GPS is not ready
		{
			//POSITION OF PLAYER
			previousPlayerPosition = currentPlayerPosition;
			currentPlayerPosition = GPSLocation.Instance.DeviceCurrentPosition();

			transform.position = Vector2.Lerp(transform.position, currentPlayerPosition, Time.deltaTime * GPSMovementSpeed);

			//ROTATION OF PLAYER
			Vector3 currentLookingDirection = currentPlayerPosition - previousPlayerPosition;

			if (currentLookingDirection != Vector3.zero)
			{
				nonZeroLookingDirection = currentLookingDirection;
			}

			float lookAngle = Mathf.Atan2(nonZeroLookingDirection.y, nonZeroLookingDirection.x) * Mathf.Rad2Deg;
			Quaternion target = Quaternion.Euler(0, 0, lookAngle);
			transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 12);
		}
	}
}