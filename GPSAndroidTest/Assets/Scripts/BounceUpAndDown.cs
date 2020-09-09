using UnityEngine;

public class BounceUpAndDown : MonoBehaviour
{
	public LeanTweenType easeType;
	public float distance = 1;
	public float duration = 1;

	private LTDescr tweenObject = null;

	private void Start()
	{
		tweenObject = LeanTween.move(gameObject, transform.position + (Vector3.up * distance), duration);
		tweenObject.setLoopPingPong();
		tweenObject.setEase(easeType);
	}
}
