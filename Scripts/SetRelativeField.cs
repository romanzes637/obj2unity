using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SetRelativeField : MonoBehaviour
{
	public List<int> globalVertices;
	public RelativeField relativeField;
	public Color[] colors;

	void Start ()
	{
		relativeField = transform.parent.GetComponent<RelativeField> ();
		globalVertices = GetComponent<GlobalVertices> ().globalVertices;
		colors = new Color[globalVertices.Count];
	}

	void LateUpdate ()
	{
		if (relativeField.isUpdateColors) {
//			UpdateColors ();
			StartCoroutine (UpdateColorsCoroutine ());
		}
	}

	void UpdateColors ()
	{
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = relativeField.colors [globalVertices [i]];
		}
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.colors = colors;
	}

	IEnumerator UpdateColorsCoroutine ()
	{
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = relativeField.colors [globalVertices [i]];
		}
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.colors = colors;

		yield return new WaitForEndOfFrame ();
	}
}
