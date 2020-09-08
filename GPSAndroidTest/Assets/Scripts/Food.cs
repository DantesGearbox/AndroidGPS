using System.Collections.Generic;
using UnityEngine;

public class Food : Interactable
{
	public void Eat()
	{
		Debug.Log("Ate food with name: " + name);
		Destroy(this.gameObject);
	}
}