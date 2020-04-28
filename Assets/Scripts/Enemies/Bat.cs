using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyBase 
{
	private Transform wingL;
	private Transform wingR;
	private Vector3 vector=new Vector3();
	public Vector3 target=new Vector3();
	int count=3;

	public override void SetSprites(EnemyInfo ei)
	{
		hp=5;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points=20;
		GameObject go=new GameObject("wingL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		wingL=go.transform;
		go = new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(0.2f,-0.2f,0.1f);
		wingR.localPosition=new Vector3(-0.2f,-0.2f,0.1f);
	}
	public override void Position(int i)
	{
		base.Position(i);
		target.Set(transform.position.x,transform.position.y-5,0);
	}
	new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
		vector.Set(0,0,Mathf.PingPong(Time.time*300,60f)-60f);
		wingL.eulerAngles=vector;
		wingR.eulerAngles=-vector;
		Vector3 pos=transform.position;
		pos=Vector3.MoveTowards(pos,target,Time.deltaTime*3);
		if(target==pos){
			if(count--==0)
			target=new Vector3(pos.x,Scaler.sizeY+4,pos.z);
			else
			target=pos+(GetPlayer().position-pos).normalized*8;
		}
		if(transform.position.y>Scaler.sizeY+3)Die();
		transform.position=pos;
	}
}
