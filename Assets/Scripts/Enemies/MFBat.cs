using UnityEngine;

public class MFBat : EnemyBase {
	private float timer=2;
	private float sprite;
	private static Sprite[] bats;
	private static int shootId;
	private BulletPath path;
	Vector3 position;
	bool inv;
	Del update;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=20;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points=100;
		shootId=ei.bulletsID[0];
		if(bats==null)bats=ei.sprites;
		PathEnemy pe=(PathEnemy)ei;
		path=pe.bulletPath;
		inv=Random.value>0.5;
		fallSpeed=-4;
		update=Pathing;
	}
	public override void Position(int i)
	{
		base.Position(i);
		position=transform.position;
	}
	void Pathing(){
		if(timer>0)timer-=Time.deltaTime;
		else Shoot();
		if(sprite>0)sprite-=Time.deltaTime;
		_renderer.flipX=transform.position.x<GetPlayer(transform.position).position.x;
		if(!stopMovement)transform.position=position+BulletPath.Next(ref path,inv);
		if(path.Finished())update=SlowFall;
		if(sprite<=0)_renderer.sprite=bats[Mathf.RoundToInt(Mathf.PingPong(Time.time*3,1f))];
	}
	new void Update ()
	{
		if(Ship.paused) return;
		base.Update();
		update?.Invoke();
		
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
		timer=1.5f;
		//go.transform.localScale=Vector3.one*2;
	}
}
