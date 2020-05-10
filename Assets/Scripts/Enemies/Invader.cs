using UnityEngine;

public class Invader : EnemyBase {
	private bool left;
	private Vector3 vec=Vector3.left/10;
	private Vector3 mod=Vector3.forward/10;
	private float shoottimer = 1;
	private static int shootId;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 50;
		hp=20;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		shootId=ei.bulletsID[0];
	}
	void Shoot()
	{
		//SoundManager.PlayEffects(12, 0.1f, 0.5f);
		if(shoottimer>Time.time)return;
		shoottimer=Time.time+1;
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<CircleCollider2D>();
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shootId;
		go.transform.position=transform.position+vec*(left?1:-1)+mod;
		go.transform.up=-transform.up;
		left=!left;
	}
	new void Update () {
		if(Ship.paused)return;
		base.Update();
		int i=(int)(Time.time%5f*2);
		Vector3 v=Vector3.zero;
		if(i==4||i==9)Shoot();
		else if(i%2==0)v=Vector3.down;
			else if(i>1&&i<7)v=Vector3.right;
				else v=Vector3.left;
		transform.Translate(v*Time.deltaTime*2f);
	}
}
