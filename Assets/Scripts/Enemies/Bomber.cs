using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : EnemyBase {
	private float timer=4;
	private Vector3 local=new Vector3(-3.5f,-0.2f);
	private Sprite bomb;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		name+="big";
		explosionID = 10;
		hp=200;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		bomb=ei.sprites[1];
	}

	public override void Position(int i)
	{
		base.Position(i);
		transform.Rotate(0,0,-90f);

	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		transform.Translate(Time.deltaTime,0,0);
		timer-=Time.deltaTime;
		if(timer<0)Bomb();
		if(transform.position.x<-Scaler.sizeX/2f-4.2F || transform.position.x>Scaler.sizeX/2f+4.2F || transform.position.y<-Scaler.sizeY-4.2F) Die();

	}
	void Bomb()
	{
		timer=2;
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=bomb;
		go.transform.parent=transform;
		go.transform.localPosition=local;
		go.transform.parent=null;
		go.AddComponent<Bomb>().SetSprites(null);
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