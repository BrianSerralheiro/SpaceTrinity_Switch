using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDialog : MonoBehaviour {
	private static GameObject dialog;
	[SerializeField]
	private Sprite[] box;
	private static Sprite[] _box;
	private static Image dialogBox;
	private static Text text;
	private static string fulltext;
	private static float chars;
	void Start () {
		dialogBox=GetComponentInChildren<Image>();
		text=GetComponentInChildren<Text>();
		dialog=gameObject;
		dialog.SetActive(false);
		_box=box;
		box=null;
	}
	public static void Open(int i,string s)
	{
		chars=0;
		Ship.paused=true;
		dialog.SetActive(true);
		dialogBox.sprite=_box[i];
		text.text="";
		fulltext=s;
		text.fontSize = Screen.height / 30;
	}
	void Update () {
		if(chars<fulltext.Length)
		{
			chars+=Time.deltaTime*15;
			if(text.text.Length<Mathf.FloorToInt(chars))text.text=fulltext.Substring(0,Mathf.FloorToInt(chars));
		}
	}
	public void Close()
	{
		if(chars>=fulltext.Length)
		{
			Ship.paused=false;
			gameObject.SetActive(false);
		}
		else
		{
			chars=fulltext.Length;
			text.text=fulltext.Substring(0,Mathf.FloorToInt(chars));
		}
	}
}
