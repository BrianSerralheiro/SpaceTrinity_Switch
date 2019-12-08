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
		explosionID = 10;
		hp=30;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		shootId=ei.bulletsID[0];
	}
	void Shoot()
	{
		SoundManager.PlayEffects(12, 0.1f, 0.5f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shootId;
		go.transform.position=transform.position+vec*(left?1:-1)+mod;
		go.transform.up=-transform.up;
		left=!left;
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		shoottimer-=Time.deltaTime;
		if(shoottimer<=0)
		{
			shoottimer=0.5f;
			Shoot();
		}
		if(player.position.x<transform.position.x)transform.Translate(-Time.deltaTime*2,0,0);
		else transform.Translate(Time.deltaTime*2,0,0);
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime,0);
	}
}
