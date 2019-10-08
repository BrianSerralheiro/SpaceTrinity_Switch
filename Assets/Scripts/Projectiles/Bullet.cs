using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	protected float timer;
	public string owner;
	public int damage;
	public bool pierce;
	public static float bulletTime;
	public static bool blink;
	public int spriteID;
	public int particleID = 2;
	new protected SpriteRenderer renderer;
	private void Start()
	{
		renderer=GetComponent<SpriteRenderer>();
		timer=2;
	}
	void Update()
	{
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		transform.Translate(0,Time.deltaTime*12.5f,0);
		if(bulletTime<=0)renderer.sprite=SpriteBase.I.bullets[spriteID+(blink?0:1)];
		timer-=Time.deltaTime;
		if(timer<=0) Destroy(gameObject);
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.name!="laser" && col.gameObject.name!=owner && !pierce){
			Destroy(gameObject);
		}
	}
	
}
