﻿using UnityEngine;
using UnityEngine.UI;

public class InGame_HUD : MonoBehaviour 
{
	[SerializeField]
	private Text scoreHUD,pilotName;
	private static float _currentHP=1;
	public static float shipHealth=1,special;

	[SerializeField]
	private Image lifeBar,lifeFill,specialFill,pilotPic, pilotMask;

	private Color color;
	private Color otherColor=Color.white;
	Vector3 helper=Vector3.one;
	public static HUDInfo HUD;
	void Start()
	{
		lifeBar.sprite=HUD.lifeBar;
		lifeFill.sprite=HUD.lifeFill;
		pilotName.text=HUD.name;
		pilotPic.sprite=HUD.picture;
		specialFill.color=pilotMask.color=HUD.color;
		color=specialFill.color;
		lifeFill.material.SetFloat("_CurrentHP",_currentHP);
	}
	
	void Update()
	{
		if(shipHealth<_currentHP){
			_currentHP-=Time.deltaTime;
			if(shipHealth>_currentHP)_currentHP=shipHealth;
			lifeFill.material.SetFloat("_CurrentHP",_currentHP);
		}
		if(shipHealth>_currentHP){
			_currentHP+=Time.deltaTime;
			if(shipHealth<_currentHP)_currentHP=shipHealth;
			lifeFill.material.SetFloat("_CurrentHP",_currentHP);
		}
		scoreHUD.text = EnemySpawner.points.ToString();
		if(special >=1)
		{
			special = 1;
			specialFill.color=Color.Lerp(color,otherColor,Mathf.Cos(Time.time*8));
		}else specialFill.color=color;
		specialFill.fillAmount=special;
	}
}
