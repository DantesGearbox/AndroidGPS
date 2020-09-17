using System.Collections.Generic;
using UnityEngine;

public class Food : Interactable
{
	public float iodineRegain = 0;
	public float ironRegain = 0;
	public float vitaminARegain = 0;
	public float vitaminBRegain = 0;
	public float vitaminCRegain = 0;
	public float vitaminDRegain = 0;

	public float appetiteFilling = 0;

	public void Eat()
	{
		Debug.Log("Ate food with name: " + name);
		Destroy(this.gameObject);
	}
}