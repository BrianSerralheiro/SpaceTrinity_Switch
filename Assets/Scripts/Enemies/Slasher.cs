using UnityEngine;

public class Slasher : EnemyBase {

	private float timer;
	private Vector3 mod =new Vector3(0,-1f,0.1f);
	static int shotID;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		hp=90;
		fallSpeed=-1;
		shotID=ei.bulletsID[0];
		timer=Time.time+2;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
	}

	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		if(timer>Time.time+1){
			if(GetPlayer(transform.position).position.x<transform.position.x) transform.Translate(-Time.deltaTime,0,0);
			else transform.Translate(Time.deltaTime,0,0);
		}
		SlowFall();
		if(transform.position.y>Scaler.sizeY/3 && timer<Time.time)
		{
			Shoot();
		}
		fallSpeed=Mathf.MoveTowards(fallSpeed,-6,Time.deltaTime/5);
	}
	void Shoot()
	{
		timer=Time.time+2;
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
		go.AddComponent<BoxCollider2D>();
		go.AddComponent<Slash>().spriteID=shotID;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+mod;
		//go.transform.localScale=Vector3.one*2;
	}
    void OnCollisionStay2D(Collision2D col)
    {
        Slasher slasher=col.gameObject.GetComponent<Slasher>();
        if(slasher)
        {
            if(slasher.transform.position.x<transform.position.x)transform.Translate(Time.deltaTime,0,0,Space.World);
            else transform.Translate(-Time.deltaTime,0,0,Space.World);
        }
    }
}
