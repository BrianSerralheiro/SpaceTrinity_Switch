using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : EnemyBase
{
	private int position;
	private Vector3 finalpoint;
	private float shoottimer=1;
	private float lifetimer;

	private Transform armL;
	private Transform armR;
	private Transform legL;
	private Transform legR;
	private Core crystal;
	private Vector3 vector = new Vector3();
	private Vector3 rot = new Vector3();
	public override void SetSprites(EnemyInfo ei)
	{
		points = 100;
		explosionID=8;
		lifetimer=5;
		GameObject go = new GameObject("legL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		legL=go.transform;
		go=new GameObject("legR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		legR=go.transform;
		go=new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];
		armR=go.transform;
		go=new GameObject("crystal");
		crystal=go.AddComponent<Core>().Set(ei.sprites[5],new Color(0.4f,0f,0.4f));
		crystal.transform.parent=armL.parent=armR.parent=legL.parent=legR.parent=transform;
		armL.localPosition=new Vector3(0.4f,-1f,0.1f);
		armR.localPosition=new Vector3(-0.4f,-1f,0.1f);
		legL.localPosition=new Vector3(0,1.2f,0.1f);
		legR.localPosition=new Vector3(0,1.2f,0.1f);
		crystal.transform.localPosition=new Vector3(0,-1.27f);
	}

	public override void Position(int i)
	{
		base.Position(i);
		position=i;
		finalpoint=new Vector3((i*Scaler.sizeX/20-Scaler.sizeX/2)*0.9f,Scaler.sizeY/2,0);
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		if(position>=0)
		{
			transform.Translate((finalpoint-transform.position).normalized*4*Time.deltaTime,Space.World);
			transform.up=transform.position-finalpoint;
			if((finalpoint-transform.position).sqrMagnitude<0.005f)
			{
				transform.position=finalpoint;
				position=-1;
			}
		}
		else
		{
			Vector3 v=transform.position-player.position;
			v.z=0;
			v.Normalize();
			transform.Rotate(Vector3.Cross(v,-transform.up)*Time.deltaTime*90f);
			if(shoottimer>0) shoottimer-=Time.deltaTime;
			else
			{
				shoottimer=1.5f;
				Shoot();
			}
			crystal.Set(Mathf.Lerp(0,1,shoottimer-0.5f));
		}
		vector.Set(0,0,Mathf.PingPong(Time.time*100,45f));
		armL.localEulerAngles=vector;
		armR.localEulerAngles=-vector;
		legL.localEulerAngles=-vector;
		legR.localEulerAngles=vector;
	}
	void Shoot()
	{
		SoundManager.PlayEffects(12, 0.5f, 0.8f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[8];
		go.AddComponent<BoxCollider2D>();
		Bullet bu=go.AddComponent<Bullet>();
		bu.owner=transform.name;
		bu.spriteID=8;
		go.transform.position=crystal.transform.position;
		go.transform.up=-transform.up;
	}
}
