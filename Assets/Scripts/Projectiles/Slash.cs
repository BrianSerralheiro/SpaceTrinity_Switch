using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {
	public int spriteID;
	SpriteRenderer _renderer;
	float spriteTimer,blinkTimer=0.1f;
	private void Start()
	{
		spriteTimer=Time.time+blinkTimer;
		_renderer=GetComponent<SpriteRenderer>();
	}

	void Update () {
		if(Ship.paused) return;
		transform.Translate(0,-Time.deltaTime*8,0);
		if(Time.time>spriteTimer){
			_renderer.sprite=Bullet.sprites[spriteID+(_renderer.sprite==Bullet.sprites[spriteID]?1:0)];
			spriteTimer=Time.time+blinkTimer;
		}
		if(transform.position.y<-Scaler.sizeY)Destroy(gameObject);
	}
}
