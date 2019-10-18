using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {
	private float chars;
	[SerializeField]
	private Sprite[] box;
	private Image portrait;
	[SerializeField]
	private float cps;
	private static string[] texts;
	private static Sprite[] charPics;
	private static float[] sizes;
	private static int id;
	private static UnityAction on;
	private Text text;
	private static Image image;
	void Start () {
		text=GetComponentInChildren<Text>();
		portrait=GetComponentsInChildren<Image>()[0];
		image=GetComponentsInChildren<Image>()[1];
		if(!text || !image)Debug.LogError("Dialog box needs both image and text components in children");
		// DESCOMENTAR image.sprite=box[Ship.playerID];
		box=null;
		on=On;
		gameObject.SetActive(false);
	}
	public static void Chars(Sprite[] s)
	{
		charPics=s;
	}
	public static void Texts(string[] s,float[] f)
	{
		texts=s;
		sizes=f;
	}
	public static void Text(int i)
	{
		id=i;
		on();
	}
	private void On()
	{
		Ship.paused=true;
		gameObject.SetActive(true);
		text.text="";
		portrait.sprite=charPics[id];
		chars=0;
		text.fontSize = Mathf.CeilToInt(Screen.height/10/sizes[id]);
	}
	public void Close()
	{
		if(chars>=texts[id].Length)
		{
			Ship.paused=false;
			gameObject.SetActive(false);
		}
		else
		{
			chars=texts[id].Length;
			text.text=texts[id].Substring(0,Mathf.FloorToInt(chars));
		}
	}
	void Update () {
		if(chars<texts[id].Length)
		{
			chars+=Time.deltaTime * cps;
			if(text.text.Length<Mathf.FloorToInt(chars))text.text=texts[id].Substring(0,Mathf.FloorToInt(chars));
		}

	}

	public static string GetText(int i)
	{
		if(texts != null)
		{
			return texts[i];
		}
		return"";
	}

	public static float getSize(int i)
	{
		if(sizes==null)return 2;
		return sizes[i];
	}
	public static Sprite getBox()
	{
		if(image != null)
		{
			return image.sprite;
		}
		return null;
	}
}
