using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatGirl : EnemyBase {

	private Transform wingL;
	private Transform wingR;
	private SpriteRenderer render;
	private Sprite left;
	private Sprite closed;
	private Vector3 vector = new Vector3();
	private Vector3 pos = new Vector3(0.7f,1.32f,0.1f);
	private Vector3[] poses={new Vector3(-1,-1,0).normalized,new Vector3(0,-1,0).normalized,new Vector3(1,-1,0).normalized };
	private float timer=3;
	new void Start () {
		base.Start();
		explosionID = 9;
		hp=180;
		points=150;
		GameObject go = new GameObject("wingL");
		render=go.AddComponent<SpriteRenderer>();
		left=SpriteBase.I.batgirl[2];
		closed=SpriteBase.I.batgirl[4];
		render.sprite=left;
		wingL=go.transform;
		go = new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.batgirl[3];
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(0.7f,1.32f,0.1f);
		wingR.localPosition=new Vector3(-0.7f,1.32f,0.1f);
		go = new GameObject("head");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.batgirl[1];
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0f,1.8f,-0.1f);
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		vector.Set(0,0,Mathf.PingPong(Time.time*300,-45f)+60f);
		if(transform.position.y>0)transform.Translate(0,-Time.deltaTime*2,0);
		if(timer<2)
		{
			render.sprite=closed;
			wingR.gameObject.SetActive(false);
			vector.Set(0,0,0);
			pos.z=-0.1f;
		}
		if(timer<0)
		{
			render.sprite=left;
			wingR.gameObject.SetActive(true);
			pos.z=0.1f;
			for(int i = 0; i<3; i++)
			{
				Bat(i);
			}
			timer=5;
		}
		wingL.localPosition=pos;
		wingL.localEulerAngles=vector;
		wingR.localEulerAngles=-vector;
	}
	void Bat(int i)
	{
		GameObject go=new GameObject("enemy");
		go.AddComponent<Bat>().target=transform.position+poses[i]*3f;
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bat[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position;
	}
	protected override void Die()
	{
		base.Die();
		for(int i = 0; i<10; i++)
		{
			ParticleManager.Emit(9,(Vector3)Random.insideUnitCircle*1.5f+transform.position,1);
		}
	}
}
