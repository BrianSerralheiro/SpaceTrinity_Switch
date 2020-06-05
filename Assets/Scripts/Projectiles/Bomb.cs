using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyBase {
	private Vector3 pos;
	private Vector3 aim;
	private float speed=2;
	int explosionID;
	public void Set(int h,int id)
	{
		hp=h;
		explosionID=id;
		pos=transform.position;
		aim=(GetPlayer(pos).position-pos).normalized*20+pos;
	}
	
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		pos=Vector3.MoveTowards(pos,aim,Time.deltaTime*speed);
		speed=Mathf.MoveTowards(speed,8,Time.deltaTime+2);
		transform.position=pos;
		transform.Rotate(0,0,Time.deltaTime*90);
		if((pos-GetPlayer(transform.position).position).sqrMagnitude<10)Explode();
		if((pos-aim).sqrMagnitude<0.1f)Die();
	}
	public new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.name.Contains("Ship")) Explode();
		base.OnCollisionEnter2D(col);
	}
	protected override void Die()
	{
		Destroy(gameObject);
		ParticleManager.Emit(1,transform.position,1);
	}
	private void Explode()
	{
		Destroy(gameObject);
		GameObject go=new GameObject("enemy");
		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
		go.AddComponent<BoxCollider2D>().size=new Vector2(1f,8f);
		go.AddComponent<BoxCollider2D>().size=new Vector2(8f,1f);
		Destroy(go,0.2f);
		ParticleManager.Emit(explosionID,transform.position,transform.up,1);
	}
}
