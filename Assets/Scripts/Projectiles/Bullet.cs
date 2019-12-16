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
	public float bulleSpeed=12.5f;
	new protected SpriteRenderer renderer;
	public static List<Sprite> sprites=new List<Sprite>();
	protected virtual void Start()
	{
		renderer=GetComponent<SpriteRenderer>();
		timer=2;
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
		transform.Translate(0,Time.deltaTime*bulleSpeed,0);
		if(bulletTime<=0)renderer.sprite=sprites[spriteID+(blink?0:1)];
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
