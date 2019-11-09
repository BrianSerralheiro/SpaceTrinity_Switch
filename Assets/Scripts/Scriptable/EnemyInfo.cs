﻿using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Enemy Info")]
public class EnemyInfo : ScriptableObject
{
	public Sprite[] sprites;
	public Sprite[] bullets;
	System.Type script;
	[HideInInspector]
	public int[] bulletsID;
	//public EnemyInfo copy;
	public System.Type GetScript()
	{
		if(script==null)script=System.Type.GetType(name);
		return script;
	}
	public void Register(){
		if(bullets==null || bullets.Length==0)return;
		bulletsID=new int[bullets.Length];
		for(int i=0;i<bullets.Length;i++){
			bulletsID[i]=Bullet.Register(bullets[i]);
		}
	}
	/* void OnValidate()
	{
		if(copy!=null)copy.sprites=sprites;
	} */
}
