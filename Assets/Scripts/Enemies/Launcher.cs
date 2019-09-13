using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : EnemyBase {
	private float timer=5;
	private float spd;
	private Transform rocket;
	private Transform burst;
	private Vector3 rot;
	private Vector3 scale=Vector3.one;
	private Vector3 pos=new Vector3(0,0.4f,0.1f);
	private Core core;
	new void Start () {
		base.Start();
		points = 100;
		hp=40;
		Create();
		timer=5;
		GameObject go=new GameObject("core");
		core=go.AddComponent<Core>().Set(SpriteBase.I.launcher[3],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,0.11f);
	}
	private void Create()
	{
		timer=3;
		spd=0;
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.launcher[1];
		go.AddComponent<Missile>().SetHP(30);
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		rocket=go.transform;
		rocket.position=transform.position;
		go = new GameObject("burst");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.launcher[2];
		burst=go.transform;
		burst.parent=rocket;
		burst.localPosition=new Vector3(0,-0.5f);
		scale.y=0;
		burst.localScale=scale;
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		if(transform.position.y>-Scaler.sizeY-1)transform.Translate(0,-Time.deltaTime/2,0);
		else Die();
		if(timer>0 && rocket)
		{
			rocket.position=transform.position+pos;
			scale.y=0;
			core.Min(Time.deltaTime*2);
		}
		if(timer <0)
		{
			core.Add(Time.deltaTime*2);
			if(rocket)
			{
				spd+=Time.deltaTime/2;
				if(spd>1)spd=1;
				scale.y=spd*5;
				burst.localScale=scale;
				Vector3 v=rocket.position-player.position;
				v.z=0;
				v.Normalize();
				rocket.Rotate(Vector3.Cross(v,rocket.up)*Time.deltaTime*270f*spd);
				rocket.Translate(0,Time.deltaTime*8*spd,0);
			}
			else Create();
		}

	}
	protected override void Die()
	{
		base.Die();
		if(rocket)Destroy(rocket.gameObject);
	}
}
