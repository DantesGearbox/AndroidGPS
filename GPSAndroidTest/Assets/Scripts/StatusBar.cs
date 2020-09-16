using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatusBar : MonoBehaviour
{
	public Text statusTitleTextElement;
	public Image mask;
	public Image fill;

	public float current;
	public float minimum;
	public float maximum;

	public string barName = "Bar";

	public Color color = Color.green;

	private void Start()
	{
		SetStatusTitle();
		GetCurrentFill();
	}

	private void Update()
    {
		GetCurrentFill();
    }

	private void GetCurrentFill()
	{
		float currentOffset = current - minimum;
		float maximumOffset = maximum - minimum;
		float fillAmount = currentOffset / maximumOffset;
		mask.fillAmount = fillAmount;

		fill.color = color;
	}

	private void SetStatusTitle()
	{
		statusTitleTextElement.text = barName;
	}
}
