using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SharedData : MonoBehaviour
{
	public string timesPath = "Modules/Unity/times";
	public string fieldsListPath = "Modules/Unity/fields_list";
	public string fieldsMinPath = "Modules/Unity/fields_min";
	public string fieldsMaxPath = "Modules/Unity/fields_max";
	public List<float> times = new List<float> ();
	public List<float> fieldsMin = new List<float> ();
	public List<float> fieldsMax = new List<float> ();
	public List<string> fieldsList = new List<string> ();

	public void Read ()
	{
		times = new List<float> ();
		StreamReader sr = new StreamReader (Application.dataPath + "/" + timesPath);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			foreach (string item in tokens) {
				times.Add (float.Parse (item));
			}
		}
		sr.Close ();

		fieldsMin = new List<float> ();
		sr = new StreamReader (Application.dataPath + "/" + fieldsMinPath);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			foreach (string item in tokens) {
				fieldsMin.Add (float.Parse (item));
			}
		}
		sr.Close ();

		fieldsMax = new List<float> ();
		sr = new StreamReader (Application.dataPath + "/" + fieldsMaxPath);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			foreach (string item in tokens) {
				fieldsMax.Add (float.Parse (item));
			}
		}
		sr.Close ();

		fieldsList = new List<string> ();
		sr = new StreamReader (Application.dataPath + "/" + fieldsListPath);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			foreach (string item in tokens) {
				fieldsList.Add (item);
			}
		}
		sr.Close ();
	}

	public void Clear ()
	{
		times = new List<float> ();
		fieldsMin = new List<float> ();
		fieldsMax = new List<float> ();
		fieldsList = new List<string> ();
	}
}
