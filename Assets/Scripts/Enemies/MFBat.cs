using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFBat : EnemyBase {
	private float timer=5;
	private float sprite;
	private Vector2 dir;
	new void Start () {
		base.Start();
		dir=new Vector3(Random.Range(-1f,1f),-0.5f);
		explosionID = 10;
		hp=50;
		points=100;
	}
	
	new void Update ()
	{
		if(Ship.paused) return;
		base.Update();
		dir+=Random.insideUnitCircle/10f;
		if(timer>0)timer-=Time.deltaTime;
		else Shoot();
		if(sprite>0)sprite-=Time.deltaTime;
		_renderer.flipX=transform.position.x<player.position.x;
		if(transform.position.x>Scaler.sizeX/2-1)dir.x=-Mathf.Abs(dir.x);
		if(transform.position.x<-Scaler.sizeX/2+1)dir.x=Mathf.Abs(dir.x);
		if(transform.position.y>Scaler.sizeY-2)dir.y=-Mathf.Abs(dir.y);
		if(transform.position.y<-Scaler.sizeY-2)Die();
		transform.Translate(dir.x*Time.deltaTime,dir.y*Time.deltaTime,0);
		if(sprite<=0)_renderer.sprite=SpriteBase.I.MFBat[Mathf.RoundToInt(Mathf.PingPong(Time.time*3,1f))];
		
	}
	void Shoot()
	{
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[18];
		go.AddComponent<BoxCollider2D>();
		go.AddComponent<Slash>().spriteID=18;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position;
		go.transform.localScale=Vector3.right*3+Vector3.up*2;
		sprite=0.3f;
		_renderer.sprite=SpriteBase.I.MFBat[2];
		timer=1.2f;
		//go.transform.localScale=Vector3.one*2;
	}
}
