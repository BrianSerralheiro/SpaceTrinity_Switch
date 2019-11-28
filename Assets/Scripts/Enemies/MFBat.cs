using UnityEngine;

public class MFBat : EnemyBase {
	private float timer=2;
	private float sprite;
	private static Sprite[] bats;
	private static int shootId;
	private BulletPath path;
	Vector3 position;
	bool inv;
	public override void SetSprites(EnemyInfo ei)
	{
		explosionID = 10;
		hp=50;
		points=100;
		shootId=ei.bulletsID[0];
		if(bats==null)bats=ei.sprites;
		PathEnemy pe=(PathEnemy)ei;
		path=pe.bulletPath;
		inv=Random.value>0.5;
	}
	public override void Position(int i)
	{
		base.Position(i);
		position=transform.position;
	}
	new void Update ()
	{
		if(Ship.paused) return;
		base.Update();
		if(timer>0)timer-=Time.deltaTime;
		else Shoot();
		if(sprite>0)sprite-=Time.deltaTime;
		_renderer.flipX=transform.position.x<player.position.x;
		transform.position=position+BulletPath.Next(ref path,inv);
		if(path.Finished())Die();
		if(sprite<=0)_renderer.sprite=bats[Mathf.RoundToInt(Mathf.PingPong(Time.time*3,1f))];
		
	}
	void Shoot()
	{
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		go.AddComponent<Slash>().spriteID=shootId;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position;
		go.transform.localScale=Vector3.right*3+Vector3.up*2;
		sprite=0.3f;
		_renderer.sprite=bats[2];
		timer=1.2f;
		//go.transform.localScale=Vector3.one*2;
	}
}
