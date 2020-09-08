using UnityEngine;

public class InteractionCircle : MonoBehaviour
{
	public float radius = 8;

	void Start()
	{
		transform.localScale = new Vector3(radius, radius, 1);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			//Get mouse position
			Vector3 mousePos = Input.mousePosition;
			Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
			target.z = transform.position.z;

			//If the mouse is not inside the circle, return
			if (Vector3.Distance(target, transform.position) > radius)
			{
				Debug.Log("returned " + target);
				return;
			}

			//Check all colliders and eat any food
			Collider2D[] colliders = Physics2D.OverlapCircleAll(target, 1f);
			foreach (Collider2D collider in colliders)
			{
				Food food = collider.GetComponent<Food>();

				if(food != null)
				{
					food.Eat();
					return;
				}
			}
		}
	}
}
