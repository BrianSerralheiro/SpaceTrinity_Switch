using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyBase {
	private Vector3 pos;
	private Vector3 aim;
	private float currentSpeed=2,maxSpeed,rotaion;
	int explosionID;
	public void Set(int h,float r,int id,Vector3 t,float s1,float s2)
	{
		hp=h;
		rotaion=r;
		transform.rotation=Quaternion.Euler(0,0,r*Random.value);
		currentSpeed=s1;
		maxSpeed=s2;
		explosionID=id;
		pos=transform.position;
		aim=t;
	}
	
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		pos=Vector3.MoveTowards(pos,aim,Time.deltaTime*currentSpeed);
		currentSpeed=Mathf.MoveTowards(currentSpeed,maxSpeed,Time.deltaTime*2);
		transform.position=pos;
		transform.Rotate(0,0,Time.deltaTime*rotaion);
		// if((pos-GetPlayer(transform.position).position).sqrMagnitude<10)Explode();
		if(pos==aim)Explode();
	}
	public new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.name.Contains("Player"))Explode();
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
