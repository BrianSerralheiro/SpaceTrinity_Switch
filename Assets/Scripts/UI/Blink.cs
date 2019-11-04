using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
	private Color color=Color.white;
	private Text text;
	void Start()
	{
		text=GetComponent<Text>();
	}
	void Update()
	{
		if(InGame_HUD.special>=1)color.a=Mathf.Sin(Time.time*5);
		else color.a=0;
		text.color=color;
	}
}
