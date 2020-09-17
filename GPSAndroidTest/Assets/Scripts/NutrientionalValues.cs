using UnityEngine;

public enum NutrientTypes { Iodine, Iron, VitaminA, VitaminB, VitaminC, VitaminD };

[System.Serializable]
public class NutrientionalValues
{
	public NutrientRegain iodineRegain;
	public NutrientRegain ironRegain;
	public NutrientRegain vitaminARegain;
	public NutrientRegain vitaminBRegain;
	public NutrientRegain vitaminCRegain;
	public NutrientRegain vitaminDRegain;

	public NutrientionalValues(float iodineRegain, float ironRegain, float vitARegain, float vitBRegain, float vitCRegain, float vitDRegain)
	{
		this.iodineRegain = new NutrientRegain(NutrientTypes.Iodine, iodineRegain);
		this.ironRegain = new NutrientRegain(NutrientTypes.Iron, ironRegain);
		this.vitaminARegain = new NutrientRegain(NutrientTypes.VitaminA, vitARegain);
		this.vitaminBRegain = new NutrientRegain(NutrientTypes.VitaminB, vitBRegain);
		this.vitaminCRegain = new NutrientRegain(NutrientTypes.VitaminC, vitCRegain);
		this.vitaminDRegain = new NutrientRegain(NutrientTypes.VitaminD, vitDRegain);
	}
}

[System.Serializable]
public struct NutrientRegain
{
	public NutrientTypes nutrientType;
	public float regain;

	public NutrientRegain(NutrientTypes nutrientType, float regain)
	{
		this.nutrientType = nutrientType;
		this.regain = regain;
	}
}