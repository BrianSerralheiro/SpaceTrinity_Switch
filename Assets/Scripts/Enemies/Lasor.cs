using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasor : EnemyBase
{
	private float timer=3;
	private Vector3 dir=Vector3.right;
	private Transform laser;
	private SpriteRenderer charge;
	private new BoxCollider2D collider;
	private Core core;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=40;
		points = 120;
		GameObject go = new GameObject("charge");
		go.transform.localScale=new Vector3();
		charge=go.AddComponent<SpriteRenderer>();
		charge.sprite=ei.sprites[1];
		go.transform.parent=transform;
		go.transform.localScale=new Vector3();
		go = new GameObject("laserbase");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		go.transform.parent=transform;
		go.transform.localScale=new Vector3();
		laser=go.transform;
		laser.localPosition=charge.transform.localPosition=new Vector3();
		go = new GameObject("laserbody");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		collider=go.AddComponent<BoxCollider2D>();
		collider.enabled=false;
		go.transform.parent=laser;
		go.transform.localPosition=new Vector3(0,-1.1f,0);
		go.transform.localScale=Vector3.right+Vector3.up*38;
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[4],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,1.86f);
	}

	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="laserbody") return;
		base.OnCollisionEnter2D(col);
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*2,0);
		else 
		timer-=Time.deltaTime;
		if(timer<=0){
			timer=3;
			dir.Set(Random.value>0.5f?1f:-1f,0,0);
		}
		if(timer<1)
		{
			charge.transform.localScale=laser.localPosition;
			dir.Set(1-timer,1,1);
			core.Set(timer);
			laser.localScale=dir;
			collider.enabled=!collider.enabled;
		}else if(timer<2)
		{
			laser.localScale=laser.localPosition;
			Color c=charge.color;
			c.a=2f-timer;
			charge.color=c;
			dir.Set(1,(timer-1)*5,1);
			charge.transform.localScale=dir;
			core.Set(2f-timer);
		}
		else
		{
			dir.z=dir.y=0;
			laser.localScale=charge.transform.localScale=laser.localPosition;
			transform.Translate(dir*Time.deltaTime*3.5f);
			if(transform.position.x<-Scaler.sizeX/2f+1)dir.x=1;
			if(transform.position.x>Scaler.sizeX/2f-1)dir.x=-1;
		}
	}
}
