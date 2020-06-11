using UnityEngine;

public class Slasher : EnemyBase {

	private float timer;
	private Vector3 mod =new Vector3(0,-1f,0.1f);
	static int shotID,impactID;
	static GameObject trail;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		SetHP(40,ei.lifeproportion);
		fallSpeed=-1;
		shotID=ei.bulletsID[0];
		if(!trail)trail=ei.particles[0].gameObject;
		impactID=ei.particleID[1];
		Instantiate(ei.particles[2],transform);
		timer=Time.time+2;
	}

	new void Update()
	{
		if(Ship.paused)return;
		base.Update();
		if(timer<Time.time+1){
			if(GetPlayer(transform.position).position.x<transform.position.x)transform.Translate(-Time.deltaTime,0,0);
			else transform.Translate(Time.deltaTime,0,0);
		}
		SlowFall();
		if(transform.position.y>0 && Mathf.Abs(transform.position.x-GetPlayer(transform.position).position.x)<2 && timer<Time.time)
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
		Instantiate(trail,go.transform);
		Slash s=go.AddComponent<Slash>();
		s.spriteID=shotID;
		s.impactID=impactID;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+mod;
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
