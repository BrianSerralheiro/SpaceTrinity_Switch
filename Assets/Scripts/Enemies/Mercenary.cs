using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : EnemyBase {
	private float timer;
	[SerializeField]
	private bool low;
	private bool defeat;
	private Vector3[] dir={(Vector3.left/5+Vector3.down).normalized,Vector3.down,(Vector3.down+Vector3.right/5).normalized};
	public override void SetSprites(EnemyInfo ei)
	{
		hp=500;
		BossDialog.Open(1,"Oh! Let's see if you have what it takes to protect our planet! ò.ó");
		EnemySpawner.boss=true;
	}
	
	new void Update ()
	{
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*2,0);
		if(Ship.paused){
			if(!player.gameObject.activeSelf)transform.Translate(0,-Time.deltaTime*10,0);
			if(transform.position.y<-Scaler.sizeY-2){Destroy(gameObject);
			EnemySpawner.boss=false;}
			return;
		}
		if(defeat)
		{
			transform.Translate(0,-Time.deltaTime*4,0);
			if(transform.position.y<-Scaler.sizeY-2)Destroy(gameObject);
			return;
		}
		base.Update();
		if(player.position.x<transform.position.x) transform.Translate(-Time.deltaTime,0,0);
		else transform.Translate(Time.deltaTime,0,0);
		if(timer>0)timer-=Time.deltaTime;
		else
		{
			Shoot();
		}
	}
	protected override void Die()
	{
		Locks.Char(2,true);
		EnemySpawner.boss=false;
		BossDialog.Open(1,"WOW! I am impressed, i will help you protect earth... but it will cost you! owo");
		defeat=true;
		GetComponent<BoxCollider2D>().enabled=false;
	}
	public new void OnCollisionEnter2D(Collision2D col)
	{
		if(transform.position.y<Scaler.sizeY-1)base.OnCollisionEnter2D(col);
		low=hp<300;
	}

	void Shoot()
	{
		timer=low?1f:2.5f;
		int j=low?3:1;
		for(int i=0;i<j;i++){
			GameObject go = new GameObject("enemy");
			SpriteRenderer s=go.AddComponent<SpriteRenderer>();
			s.sprite=SpriteBase.I.bullets[6];
			s.flipY=true;
			go.AddComponent<BoxCollider2D>();
			go.AddComponent<Slash>().spriteID=6;
			Rigidbody2D r = go.AddComponent<Rigidbody2D>();
			r.isKinematic=true;
			r.useFullKinematicContacts=true;
			go.transform.position=transform.position;
			if(low)go.transform.up=-dir[i];
		}
	}
}
