using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle.Logic.FogAbout;

public class Test2 : MonoBehaviour
{
	public RawImage ri;
	public GameObject obj;
	public int delta_x;
	public int delta_y;
	public int r;
	FogBaseData kit;

	// Use this for initialization
	void Start ()
	{
		kit = new FogBaseData (2048, 1024);
		ri.texture = kit.SaveTexture ();
	}

	void OnGUI ()
	{
		if (GUILayout.Button ("Print")) {
			kit.UpdateTransparent ((int) (obj.transform.localPosition.x + delta_x), (int) (obj.transform.localPosition.y + delta_y), r);
			ri.texture = kit.SaveTexture ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
