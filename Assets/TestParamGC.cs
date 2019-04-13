using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParamGC : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (begin) {
			for (int i = 0; i < 100000; i++)
				TestParamGCFunc (1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
		}
	}

	bool begin = false;

	void OnGUI ()
	{
		if (GUILayout.Button ("TestParamGC")) {
			begin = true;
		}
		if (GUILayout.Button ("TestParamGC Stop")) {
			begin = false;
		}
	}

	void TestParamGCFunc (params int[] pa)
	{
		for (int i = 0, imax = pa.Length; i < imax; i++) {
			continue;
		}
	}
}
