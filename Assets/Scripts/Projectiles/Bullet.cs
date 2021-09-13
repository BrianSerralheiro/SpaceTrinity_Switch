 using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	protected float timer=2,spriteTimer,blinkTimer=0.1f;
	public string owner;
	public int damage;
	public bool pierce;
	public static float bulletTime;
	public static bool blink;
	public int spriteID;
	public int particleID = 4;
	public int impactID = 3;
	public float bulletSpeed=12.5f, maxSpeed, _time, delay;
	new protected SpriteRenderer renderer;
	public static List<Sprite> sprites=new List<Sprite>();
	protected virtual void Start()
	{
		_time = Time.time;
		renderer=GetComponent<SpriteRenderer>();
		timer+=maxSpeed;
		spriteTimer=_time+blinkTimer;
	}
	public void Timer(float f){
		timer=f;
	}
	public static int Register(Sprite sp)
	{
		if(sprites.Contains(sp)){
			return sprites.IndexOf(sp);
		}
		sprites.Add(sp);
		return sprites.Count-1;
	}
	void Update()
	{
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		transform.Translate(0,Time.deltaTime*(maxSpeed > 0? Mathf.Clamp((Time.time -_time) / maxSpeed*bulletSpeed , 0 , bulletSpeed): bulletSpeed),0);
		if(Time.time>spriteTimer){
			renderer.sprite=sprites[spriteID+(renderer.sprite==sprites[spriteID]?1:0)];
			spriteTimer=Time.time+blinkTimer;
		}
		timer-=Time.deltaTime;
		if(timer<=0) Destroy(gameObject);
	}
	protected void OnCollisionEnter2D(Collision2D col)
	{
		if(!col.collider.name.Contains("laser") && !col.collider.name.Contains("bullet") && col.gameObject.name.Substring(0,3)!=owner.Substring(0,3)){
			if(!pierce)Destroy(gameObject);
			
			ParticleManager.Emit(impactID,col.contacts[0].point,1);
		}
	}
	
}
