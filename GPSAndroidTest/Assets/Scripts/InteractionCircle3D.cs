using UnityEngine;

public class InteractionCircle3D : MonoBehaviour
{
	public float radius = 5;
	public LayerMask groundLayer;

	private PlayerHealthStats healthStats;

	void Start()
	{
		healthStats = GetComponentInParent<PlayerHealthStats>();
		transform.localScale = new Vector3(radius, radius, 1);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 target = Vector3.zero;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000, groundLayer))
			{
				target = hit.point + new Vector3(0, 0.5f, 0);
			}

			//If the mouse is not inside the circle, return
			if (Vector3.Distance(target, transform.position) > radius)
			{
				Debug.Log("returned " + target);
				return;
			}

			//Check all colliders and eat any food
			Collider[] colliders = Physics.OverlapSphere(target, 1f);
			foreach (Collider collider in colliders)
			{
				Food food = collider.GetComponent<Food>();

				if (food != null)
				{
					food.Eat(); //Delete the food
					healthStats.EatFood(food); //Update the player health
					return;
				}
			}
		}
	}
}
