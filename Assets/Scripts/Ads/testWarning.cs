using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testWarning : MonoBehaviour {

	private string text;
	private GUIStyle style;

	void OnGUI () 
	{
		if(style==null) style=new GUIStyle(GUI.skin.box);
		style.fontSize=Screen.height/10;
		GUI.Box(new Rect (0, Screen.height / 3, Screen.width, Screen.height/5), text,style);
		if(GUI.Button(new Rect(0,0,Screen.width / 5,Screen.height / 10), "close"))
		{
			Destroy(gameObject);
		}			
	}


	public static void Open (string s)
	{
		GameObject g = new GameObject("message");
		testWarning t = g.AddComponent<testWarning>();
		t.text = s;
	}
}
