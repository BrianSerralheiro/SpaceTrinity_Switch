using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : EnemyBase {
	private SpriteRenderer energy;
	private SpriteRenderer zap;
	private float timer = 5;
	private Vector3 rot = new Vector3();
	private Vector3 scale = Vector3.one;
	private static Sprite[] sprites;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 150;
		explosionID = 10;
		hp=80;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		GameObject go=new GameObject("zap");
		if(sprites==null){
			sprites=new Sprite[4];
			sprites[0]=ei.sprites[1];
			sprites[1]=ei.sprites[2];
			sprites[2]=ei.sprites[3];
			sprites[3]=ei.sprites[4];
		}
		zap=go.AddComponent<SpriteRenderer>();
		zap.sprite=ei.sprites[1];
		BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size=new Vector2(0.7f,4);
		col.offset=new Vector2(0,2.2f);
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,2,0.1f);
		go.transform.localScale=new Vector3(1,4);
		go.SetActive(false);
		go=new GameObject("energy");
		energy=go.AddComponent<SpriteRenderer>();
		energy.sprite=sprites[2];
		energy.transform.parent=transform;
		energy.transform.localPosition=new Vector3(0,2,-0.1f);
		energy.gameObject.SetActive(false);
		
	}
	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="zap") return;
		base.OnCollisionEnter2D(col);
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		if(transform.position.y<-Scaler.sizeY-2)Die();
		if(timer>1){
			transform.Translate(0,-Time.deltaTime,0,Space.World);
			Vector3 v=GetPlayer(transform.position).position-transform.position;
            v.z=0;
            v=Vector3.Cross(-transform.up,v);
            transform.Rotate(v*Time.deltaTime*15);
		}else if(timer >0.1f)
		{
			energy.sprite=Bullet.blink? sprites[2]:sprites[3];
			energy.gameObject.SetActive(true);
			scale.x=scale.y=0.9f-timer;
			energy.transform.localScale=scale;
		}else if(timer >0)
		{
			zap.gameObject.SetActive(true);
			energy.sprite=Bullet.blink ? sprites[2] : sprites[3];
			zap.sprite=Bullet.blink ? sprites[0] : sprites[1];
		}
		else
		{
			timer=5;
			zap.gameObject.SetActive(false);
			energy.gameObject.SetActive(false);
		}
	}
}
