using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPixelColors : MonoBehaviour
{
	bool done = false;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!done) {
			Camera[] cs = GetComponentsInChildren<Camera> ();
//			GvrReticlePointer pointer = GetComponentInChildren<GvrReticlePointer> (true);
			foreach (Camera c in cs) {
				if (c.gameObject != gameObject) {
					c.gameObject.AddComponent<PixelColor> ();
//					c.gameObject.GetComponent<PixelColor> ().pointer = pointer;
				}
			}
			done = true;
		}	
	}
}
