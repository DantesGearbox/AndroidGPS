using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStats : MonoBehaviour
{
	[Header("Appetite")]
	public float appetiteIncreasePerSecond = 1;
	public float appetiteCurrentLevel = 100;
	public float appetiteMaxLevel = 100;

	[Header("Overall health")]
	public float overallCurrentHealth = 0;
	public float overallMaxHealth = 0;

	[Header("Nutrient Levels")]
	public NutrientLevel iodineLevel = new NutrientLevel(5, 100, 0);
	public NutrientLevel ironLevel = new NutrientLevel(5, 100, 0);
	public NutrientLevel vitaminALevel = new NutrientLevel(5, 100, 0);
	public NutrientLevel vitaminBLevel = new NutrientLevel(5, 100, 0);
	public NutrientLevel vitaminCLevel = new NutrientLevel(5, 100, 0);
	public NutrientLevel vitaminDLevel = new NutrientLevel(5, 100, 0);

	// Update is called once per frame
	void Update()
	{
		//increase appetite
		if(appetiteCurrentLevel < appetiteMaxLevel)
		{
			appetiteCurrentLevel += appetiteIncreasePerSecond * Time.deltaTime;
		}
	}

	public void EatFood(Food food)
	{
		appetiteCurrentLevel -= food.appetiteFilling;

		SetNutrientLevels(food);

		SetOverallHealth();
	}

	private void SetNutrientLevels(Food food)
	{
		iodineLevel.currentNutrientLevel += food.iodineRegain;
		ironLevel.currentNutrientLevel += food.ironRegain;
		vitaminALevel.currentNutrientLevel += food.vitaminARegain;
		vitaminBLevel.currentNutrientLevel += food.vitaminBRegain;
		vitaminCLevel.currentNutrientLevel += food.vitaminCRegain;
		vitaminDLevel.currentNutrientLevel += food.vitaminDRegain;
	}

	private void SetOverallHealth()
	{
		overallMaxHealth = iodineLevel.maxNutrientLevel + ironLevel.maxNutrientLevel +
			vitaminALevel.maxNutrientLevel + vitaminBLevel.maxNutrientLevel +
			vitaminCLevel.maxNutrientLevel + vitaminDLevel.maxNutrientLevel;

		overallCurrentHealth = iodineLevel.currentNutrientLevel + ironLevel.currentNutrientLevel +
			vitaminALevel.currentNutrientLevel + vitaminBLevel.currentNutrientLevel +
			vitaminCLevel.currentNutrientLevel + vitaminDLevel.currentNutrientLevel;
	}
	
	[System.Serializable]
	public struct NutrientLevel
	{
		public float currentNutrientLevel;
		public float maxNutrientLevel;
		public float minNutrientLevel;

		public NutrientLevel(float current, float max, float min)
		{
			currentNutrientLevel = current;
			maxNutrientLevel = max;
			minNutrientLevel = min;
		}
	}
}
