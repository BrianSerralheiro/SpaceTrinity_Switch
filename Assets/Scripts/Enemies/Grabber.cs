﻿using UnityEngine;
public class Grabber : EnemyBase {
	private Transform armL, armR;
	private Ship grabed;
	private float timer;
	private Core core;
	float vector;
	BulletPath path;
	static BulletPath[] paths;
	Vector3 position;
	private int shotId;
	public override void SetSprites(EnemyInfo ei)
	{
		SetHP(10,ei.lifeproportion);
		points = 50;
		GameObject go = new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		armR=go.transform;
		armL.parent=armR.parent=transform;
		armL.localPosition=new Vector3(0.6f,-0.6f,-0.1f);
		armR.localPosition=new Vector3(-0.6f,-0.6f,-0.1f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[3],Color.black);
		EnemySpawner.AddPost(go);
		core.white=new Color(0.5f,0.1f,0.05f);
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,-0.18f,-0.1f);
		if(paths==null)paths=(ei as MultiPathEnemy).paths;
		shotId=ei.bulletsID[0];
	}
	public override void Position(int i){
		base.Position(i);
		int p=i<10?i:(19-i);
		if(i==0)transform.position=new Vector3(-Scaler.sizeX/2-2,Scaler.sizeY/2,0.1f);
		if(i==19)transform.position=new Vector3(Scaler.sizeX/2+2,Scaler.sizeY/2,0.1f);
		position=transform.position;
		if(p<3)path=paths[p];
		else path=paths[3];
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		if(path.Finished()){
			Die();
			Shoot();
		}
		else {
			transform.Rotate(Vector3.Cross(-transform.up,path.Direction(position.x>0))*180*Time.deltaTime);
			if(!stopMovement)transform.position=position+BulletPath.Next(ref path,position.x>0);
		}
		if(timer>0 && grabed){
			timer-=Time.deltaTime;
			grabed.transform.position=transform.position-transform.up*.8f;
			if(timer<=0 || grabed.reviveTimer>0){
				grabed.transform.parent=null;
				grabed.transform.rotation=Quaternion.identity;
				grabed=null;
			}
			core.Add(Time.deltaTime);
		}
		else {
			core.Min(Time.deltaTime);
			vector=Mathf.PingPong(Time.time*100,45f);
		}
		armL.localRotation=Quaternion.Euler(0,0,vector);
		armR.localRotation=Quaternion.Euler(0,0,-vector);
	}
	void Shoot()
	{
		GameObject go = new GameObject("enemybullet");
		SpriteRenderer r = go.AddComponent<SpriteRenderer>();
		r.sprite=Bullet.sprites[shotId];
		r.material = Round.shotMaterial;
		go.AddComponent<CircleCollider2D>();
		Bullet bu = go.AddComponent<Bullet>();
		r.color = new Color(0.5f,0.5f,0.5f);
		bu.owner=transform.name;
		bu.spriteID=shotId;
		bu.Timer(3);
		go.transform.position=transform.position;
		go.transform.up=GetPlayer().position-go.transform.position;
		bu.particleID=Round.trailID;
		bu.impactID=Round.impactID;
	}
	void OnDisable()
	{
		if(grabed){
			grabed.transform.parent=null;
			grabed.transform.rotation=Quaternion.identity;
			grabed=null;
		}
	}
	protected override void Die()
	{
		if(grabed){
			grabed.transform.parent=null;
			grabed.transform.rotation=Quaternion.identity;
			grabed=null;
		}
		base.Die();
	}
	new private void OnCollisionEnter2D(Collision2D col)
	{
		Ship s=col.collider.GetComponent<Ship>();
		if(s && s.immuneTime<=0 && s.transform.parent==null && timer<=0){
			grabed=s;
			grabed.transform.parent=transform;
			timer=3;
			vector=-30f;
		}else 
			base.OnCollisionEnter2D(col);
	}
}
