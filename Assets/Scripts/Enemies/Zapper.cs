using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : EnemyBase {
	private SpriteRenderer energy;
	private SpriteRenderer zap;
	private float timer = 5;
	private Vector3 rot = new Vector3();
	private Vector3 scale = Vector3.one;
	new void Start () {
		base.Start();
		points = 150;
		explosionID = 10;
		hp=80;
		GameObject go=new GameObject("zap");
		zap=go.AddComponent<SpriteRenderer>();
		zap.sprite=SpriteBase.I.zapper[1];
		BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size=new Vector2(0.7f,4);
		col.offset=new Vector2(0,2.2f);
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,1,0.1f);
		go.transform.localScale=new Vector3(1,4);
		go.SetActive(false);
		go=new GameObject("energy");
		energy=go.AddComponent<SpriteRenderer>();
		energy.sprite=SpriteBase.I.zapper[3];
		energy.transform.parent=transform;
		energy.transform.localPosition=new Vector3(0,1,-0.1f);
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
			rot.Set(0,0,Mathf.Atan2(transform.position.x-player.position.x,player.position.y-transform.position.y)*Mathf.Rad2Deg);
			transform.eulerAngles=rot;
		}else if(timer >0.1f)
		{
			energy.sprite=Bullet.blink? SpriteBase.I.zapper[3]:SpriteBase.I.zapper[4];
			energy.gameObject.SetActive(true);
			scale.x=scale.y=0.9f-timer;
			energy.transform.localScale=scale;
		}else if(timer >0)
		{
			zap.gameObject.SetActive(true);
			energy.sprite=Bullet.blink ? SpriteBase.I.zapper[3] : SpriteBase.I.zapper[4];
			zap.sprite=Bullet.blink ? SpriteBase.I.zapper[1] : SpriteBase.I.zapper[2];
		}
		else
		{
			timer=5;
			zap.gameObject.SetActive(false);
			energy.gameObject.SetActive(false);
		}
	}
}
