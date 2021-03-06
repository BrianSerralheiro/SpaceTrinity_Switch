﻿using UnityEngine;

public class Shooter : EnemyBase
{
	private int position;
	private Vector3 finalpoint;
	private float shoottimer;
	int shots;
	private Transform armL;
	private Transform armR;
	private Transform legL;
	private Transform legR;
	private Core crystal;
	private Vector3 vector = new Vector3();
	private Vector3 rot = new Vector3();
	Del movement;
	private static int shootId,trailID,impactID;
    private int shotCount,cicles;
    private float shotDelay,reloadTime;
	private BulletPath path;
	private static BulletPath[] paths;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 100;
		SetHP(8,ei.lifeproportion);
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
		crystal=go.AddComponent<Core>().Set(ei.sprites[5],Color.black);
		crystal.white=new Color(0.8f,0f,0.8f);
		EnemySpawner.AddPost(go);
		crystal.transform.parent=armL.parent=armR.parent=legL.parent=legR.parent=transform;
		armL.localPosition=new Vector3(0.4f,-1f,0.1f);
		armR.localPosition=new Vector3(-0.4f,-1f,0.1f);
		legL.localPosition=new Vector3(0,1.2f,0.1f);
		legR.localPosition=new Vector3(0,1.2f,0.1f);
		crystal.transform.localPosition=new Vector3(0,-1.27f);
		fallSpeed=-9;
		movement=Moving;
		shootId=ei.bulletsID[0];
		trailID=ei.particleID[0];
		impactID=ei.particleID[1];
		MultiPathEnemy pe=(MultiPathEnemy)ei;
		shotCount=pe.shotCount;
		cicles=pe.cicles;
		shotDelay=pe.shotDelay;
		reloadTime=pe.reloadTime;
		paths=pe.paths;
	}

	public override void Position(int i)
	{
		base.Position(i);
		position=i;
		finalpoint=new Vector3(i*Scaler.sizeX/20-Scaler.sizeX/2+Scaler.sizeX/40,Scaler.sizeY/2,0);
		path=paths[i<10?i:(19-i)];
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		movement();
		armL.localEulerAngles=vector;
		armR.localEulerAngles=-vector;
		legL.localEulerAngles=-vector;
		legR.localEulerAngles=vector;
	}
	void Moving(){
		transform.position=Vector3.MoveTowards(transform.position,finalpoint,4*Time.deltaTime);
		transform.up=transform.position-finalpoint;
		if(finalpoint==transform.position)
		{
			movement=Shooting;
			//transform.up=-path.GetNode0().normalized;
		}
		vector.Set(0,0,Mathf.PingPong(Time.time*80,45f));
	}
	void Shooting(){
		Vector3 v=path.GetNode0(transform.position.x>0);
		// if(transform.position.x>0)v.x*=-1;
		transform.Rotate(Vector3.Cross(v,transform.up)*Time.deltaTime*30f);
		shoottimer-=Time.deltaTime;
		if(shoottimer<-shotDelay) 
		{
			if(shots-->0){
				shoottimer=0;
				Shoot();
			}
			else if(cicles-->0){
				shoottimer=reloadTime;
				shots=shotCount;
			}
			else
			movement=SlowFall;
		}
		crystal.Min(Time.deltaTime*2);
		vector.Set(0,0,Mathf.PingPong(Time.time*67.5f,45f));
	}
	protected override void SlowFall(){
		base.SlowFall();
		transform.Rotate(Vector3.Cross(Vector3.down,transform.up)*Time.deltaTime*190f);
		crystal.Min(Time.deltaTime);
		vector.Set(0,0,Mathf.PingPong(Time.time*200,45f));
	}
	void Shoot()
	{
		// SoundManager.PlayEffects(12, 0.5f, 0.8f);
		crystal.Set(2);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		PathBullet bu=go.AddComponent<PathBullet>();
		bu.owner=transform.name;
		bu.spriteID=shootId;
		bu.path=path;
		bu.particleID=trailID;
		bu.impactID=impactID;
		bu.mirror=transform.position.x>0;
		go.transform.position=crystal.transform.position;
		go.transform.up=-transform.up;
	}
}
