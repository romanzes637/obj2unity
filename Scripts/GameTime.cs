using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameTime : MonoBehaviour
{
	public float speed = 0.0f;
	public float time = 0.0f;

	public float Seconds { get { return time; } set { time = value; } }

	public float Years { get { return time / 3.154e+7f; } set { time = value * 3.154e+7f; } }

	public int currentIndex = 0;
	public int maxIndex;
	public float currentIndexTimeDelta = 0.0f;
	List<float> times;
	public bool isLoop = false;

	void Start ()
	{
		times = GetComponent<SharedData> ().times;
		maxIndex = times.Count - 1;
	}

	void EvaluateCurrentIndex ()
	{
		int index = -1;
		for (int i = 1; i < times.Count; i++) {
			if (times [i - 1] <= time && time < times [i]) {
				index = i - 1;
				break;
			}
		}
		if (index == -1) {
			index = times.Count - 1;
		}
		currentIndex = index;
	}

	void Update ()
	{
		time += Time.deltaTime * speed;
		EvaluateCurrentIndex ();
		currentIndexTimeDelta = time - times [currentIndex];
		if (currentIndex == maxIndex && isLoop) {
			time = 0.0f;
		}
	}

	public void SetTime (float value)
	{
		time = value;
	}
}
