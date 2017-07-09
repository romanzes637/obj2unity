using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Fenia : MonoBehaviour
{
	// Max faces in mesh = 65534/3 = 21844, where 65534 - max vertices in mesh
	int maxMeshFaces = 21844;
	public string objPath = "Modules/Unity/Obj/mesh.obj";
	public GameObject meshSample;
	public SharedData sharedData;
	public RelativeField relativeField;

	public List<GameObject> meshes = new List<GameObject> ();

	void Reset ()
	{
		sharedData = gameObject.AddComponent<SharedData> ();
		gameObject.AddComponent<GameTime> ();
		relativeField = gameObject.AddComponent<RelativeField> ();
	}

	public void Import ()
	{
		Debug.Log ("Clearing data");
		Clear ();

		Debug.Log ("Reading shared data");
		sharedData.Read ();

		Debug.Log ("Reading field");
		relativeField.Read ();

		Debug.Log ("Reading mesh");
		StreamReader sr = new StreamReader (Application.dataPath + "/" + objPath);
		List<List<float>> globalVertices = new List<List<float>> ();
//		List<List<float>> textureVertices = new List<List<float>> ();
		List<string> groupsNames = new List<string> ();
		List<List<List<int>>> groupsFacesVertices = new List<List<List<int>>> ();
//		List<List<List<int>>> groupsFacesTextureVertices = new List<List<List<int>>> ();
		List<List<int>> groupFacesVertices = new List<List<int>> ();
//		List<List<int>> groupFacesTextureVertices = new List<List<int>> ();

		bool isFirstGroup = true;
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			if (tokens [0] == "g") {
				groupsNames.Add (tokens [1]);
				if (isFirstGroup) {
					isFirstGroup = false;
				} else {
					groupsFacesVertices.Add (groupFacesVertices);
//					groupsFacesTextureVertices.Add (groupFacesTextureVertices);
				}
				groupFacesVertices = new List<List<int>> ();
//				groupFacesTextureVertices = new List<List<int>> ();
				continue;
			}
			if (tokens [0] == "v") {
				List<float> coordinates = new List<float> ();
				for (int i = 1; i < tokens.Length; i++) {
					coordinates.Add (float.Parse (tokens [i]));
				}
				globalVertices.Add (coordinates);
			} else if (tokens [0] == "vt") {
//				List<float> coordinates = new List<float> ();
//				for (int i = 1; i < tokens.Length; i++) {
//					coordinates.Add (float.Parse (tokens [i]));
//				}
//				textureVertices.Add (coordinates);
			} else if (tokens [0] == "f") {
				List<int> faceVertices = new List<int> ();
//				List<int> faceTextureVertices = new List<int> ();
				for (int i = 1; i < tokens.Length; i++) {
					string[] tokens2 = tokens [i].Split ('/');
					// Reduce vertex index by 1 (.obj format starts vertex indexing with 1)
					faceVertices.Add (int.Parse (tokens2 [0]) - 1);
					if (tokens2.Length > 1) {
//						faceTextureVertices.Add (int.Parse (tokens2 [1]) - 1);
					}
				}
				groupFacesVertices.Add (faceVertices);
//				groupFacesTextureVertices.Add (faceTextureVertices);
			}
		}
		if (groupFacesVertices.Count > 0) {
			groupsFacesVertices.Add (groupFacesVertices);
//			groupsFacesTextureVertices.Add (groupFacesTextureVertices);
		}
		sr.Close ();
		Debug.Log ("Number of vertices = " + globalVertices.Count);
//		Debug.Log ("Number of texture vertices = " + textureVertices.Count);
		Debug.Log ("Number of groups names = " + groupsNames.Count);
		Debug.Log ("Number of groups faces vertices = " + groupsFacesVertices.Count);
//		Debug.Log ("Number of groups faces texture vertices = " + groupsFacesTextureVertices.Count);

		Debug.Log ("Converting mesh");
		List<List<int>> groupsVerticesGlobalIndices = new List<List<int>> ();
		for (int i = 0; i < groupsFacesVertices.Count; i++) {
			Debug.Log (string.Format ("Creating {0}", groupsNames [i]));
			int nFaces = groupsFacesVertices [i].Count;
			Debug.Log (string.Format ("nFaces = {0}", nFaces));
			int nMeshParts = 1;
			if (nFaces > maxMeshFaces) {
				nMeshParts = Mathf.CeilToInt ((float)nFaces / maxMeshFaces);
			}
			Debug.Log (string.Format ("nMeshParts = {0}", nMeshParts));
			for (int k = 0; k < nMeshParts; k++) {
				// Creating mesh part
				int startFace = k * maxMeshFaces;
				int endFace;
				if (k != nMeshParts - 1) {
					endFace = (k + 1) * maxMeshFaces;
				} else {
					endFace = nFaces;
				}
				string childName;
				if (k != 0) {
					childName = string.Format ("{0}{1}", groupsNames [i], k + 1);
				} else {
					childName = string.Format ("{0}", groupsNames [i]);
				}
				Debug.Log (childName);

				GameObject childObject = Instantiate (meshSample, transform);
				childObject.name = childName;
				Mesh mesh = new Mesh ();
				mesh.name = childName;
				childObject.GetComponent<MeshFilter> ().mesh = mesh;
				meshes.Add (childObject);

				string childNameFlip = string.Format ("{0}Flip", childName);
				GameObject childObjectFlip = Instantiate (meshSample, transform);
				childObjectFlip.name = childNameFlip;
				Mesh meshFlip = new Mesh ();
				meshFlip.name = childNameFlip;
				childObjectFlip.GetComponent<MeshFilter> ().mesh = meshFlip;
				meshes.Add (childObjectFlip);

				// Creating triangles
				int[] ts = new int[(endFace - startFace) * 3];
				int[] tsFlip = new int[(endFace - startFace) * 3];
				// Dictionary map for perfomance
				Dictionary<int,int> global_to_local = new Dictionary<int,int> ();
				int n_local_vertices = 0;
				List<int> verticesGlobalIndices = new List<int> ();
				for (int j = startFace; j < endFace; j++) {
					List<int> verticesLocalIndices = new List<int> ();
					foreach (int vertexIndex in groupsFacesVertices [i] [j]) {
						if (!global_to_local.ContainsKey (vertexIndex)) {
							verticesGlobalIndices.Add (vertexIndex);
							global_to_local.Add (vertexIndex, n_local_vertices);
							n_local_vertices += 1;
						}
						verticesLocalIndices.Add (global_to_local [vertexIndex]);
					}
					ts [3 * (j - startFace)] = verticesLocalIndices [0];
					ts [3 * (j - startFace) + 1] = verticesLocalIndices [1];
					ts [3 * (j - startFace) + 2] = verticesLocalIndices [2];
					tsFlip [3 * (j - startFace)] = verticesLocalIndices [0];
					tsFlip [3 * (j - startFace) + 1] = verticesLocalIndices [2];
					tsFlip [3 * (j - startFace) + 2] = verticesLocalIndices [1];
				}

				// Creating vertices
				groupsVerticesGlobalIndices.Add (verticesGlobalIndices);
				groupsVerticesGlobalIndices.Add (verticesGlobalIndices);
				Vector3[] vs = new Vector3 [verticesGlobalIndices.Count];
				for (int j = 0; j < vs.Length; j++) {
					int vertexIndex = verticesGlobalIndices [j];
					// Swap Y and Z coordinates for unity coordinate system
					vs [j] = new Vector3 (globalVertices [vertexIndex] [0], 
						globalVertices [vertexIndex] [2], 
						globalVertices [vertexIndex] [1]); 
				}
				Debug.Log ("vertices size = " + vs.Length);
				mesh.vertices = vs;
				meshFlip.vertices = vs;
				mesh.triangles = ts;
				meshFlip.triangles = tsFlip;

				// Creating texture vertices
				Vector2[] uvs = new Vector2 [vs.Length];
				for (int j = 0; j < vs.Length; j++) {
					uvs [j] = Vector2.zero;
				}
				mesh.uv = uvs;
				meshFlip.uv = uvs;

				// Mesh functions
				mesh.RecalculateNormals ();
//				mesh.Optimize ();
				meshFlip.RecalculateNormals ();
//				meshFlip.Optimize ();
			}
		}
			
		Debug.Log ("Adding components to meshes");
		for (int i = 0; i < meshes.Count; i++) {
			meshes [i].AddComponent<SetRelativeField> ();
			meshes [i].AddComponent <GlobalVertices> ();
			meshes [i].GetComponent<GlobalVertices> ().globalVertices = groupsVerticesGlobalIndices [i];
		}
	}

	public void Clear ()
	{
		foreach (GameObject mesh in meshes) {
			DestroyImmediate (mesh);
		}
		meshes = new List<GameObject> ();
		sharedData.Clear ();
		relativeField.Clear ();
	}
}
