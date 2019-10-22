using UnityEngine;

public class Header : EnemyBase {
	private Vector3[] dir={Vector3.right,Vector3.down,Vector3.left};
	private int  prev=1;
	private Vector3 rot;
	private float timer;
	private Core eyes;
	private Core core;
	Del movement;
	private static int shootId;
	public override void SetSprites(EnemyInfo ei)
	{
		explosionID = 9;
		hp=70;
		points=180;
		GameObject go=new GameObject("eyes");
		eyes=go.AddComponent<Core>().Set(ei.sprites[1],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,-0.28f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[2],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,0.42f);
		movement=Shooting;
		shootId=ei.bulletsID[0];
	}
	void Shooting(){
		timer+=Time.deltaTime;
		core.Min(Time.deltaTime);
		eyes.Set(Mathf.PingPong(Time.time/2,1));
		if(timer>1)
		{
			timer=0;
			Shoot();
			core.Set(1);
		}
		transform.Translate(0,-4*Time.deltaTime,0);
		if(transform.position.y<Scaler.sizeY/2)movement=Moving;
	}
	void Moving(){
		core.Min(Time.deltaTime);
		eyes.Set(Mathf.PingPong(Time.time/2,1));
		if(transform.position.x>0)
			transform.Translate(2*Time.deltaTime,-2*Time.deltaTime,0);
		else
			transform.Translate(-2*Time.deltaTime,-2*Time.deltaTime,0);
		if(transform.position.y<-Scaler.sizeY-2)Die();
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		movement();
	}
	void Shoot()
	{
		SoundManager.PlayEffects(12, 0.5f, 0.8f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		Bullet bu=go.AddComponent<Bullet>();
		bu.owner=name;
		bu.spriteID=shootId;
		go.transform.position=transform.position;
		Vector3 rotation = new Vector3(0,0,Mathf.Atan2(transform.position.x-player.position.x,player.position.y-transform.position.y)*Mathf.Rad2Deg);
		go.transform.eulerAngles=rotation;
	}

}
