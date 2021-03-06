﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_HUD : MonoBehaviour 
{
	[SerializeField]
	private bool p1;
	bool hide;
	private int id;
	private float currentHp,_shipHealth,_special;
	[SerializeField]
	private Text scoreHUD,pilotName,steal,pointsToRevive,level;
	private static int[] levels={1,1};
	private static float[] _currentHP={1,1};
	public static float[] shipHealth={1,1},special={0,0};

	[SerializeField]
	private Image lifeBar,lifeFill,specialFill,pilotPic, pilotMask;
	[SerializeField]
	private Image[] lifes=new Image[4];

	private Color color;
	private Color otherColor=Color.white;
	public static HUDInfo[] HUD=new HUDInfo[2];
	private static HashSet<Ship> shipsToRevive=new HashSet<Ship>();
	private static int levelChanged=-1;
	void Start()
	{
		if(!p1 && !PlayerInput.Conected(1))gameObject.SetActive(false);
		id=p1?0:1;
		lifeBar.sprite=HUD[id].lifeBar;
		lifeFill.sprite=HUD[id].lifeFill;
		pilotName.text=HUD[id].name;
		pilotPic.sprite=HUD[id].picture;
		specialFill.color=pilotMask.color=HUD[id].color;
		color=specialFill.color;
		lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
		for (int i = 0; i < lifes.Length; i++)
		{
			lifes[i].sprite=HUD[id].lifeIcon;
			lifes[i].gameObject.SetActive(i<Ship.continues[id]);
		}
	}
	public static void Revive(Ship s){
		s.reviveTimer=1;
		shipsToRevive.Add(s);
	}
	public static void Level(int i,int l){
		levels[i]=l;
		levelChanged=i;
	}
	void Update()
	{
		if(Ship.continues[id]<0){
			pointsToRevive.color=steal.color=Color.Lerp(Color.white,Color.clear,Mathf.Cos(Time.unscaledTime*8));
			int i=p1?1:0;
			pointsToRevive.text=Ship.revivePoints-(Ship.pointsToRevive-EnemySpawner.points[i])+"/"+Ship.revivePoints;
			if(!hide){
				hide=true;
				lifeBar.enabled=lifeFill.enabled=specialFill.enabled=pilotMask.enabled=pilotPic.enabled=pilotName.enabled=
				level.enabled=scoreHUD.enabled=false;
				pointsToRevive.enabled=steal.enabled=true;
			}
			if(PlayerInput.GetKeyShot(id) && PlayerInput.GetKeySpecialDown(id)){
				if(Ship.continues[i]>0){
					Ship.continues[i]--;
					Ship.continues[id]=0;
					shipHealth[id]=1;
					_currentHP[i]-=Time.unscaledDeltaTime;
					Revive(EnemyBase.players[id].GetComponent<Ship>());
				}
			}
		}
		else if(hide){
				hide=false;
				lifeBar.enabled=lifeFill.enabled=specialFill.enabled=pilotMask.enabled=pilotPic.enabled=pilotName.enabled=
				level.enabled=scoreHUD.enabled=true;
				pointsToRevive.enabled=steal.enabled=false;
			}
		if(shipHealth[id]<_currentHP[id]){
			_currentHP[id]-=Time.unscaledDeltaTime;
			if(shipHealth[id]>_currentHP[id])_currentHP[id]=shipHealth[id];
			lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
		}
		if(shipHealth[id]>_currentHP[id]){
			_currentHP[id]+=Time.unscaledDeltaTime;
			for (int i = 0; i < lifes.Length; i++)
				lifes[i].gameObject.SetActive(i<Ship.continues[id]);
			if(shipHealth[id]<_currentHP[id])_currentHP[id]=shipHealth[id];
			lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
		}
		scoreHUD.text = EnemySpawner.points[id].ToString();
		if(special[id] >=1)
		{
			special[id] = 1;
			specialFill.color=Color.Lerp(color,otherColor,Mathf.Cos(Time.unscaledTime*8));
		}else specialFill.color=color;
		specialFill.fillAmount=special[id];
		if(levelChanged==id){
			level.text="Lv. "+levels[levelChanged];
			levelChanged=-1;
		}
		if(p1)foreach (Ship s in shipsToRevive)
		{
			s.reviveTimer-=Time.unscaledDeltaTime;
			if(s.reviveTimer<=0){
				s.Revive();
				shipsToRevive.Remove(s);
				return;
			}
		}
	}
}
