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
			float x = Input.GetAxis("Mouse X");
			float y = Input.GetAxis("Mouse Y");

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				freeLook.m_XAxis.Value = x;
			}
			else
			{
				freeLook.m_XAxis.Value = y;
			}
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
