using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	DeviceOrientation orientation;
	public AutoWalk autoWalk;
	public Canvas menu;
	public GvrReticlePointer pointer;
	public Builder builder;

	public bool showPointer;

	public bool ShowPointer{ set { showPointer = value; } }

	public bool showMenu;

	public bool ShowMenu{ set { showMenu = value; } }

	// Use this for initialization
	void Start ()
	{
		menu.gameObject.SetActive (showMenu);
		pointer.gameObject.SetActive (showPointer);
		orientation = Input.deviceOrientation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (showPointer != pointer.gameObject.activeSelf && !menu.gameObject.activeSelf) {
			pointer.gameObject.SetActive (true);
		}
		if (showMenu != menu.gameObject.activeSelf) {
			menu.gameObject.SetActive (showMenu);
			if (menu.gameObject.activeSelf) {
				pointer.gameObject.SetActive (true);
				UpdateMenuTransform ();
				Handheld.Vibrate ();
			}
		}
		if (Input.GetMouseButtonDown (1) || (
		        orientation != Input.deviceOrientation && (
		            Input.deviceOrientation == DeviceOrientation.Portrait ||
		            Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown))) {
			Debug.Log ("mouse 1 down");
			autoWalk.move = false;
			showMenu = !showMenu;
			orientation = Input.deviceOrientation;
		} else if (orientation != Input.deviceOrientation) {
			orientation = Input.deviceOrientation;
		}
		if (Input.GetMouseButtonDown (0) && !menu.gameObject.activeSelf) {
			Debug.Log ("mouse 0 down");
			autoWalk.move = !autoWalk.move;
		}
	}

	public void UpdateMenuTransform ()
	{
		menu.gameObject.transform.position = Camera.main.transform.position
		+ Camera.main.transform.TransformDirection (Vector3.forward);
		// Don't use camera rotation because while rotating a phone 
		// the camera rotation set is about 45 degrees (bad view)
		menu.gameObject.transform.rotation = 
		Quaternion.Euler (0, Camera.main.transform.rotation.eulerAngles.y, 0);
//		menu.gameObject.transform.rotation = Camera.main.transform.rotation;
	}
}
