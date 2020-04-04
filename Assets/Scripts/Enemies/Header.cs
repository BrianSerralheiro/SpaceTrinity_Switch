﻿using UnityEngine;

public class Header : EnemyBase {
	private int  prev=1,shots=5;
	private Vector3 position;
	private float timer;
	private Core eyes;
	private Core core;
	Del movement;
	private static int shootId;
	static Vector3[] dir={Vector3.left,Vector3.down,Vector3.right};
	public override void SetSprites(EnemyInfo ei)
	{
		hp=70;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points=180;
		GameObject go=new GameObject("eyes");
		eyes=go.AddComponent<Core>().Set(ei.sprites[1],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,-0.28f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[2],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,0.42f);
		movement=Pathing;
		shootId=ei.bulletsID[0];
		fallSpeed=-4;
	}
	public override void Position(int i){
		base.Position(i);
		position=transform.position+dir[1]*3;
	}
	void Pathing(){
		eyes.Set(Mathf.PingPong(Time.time/2,1));
		transform.position=Vector3.MoveTowards(transform.position,position,Time.deltaTime*4);
		if(transform.position==position){
			int i=0;
			Shoot();
			while (true)
			{
				i=Random.Range(0,3);
				if(position.x<-Scaler.sizeX/2+4 &&  i==0)continue;
				if(position.x>Scaler.sizeX/2-4 &&  i==2)continue;
				if(i!=prev){
					prev=i;
					position+=dir[i]*4;
					break;
				}
			}
			if(shots<=0)movement=Flee;
		}
	}
	void Flee(){
		transform.Translate((transform.position.x>0?Time.deltaTime:-Time.deltaTime)*10,0,0);
		if(Mathf.Abs(transform.position.x)>Scaler.sizeX/2+2)Die();
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		movement();
		core.Min(Time.deltaTime);
	}
	void Shoot()
	{
		core.Set(1);
		shots--;
		// SoundManager.PlayEffects(12, 0.5f, 0.8f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		Bullet bu=go.AddComponent<Bullet>();
		bu.owner=name;
		bu.spriteID=shootId;
		go.transform.position=transform.position;
		Vector3 v=GetPlayer(transform.position).position-transform.position;
		v.z=0;
		go.transform.up=v.normalized;
	}

}
