using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBars : MonoBehaviour
{
	public static UIBars Instance;

	public StatusBar overallHealthBar;
	public StatusBar appetiteBar;
	public StatusBar iodineBar;
	public StatusBar ironBar;
	public StatusBar vitaminABar;
	public StatusBar vitaminBBar;
	public StatusBar vitaminCBar;
	public StatusBar vitaminDBar;

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
	}
}
