using UnityEngine;

public class Header : EnemyBase {
	private int  prev=1;
	private Vector3 rot;
	private float timer;
	private Core eyes;
	private Core core;
	Del movement;
	BulletPath path;
	Vector3 position;
	private static int shootId;
	public override void SetSprites(EnemyInfo ei)
	{
		explosionID = 9;
		hp=70;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points=180;
		GameObject go=new GameObject("eyes");
		eyes=go.AddComponent<Core>().Set(ei.sprites[1],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,-0.28f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[2],new Color(0.4f,0f,0f));
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0,0.42f);
		movement=Pathing;
		shootId=ei.bulletsID[0];
		path=(ei as PathEnemy).bulletPath;
		fallSpeed=-4;
	}
	public override void Position(int i){
		base.Position(i);
		position=transform.position;
	}
	void Pathing(){
		core.Min(Time.deltaTime);
		eyes.Set(Mathf.PingPong(Time.time/2,1));
		transform.position=position+BulletPath.Next(ref path,position.x>0);
		if(path.Finished()){
			Shoot();
			movement=SlowFall;
		}
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		movement();
		core.Min(Time.deltaTime);
	}
	void Shoot()
	{
		core.Set(1);
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
