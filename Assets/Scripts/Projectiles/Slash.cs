using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {
	public int spriteID;
	new private SpriteRenderer renderer;

	private void Start()
	{
		renderer=GetComponent<SpriteRenderer>();
	}

	void Update () {
		if(Ship.paused) return;
		transform.Translate(0,-Time.deltaTime*8,0);
		if(transform.position.y<-Scaler.sizeY) Destroy(gameObject);
		if(Bullet.bulletTime<=0) renderer.sprite=SpriteBase.I.bullets[spriteID+(Bullet.blink ? 0 : 1)];
	}
	public void OnCollisionEnter2D(Collision2D col)
	{
		//if(col.gameObject.name.Contains("Ship") && col.collider.name!="laserbody")Destroy(gameObject);
	}
}
