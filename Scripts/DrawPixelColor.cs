using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPixelColor : MonoBehaviour
{
	public Image i;
	public Image i2;
	public PixelColor pc;
	public PixelColor pc2;
	bool done = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (!done) {
			Camera[] cs = Camera.main.GetComponentsInChildren<Camera> ();
			pc = cs[1].gameObject.GetComponent<PixelColor> ();
			pc2 = cs[2].gameObject.GetComponent<PixelColor> ();
			done = true;
		}
		i.color = pc.centerColor;
		i2.color = pc2.centerColor;
	}
}
