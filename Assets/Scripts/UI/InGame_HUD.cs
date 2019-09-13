using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_HUD : MonoBehaviour 
{
	[SerializeField]
	private Text scoreHUD;

	public static float shipHealth = 1;

	[SerializeField]
	private RectTransform fillMask;
	private RectTransform lifeFill;

	public static float _special;

	[SerializeField]
	private RectTransform specialFill;
	private Image bar;
	private Color color;
	private Color otherColor=Color.white;
	Vector3 helper=Vector3.one;
	
	void Start()
	{
		lifeFill=fillMask.GetChild(0) as RectTransform;
		bar=specialFill.GetComponentInChildren<Image>();
		color=bar.color;
	}
	
	void Update()
	{
		helper.x=shipHealth;
		fillMask.localScale =helper;
		helper.x=1f/shipHealth;
		lifeFill.localScale=helper;
		scoreHUD.text = EnemySpawner.points.ToString();
		if(_special >=1)
		{
			_special = 1;
			bar.color=Color.Lerp(color,otherColor,Mathf.Cos(Time.time*8));
		}else bar.color=color;
		helper.x=_special;
		specialFill.localScale =helper;
	}
}
