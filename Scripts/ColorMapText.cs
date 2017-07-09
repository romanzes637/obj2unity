using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMapText : MonoBehaviour
{
	public Text minValue;
	public Text maxValue;
	public Text currentMinValue;
	public Text currentMaxValue;

	public SharedData data;
	public RelativeField field;

	public int fieldIndex;

	// Use this for initialization
	void Start ()
	{
		minValue.text = string.Format ("{0:F0}", data.fieldsMin [fieldIndex]);
		maxValue.text = string.Format ("{0:F0}", data.fieldsMax [fieldIndex]);
	}
	
	void LateUpdate ()
	{
		currentMinValue.text = string.Format ("Current min: {0:F0}", Mathf.Lerp (data.fieldsMin [fieldIndex], data.fieldsMax [fieldIndex], field.minValue));
		currentMaxValue.text = string.Format ("Current max: {0:F0}", Mathf.Lerp (data.fieldsMin [fieldIndex], data.fieldsMax [fieldIndex], field.maxValue));
	}
}
