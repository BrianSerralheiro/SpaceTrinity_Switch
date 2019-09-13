using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : EnemyBase
{

	private float shoottimer = 2;
	private Vector3 vector=new Vector3();
	protected new void Start()
	{
		base.Start();
		hp=52;
		points = 150;
	}

	public override void Position(int i)
	{
		base.Position(i%8);
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		shoottimer-=Time.deltaTime;
		transform.Translate(0,-Time.deltaTime * 2,0,Space.World);
		if(transform.position.y<-Scaler.sizeY-1)Destroy(gameObject);
		vector.z=shoottimer/0.2f*90f;
		transform.eulerAngles=vector;
		if(shoottimer<=0)
		{
			SoundManager.PlayEffects(12, 0.1f, 0.5f);
			shoottimer=0.2f;
			for(int i=0;i<4;i++){
				Shoot(i);
			}
		}
	}
	void Shoot(int i)
	{
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[10];
		go.AddComponent<BoxCollider2D>();
		Bullet bu = go.AddComponent<Bullet>();
		bu.owner=transform.name;
		bu.spriteID=10;
		Vector3 v= new Vector3(i%2,i/2,0)-Vector3.one*0.5f;
		go.transform.position=transform.position+v;
		go.transform.eulerAngles=new Vector3(0,0,135+i*90)*(i<2?1:-1);
	}

}
