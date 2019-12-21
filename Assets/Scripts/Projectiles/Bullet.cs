 using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	protected float timer,spriteTimer,blinkTimer=0.1f;
	public string owner;
	public int damage;
	public bool pierce;
	public static float bulletTime;
	public static bool blink;
	public int spriteID;
	public int particleID = 2;
	public float bulleSpeed=12.5f, maxSpeed, _time;
	new protected SpriteRenderer renderer;
	public static List<Sprite> sprites=new List<Sprite>();
	protected virtual void Start()
	{
		_time = Time.time;
		renderer=GetComponent<SpriteRenderer>();
		timer=2 + maxSpeed;
		spriteTimer=_time+blinkTimer;
	}
	public static int Register(Sprite sp){
		if(sprites.Contains(sp)){
			Debug.Log("Sprite "+sp.name+" alredy added");
			return sprites.IndexOf(sp);
		}
		sprites.Add(sp);
		return sprites.Count-1;
	}
	void Update()
	{
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		transform.Translate(0,Time.deltaTime*(maxSpeed > 0? Mathf.Clamp((Time.time -_time) / maxSpeed*bulleSpeed , 0 , bulleSpeed): bulleSpeed),0);
		if(Time.time>spriteTimer){
			renderer.sprite=sprites[spriteID+(renderer.sprite==sprites[spriteID]?1:0)];
			spriteTimer=Time.time+blinkTimer;
		}
		timer-=Time.deltaTime;
		if(timer<=0) Destroy(gameObject);
	}
	protected void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.name!="laser" && col.gameObject.name.Substring(0,3)!=owner.Substring(0,3) && !pierce){
			Destroy(gameObject);
		}
	}
	
}
