using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetGlobalField : MonoBehaviour
{
	public List<List<float>> values;
	public List<List<float>> velocities;
	public bool isParentField = false;
	public RelativeField rf;

	void Start ()
	{
//		if (!isParentField) {
//			values = GetComponent<RelativeField> ().values;
//			velocities = GetComponent<RelativeField> ().velocities;
//		} else {
//			rf = transform.parent.GetComponent<RelativeField> ();
//		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isParentField) {
			int currentIndex = GetComponent<GameTime> ().currentIndex;
			int maxIndex = GetComponent<GameTime> ().maxIndex;
			float currentTimeDelta = GetComponent<GameTime> ().currentIndexTimeDelta;
			if (currentIndex < maxIndex) {
				Mesh mesh = GetComponent<MeshFilter> ().mesh;
				Color[] colors = new Color[mesh.vertices.Length];
				for (int i = 0; i < colors.Length; i++) {
					float value = values [currentIndex] [i] + currentTimeDelta * velocities [currentIndex] [i];
					if (value < 0.5) {
						colors [i] = Color.Lerp (Color.blue, Color.green, value * 2f);
					} else {
						colors [i] = Color.Lerp (Color.green, Color.red, (value - 0.5f) * 2f);
					}
				}
				mesh.colors = colors;
			}
		} else {
//			Color[] colors = rf.colors;
//			Mesh mesh = GetComponent<MeshFilter> ().mesh;
//			mesh.colors = colors;
			GetComponent<MeshFilter> ().mesh.colors = rf.colors;
		}
	}
}
