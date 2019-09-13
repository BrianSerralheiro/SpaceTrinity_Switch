using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour {
	private static GameObject go;
	private static Text text;
	void Start () {
		go=gameObject;
		text=GetComponentInChildren<Text>();
		go.SetActive(false);
	}
	
	public static void Open(string s)
	{
		if(go)
		{
			text.text=s;
			go.SetActive(true);
		}
	}
}
