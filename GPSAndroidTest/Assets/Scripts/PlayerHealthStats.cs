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

	private void Start()
	{
		//The nutrient values are set in the editor.
		//The overall health is based on the nutrient values.

		SetOverallHealth();
		InitialUIBarSetup();
	}

	private void InitialUIBarSetup()
	{
		UIBars.Instance.iodineBar.maximum = iodineLevel.maxNutrientLevel;
		UIBars.Instance.iodineBar.minimum = iodineLevel.minNutrientLevel;

		UIBars.Instance.ironBar.maximum = ironLevel.maxNutrientLevel;
		UIBars.Instance.ironBar.minimum = ironLevel.minNutrientLevel;

		UIBars.Instance.vitaminABar.maximum = vitaminALevel.maxNutrientLevel;
		UIBars.Instance.vitaminABar.minimum = vitaminALevel.minNutrientLevel;

		UIBars.Instance.vitaminBBar.maximum = vitaminBLevel.maxNutrientLevel;
		UIBars.Instance.vitaminBBar.minimum = vitaminBLevel.minNutrientLevel;

		UIBars.Instance.vitaminCBar.maximum = vitaminCLevel.maxNutrientLevel;
		UIBars.Instance.vitaminCBar.minimum = vitaminCLevel.minNutrientLevel;

		UIBars.Instance.vitaminDBar.maximum = vitaminDLevel.maxNutrientLevel;
		UIBars.Instance.vitaminDBar.minimum = vitaminDLevel.minNutrientLevel;

		UIBars.Instance.overallHealthBar.maximum = overallMaxHealth;
		UIBars.Instance.overallHealthBar.minimum = 0;

		UIBars.Instance.appetiteBar.maximum = appetiteMaxLevel;
		UIBars.Instance.appetiteBar.minimum = 0;
	}

	private void UpdateNutrientUIBars()
	{
		UIBars.Instance.iodineBar.current = iodineLevel.currentNutrientLevel;
		UIBars.Instance.ironBar.current = ironLevel.currentNutrientLevel;
		UIBars.Instance.vitaminABar.current = vitaminALevel.currentNutrientLevel;
		UIBars.Instance.vitaminBBar.current = vitaminBLevel.currentNutrientLevel;
		UIBars.Instance.vitaminCBar.current = vitaminCLevel.currentNutrientLevel;
		UIBars.Instance.vitaminDBar.current = vitaminDLevel.currentNutrientLevel;
	}

		// Update is called once per frame
	void Update()
	{
		//increase appetite
		if(appetiteCurrentLevel < appetiteMaxLevel)
		{
			appetiteCurrentLevel += appetiteIncreasePerSecond * Time.deltaTime;
		}
		UIBars.Instance.appetiteBar.current = appetiteCurrentLevel;
	}

	public void EatFood(Food food)
	{
		appetiteCurrentLevel -= food.appetiteFilling;
		if(appetiteCurrentLevel < 0)
		{
			appetiteCurrentLevel = 0;
		}
		UIBars.Instance.appetiteBar.current = appetiteCurrentLevel;

		SetNutrientLevels(food);
		UpdateNutrientUIBars();

		SetOverallHealth();
		UIBars.Instance.overallHealthBar.current = overallCurrentHealth;
	}

	private void SetNutrientLevels(Food food)
	{
		iodineLevel = IncreaseNutrientLevel(iodineLevel, food.iodineRegain);
		ironLevel = IncreaseNutrientLevel(ironLevel, food.ironRegain);
		vitaminALevel = IncreaseNutrientLevel(vitaminALevel, food.vitaminARegain);
		vitaminBLevel = IncreaseNutrientLevel(vitaminBLevel, food.vitaminBRegain);
		vitaminCLevel = IncreaseNutrientLevel(vitaminCLevel, food.vitaminCRegain);
		vitaminDLevel = IncreaseNutrientLevel(vitaminDLevel, food.vitaminDRegain);
	}

	private NutrientLevel IncreaseNutrientLevel(NutrientLevel nutrientLevel, float increase)
	{
		nutrientLevel.currentNutrientLevel += increase;
		if(nutrientLevel.currentNutrientLevel + increase > nutrientLevel.maxNutrientLevel)
		{
			nutrientLevel.currentNutrientLevel = nutrientLevel.maxNutrientLevel;
		}

		return nutrientLevel;
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
