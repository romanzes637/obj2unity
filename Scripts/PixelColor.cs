using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelColor : MonoBehaviour
{
	public Camera c;
	public Color centerColor;
	public Texture2D image;
	public float x = 0.5f;
	public float y = 0.5f;
	public float z = 0;
	public Vector3 uv;

	//	public GvrReticlePointer pointer;
	//	public float distance;
	//	public float stereoSeparation;
	//	public bool stereoEnabled;
	//	public Matrix4x4 a;
	//	public Matrix4x4 b;
	//	public Matrix4x4 d;
	//	public Matrix4x4 d2;
	//	public GvrEye eye;
	//	public Rect rect;
	//	public Rect rect2;
	//	public Vector3 pos;
	//	public Vector3 pos2;
	//	public Pose3D dist;
	//	public Vector3 pointerPos;

	// Use this for initialization
	void Start ()
	{
//		eye = GetComponent<GvrEye> ();
//		d = GvrViewer.Instance.Projection (eye.eye);
//		d2 = GvrViewer.Instance.Projection (eye.eye, GvrViewer.Distortion.Undistorted);
//		rect = GvrViewer.Instance.Viewport (eye.eye);
//		rect2 = GvrViewer.Instance.Viewport (eye.eye, GvrViewer.Distortion.Undistorted);
//		dist = GvrViewer.Instance.EyePose (eye.eye);
//		distance = c.stereoConvergence;
//		stereoSeparation = c.stereoSeparation;
//		stereoEnabled = c.stereoEnabled;
//		a = c.GetStereoViewMatrix (Camera.StereoscopicEye.Left);
//		b = c.GetStereoProjectionMatrix (Camera.StereoscopicEye.Left);
//		pos2 = c.WorldToViewportPoint (new Vector3 (-0.032f, 0f, 10f));
//		pos = GvrViewer.Controller.ComputeStereoEyePosition (eye.eye, d2 [1, 1], 10);
		image = new Texture2D (1, 1);
		c = GetComponent<Camera> ();
		uv = c.ViewportToScreenPoint (new Vector3 (x, y, z));
	}
		
	// Update is called once per frame
	void Update ()
	{
		UpdateCenterColor ();
	}

	void UpdateCenterColor ()
	{
		RenderTexture old = RenderTexture.active;
		RenderTexture.active = c.targetTexture;
		Rect r = new Rect ((int)uv.x, (int)uv.y, 1, 1);
		image.ReadPixels (r, 0, 0, true);
		centerColor = image.GetPixel (0, 0);
		RenderTexture.active = old;
	}
}
