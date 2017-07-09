using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(Fenia))]
public class FeniaEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		Fenia fenia = (Fenia)target;
		if (GUILayout.Button ("Import")) {
			fenia.Import ();
		}
	}
}