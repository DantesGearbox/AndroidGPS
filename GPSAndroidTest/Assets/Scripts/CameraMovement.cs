using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public static CameraMovement Instance;

    public CinemachineFreeLook freeLook;

    private bool isDragging;

	private void Start()
	{
		Instance = this;
	}

	void Update()
    {
		if (isDragging)
		{
			freeLook.m_XAxis.Value = Input.GetAxis("Mouse X");
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
            isDragging = true;
		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			isDragging = false;
		}
    }
}
