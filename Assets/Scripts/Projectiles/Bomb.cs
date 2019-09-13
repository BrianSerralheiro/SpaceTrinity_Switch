using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyBase {
	private Vector3 pos;
	private Vector3 aim;
	private float speed=2;
	new void Start () {
		base.Start();
		hp=5;
		pos=transform.position;
		aim=(player.position-pos).normalized*20+pos;
	}
	
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		pos=Vector3.MoveTowards(pos,aim,Time.deltaTime*speed);
		speed+=Time.deltaTime;
		if(speed>8)speed=8;
		transform.position=pos;
		transform.Rotate(0,0,Time.deltaTime*90);
		if((pos-player.position).sqrMagnitude<2.25f)Explode();
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
		ParticleManager.Emit(10,transform.position,1);
	}
	private void Explode()
	{
		Destroy(gameObject);
		GameObject go=new GameObject("enemy");
		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
		go.AddComponent<BoxCollider2D>().size=new Vector2(0.5f,2.5f);
		go.AddComponent<BoxCollider2D>().size=new Vector2(2.5f,0.5f);
		Destroy(go,0.3f);
		ParticleManager.Emit(11,transform,1);
	}
}
