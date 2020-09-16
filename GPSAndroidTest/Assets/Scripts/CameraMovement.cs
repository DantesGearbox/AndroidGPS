using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public static CameraMovement Instance;

    public CinemachineFreeLook freeLook;

	public CinemachineTargetGroup targetGroup;

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

			Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

			float invertYRotation = 1;
			float invertXRotation = 1;

			if(mousePos.x < 0.5)
			{
				invertYRotation = -1;
			}
			if (mousePos.y > 0.5)
			{
				invertXRotation = -1;
			}

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				freeLook.m_XAxis.Value = x * invertXRotation;
			}
			else
			{
				freeLook.m_XAxis.Value = y * invertYRotation;
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

	public void AddLookAt(Transform transform)
	{
		targetGroup.AddMember(transform, 1, 1);
	}

	public void SetLookAt(Transform transform)
	{
		freeLook.m_LookAt = transform;
	}

	public void SetFollow(Transform transform)
	{
		freeLook.m_Follow = transform;
	}
}
