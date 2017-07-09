using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RelativeField : MonoBehaviour
{
	public bool isUpdateColors = true;
	public int nVertices;
	public int verticesLimit = 0;
	public string valuesPath = "Modules/Unity/RelativeFields/T";
	public string velocitiesPath = "Modules/Unity/RelativeFieldsVelocities/T";
	public float[] values;
	public float[] velocities;
	public Color[] colors;
	public float previousTime = -1.0f;
	public float maxValue = 0.0f;
	public float minValue = 0.0f;

	public void Read ()
	{
		Debug.Log ("Reading relative field T");
		// Read values
		StreamReader sr = new StreamReader (Application.dataPath + "/" + valuesPath);
		List<List<float>> data = new List<List<float>> ();
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			List<float> tokensf = new List<float> ();
			foreach (string item in tokens) {
				tokensf.Add (float.Parse (item));
			}
			data.Add (tokensf);
		}
		sr.Close ();
		Debug.Log ("nValuesSteps = " + data.Count);
		Debug.Log ("nVertices = " + data [0].Count);

		// Set number of verticies
		nVertices = data [0].Count;

		// Set values
		int counter = 0;
		values = new float[data.Count * nVertices];
		for (int i = 0; i < data.Count; i++) {
			for (int j = 0; j < nVertices; j++) {
				values [counter] = data [i] [j];
				counter++;
			}
		}
		Debug.Log ("values.Length = " + values.Length);

		// Read velocities
		data = new List<List<float>> ();
		sr = new StreamReader (Application.dataPath + "/" + velocitiesPath);
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			string[] tokens = line.Split ();
			List<float> tokensf = new List<float> ();
			foreach (string item in tokens) {
				tokensf.Add (float.Parse (item));
			}
			data.Add (tokensf);
		}
		sr.Close ();
		Debug.Log ("nVelocitiesSteps = " + data.Count);
		Debug.Log ("nVertices = " + data [0].Count);

		// Set velocities
		counter = 0;
		velocities = new float[data.Count * data [0].Count];
		for (int i = 0; i < data.Count; i++) {
			for (int j = 0; j < data [0].Count; j++) {
				velocities [counter] = data [i] [j];
				counter++;
			}
		}
		Debug.Log ("velocities.Length = " + velocities.Length);

		// Initialize colors
		colors = new Color[nVertices];

		// Set default vertices limit
		verticesLimit = nVertices;
	}

	void Update ()
	{
		float currentTime = GetComponent<GameTime> ().time;
		if (currentTime != previousTime) {
			isUpdateColors = true;
		} else {
			isUpdateColors = false;
		}

		if (isUpdateColors) {
//			UpdateColors ();
			StartCoroutine (UpdateColorsCoroutine ());
		}

		previousTime = currentTime;
	}

	void UpdateColors ()
	{
		int currentIndex = GetComponent<GameTime> ().currentIndex;
		int	maxIndex = GetComponent<GameTime> ().maxIndex;
		float currentIndexTimeDelta = GetComponent<GameTime> ().currentIndexTimeDelta;
		for (int i = 0; i < verticesLimit; i++) {
			float value;
			if (currentIndex < maxIndex) {
				value = values [currentIndex * nVertices + i] + currentIndexTimeDelta * velocities [currentIndex * nVertices + i];
			} else {
				value = values [maxIndex * nVertices + i];
			}
			if (value < 0.25) {
				colors [i] = Color.Lerp (Color.blue, Color.cyan, value * 4f);
			} else if (value < 0.5) {
				colors [i] = Color.Lerp (Color.cyan, Color.green, (value - 0.25f) * 4f);
			} else if (value < 0.75) {
				colors [i] = Color.Lerp (Color.green, Color.yellow, (value - 0.5f) * 4f);
			} else {
				colors [i] = Color.Lerp (Color.yellow, Color.red, (value - 0.75f) * 4f);
			}
			if (i == 0) {
				maxValue = value;
				minValue = value;
			} else if (value > maxValue) {
				maxValue = value;
			} else if (value < minValue) {
				minValue = value;
			}
		}
	}

	IEnumerator UpdateColorsCoroutine ()
	{
		int currentIndex = GetComponent<GameTime> ().currentIndex;
		int	maxIndex = GetComponent<GameTime> ().maxIndex;
		float currentIndexTimeDelta = GetComponent<GameTime> ().currentIndexTimeDelta;
		for (int i = 0; i < verticesLimit; i++) {
			float value;
			if (currentIndex < maxIndex) {
				value = values [currentIndex * nVertices + i] + currentIndexTimeDelta * velocities [currentIndex * nVertices + i];
			} else {
				value = values [maxIndex * nVertices + i];
			}
			if (value < 0.25) {
				colors [i] = Color.Lerp (Color.blue, Color.cyan, value * 4f);
			} else if (value < 0.5) {
				colors [i] = Color.Lerp (Color.cyan, Color.green, (value - 0.25f) * 4f);
			} else if (value < 0.75) {
				colors [i] = Color.Lerp (Color.green, Color.yellow, (value - 0.5f) * 4f);
			} else {
				colors [i] = Color.Lerp (Color.yellow, Color.red, (value - 0.75f) * 4f);
			}
			if (i == 0) {
				maxValue = value;
				minValue = value;
			} else if (value > maxValue) {
				maxValue = value;
			} else if (value < minValue) {
				minValue = value;
			}
		}
		yield return null;
	}


	public void Clear ()
	{
		nVertices = 0;
		values = new float[0];
		velocities = new float[0];
		colors = new Color[0];
		previousTime = -1.0f;
		maxValue = 0.0f;
		minValue = 0.0f;
	}
}
