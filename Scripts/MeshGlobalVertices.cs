using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MeshGlobalVertices : MonoBehaviour {

	public string face_zone_vertices_path;
	public List<int> vertices = new List<int> ();

	// Use this for initialization
	void Start () {
		face_zone_vertices_path = "./Assets/Modules/Unity/Obj/FaceZonesVertices/" + this.gameObject.name;
		StreamReader sr = new StreamReader (face_zone_vertices_path);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			foreach (string item in tokens) {
				vertices.Add (int.Parse (item));
			}
		}
		sr.Close ();
	}

	// Update is called once per frame
	void Update () {

	}
}
