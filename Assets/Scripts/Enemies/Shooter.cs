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
	protected new void Start()
	{
		base.Start();
		points = 100;
		explosionID=8;
		_renderer.flipY=true;
		lifetimer=5;
		if(legL) return;
		GameObject go = new GameObject("legL");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.shooter[1];
		legL=go.transform;
		go=new GameObject("legR");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.shooter[2];
		legR=go.transform;
		go=new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.shooter[3];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.shooter[4];
		armR=go.transform;
		go=new GameObject("crystal");
		crystal=go.AddComponent<Core>().Set(SpriteBase.I.shooter[5],new Color(0.4f,0f,0.4f));
		crystal.transform.parent=armL.parent=armR.parent=legL.parent=legR.parent=transform;
		armL.localPosition=new Vector3(-0.4f,1.2f,0.1f);
		armR.localPosition=new Vector3(0.4f,1.2f,0.1f);
		legL.localPosition=new Vector3(0,-1f,0.1f);
		legR.localPosition=new Vector3(0,-1f,0.1f);
		crystal.transform.localPosition=new Vector3(0,1.2f);
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
			transform.up=finalpoint-transform.position;
			if((finalpoint-transform.position).sqrMagnitude<0.005f)
			{
				transform.position=finalpoint;
				position=-1;
			}
		}
		else
		{
			rot.Set(0,0,Mathf.Atan2(transform.position.x-player.position.x,player.position.y-transform.position.y)*Mathf.Rad2Deg);
			transform.eulerAngles=rot;
			if(shoottimer>0) shoottimer-=Time.deltaTime;
			else
			{
				shoottimer=1.5f;
				Shoot();
			}
			crystal.Set(Mathf.Lerp(0,1,shoottimer-0.5f));
		}
		vector.Set(0,0,180+Mathf.PingPong(Time.time*100,35f));
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
		go.transform.rotation=transform.rotation;
	}
}
