using UnityEngine;

public class Slasher : EnemyBase {

	private float timer = 3;
	private Vector3 mod =new Vector3(0,-1f,0.1f);
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		hp=90;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
	}

	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		if(timer>1){
			if(GetPlayer(transform.position).position.x<transform.position.x) transform.Translate(-Time.deltaTime,0,0);
			else transform.Translate(Time.deltaTime,0,0);
		}
		if(transform.position.y>Scaler.sizeY/3) transform.Translate(0,-Time.deltaTime,0);
		if(timer<=0)
		{
			timer=2;
			Shoot();
		}
	}
	void Shoot()
	{
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[16];
		go.AddComponent<BoxCollider2D>();
		go.AddComponent<Slash>().spriteID=16;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+mod;
		//go.transform.localScale=Vector3.one*2;
	}
}
