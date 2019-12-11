using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver : EnemyBase
{
	private Vector3 rotation=Vector3.zero;
	private Transform mouthL;
	private Transform mouthR;
	private Transform legL;
	private Transform legR;
	private Vector3 vector = new Vector3();
	private float time;
	Del movement;
	public override void SetSprites(EnemyInfo ei)
	{
		explosionID=8;
		hp=12;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 50;
		if(legL)return;
		GameObject go = new GameObject("legL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		legL=go.transform;
		go=new GameObject("legR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		legR=go.transform;
		go=new GameObject("mouthL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		mouthL=go.transform;
		go=new GameObject("mouthR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];
		mouthR=go.transform;
		mouthL.parent=mouthR.parent=legL.parent=legR.parent=transform;
		mouthL.localPosition=new Vector3(0.4f,-1f,0.1f);
		mouthR.localPosition=new Vector3(-0.4f,-1f,0.1f);
		legL.localPosition=new Vector3(0.1f,1f,0.1f);
		legR.localPosition=new Vector3(-0.1f,1f,0.1f);
		movement=SlowFall;
		fallSpeed=-2.5f;
		time=Time.time+5;
	}
	public override void Position(int i){
		base.Position(i);
		if(i<=0 || i>=19)time=0;
	}
	public void Fall(float f){
		fallSpeed=f;
	}
	new void Update()
	{
		base.Update();
		if(Ship.paused) return;
		movement();
		mouthL.localEulerAngles=vector;
		mouthR.localEulerAngles=-vector;
		legL.localEulerAngles=-vector*2;
		legR.localEulerAngles=vector*2;
	}
	protected override void SlowFall(){
		base.SlowFall();
		Vector3 v=transform.position-player.position;
		v.z=0;
		v.Normalize();
		transform.Rotate(Vector3.Cross(v,-transform.up)*Time.deltaTime*270f);
		vector.Set(0,0,Mathf.PingPong(Time.time*50,45f));
		if(time<Time.time)movement=Follow;
	}
	void Follow(){
		Vector3 v=transform.position-player.position;
		v.z=0;
		v.Normalize();
		transform.Rotate(Vector3.Cross(v,-transform.up)*Time.deltaTime*60f);
		transform.Translate(0,-12*Time.deltaTime,0);
		vector.Set(0,0,Mathf.PingPong(Time.time*150,45f));
		if(transform.position.y<-Scaler.sizeY || transform.position.x<-Scaler.sizeX || transform.position.x>Scaler.sizeX)Die();
	}
}
