using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Utility class that let you see normals and tangent vectors for a mesh.
/// This is really useful when debugging mesh appearance
/// </summary>
[RequireComponent (typeof(MeshFilter))]
public class MeshDisplay : MonoBehaviour
{
	public Material mat;
	public bool showNormals = false;
	public bool showTangents = false;
	public bool showVertices = false;
	public bool showVerticesCoordinates = false;
	public bool showColors = false;

	public bool showBounds = false;

	public float displayLengthScale = 1.0f;

	public Color normalColor = Color.red;
	public Color tangentColor = Color.blue;
	public Color boundsColor = Color.green;

	void OnDrawGizmosSelected ()
	{
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			Debug.LogWarning ("Cannot find MeshFilter");
			return;
		}
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null) {
			Debug.LogWarning ("Cannot find mesh");
			return;
		}
		bool doShowNormals = showNormals && mesh.normals.Length == mesh.vertices.Length;
		bool doShowTangents = showTangents && mesh.tangents.Length == mesh.vertices.Length;
		bool doShowColors = showColors && mesh.colors.Length == mesh.vertices.Length;

		if (doShowNormals) {
			Gizmos.color = normalColor;
			GL.Begin (GL.LINES);
			mat.SetPass (0);
			GL.Color (normalColor);
			for (int i = 0; i < mesh.vertices.Length; i++) {
				Vector3 vertex = transform.TransformPoint (mesh.vertices [i]);
				Vector3 normal = transform.TransformDirection (mesh.normals [i]);
				GL.Vertex (vertex);
				GL.Vertex (vertex + normal * displayLengthScale);
//				Gizmos.DrawLine (vertex, vertex + normal * displayLengthScale);
			}
			GL.End ();
		}

		if (doShowTangents) {
			Gizmos.color = tangentColor;
			foreach (int idx in mesh.triangles) {
				Vector3 vertex = transform.TransformPoint (mesh.vertices [idx]);
				Vector3 tangent = transform.TransformDirection (mesh.tangents [idx]);
				Gizmos.DrawLine (vertex, vertex + tangent * displayLengthScale);
			}
		}


		if (showVertices) {
			List<Vector3> positions = new List<Vector3> ();
			List<string> labels = new List<string> ();
			for (int i = 0; i < mesh.vertices.Length; i++) {
				Vector3 pos = transform.TransformPoint (meshFilter.sharedMesh.vertices [i]);
				if (!positions.Contains (pos)) {
					positions.Add (pos);
					labels.Add (i.ToString ());
				} else {
					labels [positions.IndexOf (pos)] += (" " + i.ToString ());
				}

				if (doShowColors) {
					UnityEditor.Handles.Label (mesh.vertices [i], mesh.colors [i].ToString ());
				}
			}

			for (int i = 0; i < positions.Count; i++) {
				if (showVertices) {
					UnityEditor.Handles.Label (positions [i], labels [i]);
				}
				if (showVerticesCoordinates) {
					Vector3 coordinatesPos = positions [i] + Vector3.up * 0.1f;
					UnityEditor.Handles.Label (coordinatesPos, positions [i].ToString ());
				}
			}
		}

		if (showBounds) {
			Gizmos.color = boundsColor;
			Gizmos.DrawWireCube (transform.position + mesh.bounds.center, meshFilter.sharedMesh.bounds.size);
		}
	}
}