using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyBase 
{
	private Transform wingL;
	private Transform wingR;
	private Vector3 vector=new Vector3();
	public Vector3 target=new Vector3();


	public override void SetSprites(EnemyInfo ei)
	{
		explosionID = 9;
		hp=10;
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
		if(i<8)
		{
			target.Set(transform.position.x,transform.position.y-5,0);
		}
		else
		{
			target.Set(transform.position.x+(i==8 ? 5: -5),transform.position.y,0);
		}
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
		if((target-pos).sqrMagnitude<0.2f)target=pos+(player.position-pos).normalized*8;
		transform.position=pos;
	}
}
