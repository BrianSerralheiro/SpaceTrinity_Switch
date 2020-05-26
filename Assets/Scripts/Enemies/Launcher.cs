using UnityEngine;

public class Launcher : EnemyBase 
{
	private float timer=5,spd;
	private Transform rocket, burst,target;
	private Vector3 rot,scale=Vector3.one, pos=new Vector3(0,0.4f,0.1f);
	private Core core;
	private static Sprite rocketSprite,burstSprite;
	private int rocketTrail;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 100;
		hp=20;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		timer=5;
		rocketSprite=ei.sprites[1];
		burstSprite=ei.sprites[2];
		rocketTrail=ei.particleID[0];
		GameObject go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[3],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,0.22f);
	}
	public override void Position(int i){
		base.Position(i);
		Create();
	}
	private void Create()
	{
		target=GetPlayer();
		timer=1.5f;
		spd=0;
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=rocketSprite;
		go.AddComponent<Missile>().SetHP(20);
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		rocket=go.transform;
		rocket.position=transform.position;
		go = new GameObject("burst");
		go.AddComponent<SpriteRenderer>().sprite=burstSprite;
		burst=go.transform;
		burst.parent=rocket;
		burst.localPosition=new Vector3(0,-1f);
		scale.y=0;
		burst.localScale=scale;
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		if(transform.position.y>-Scaler.sizeY-1)transform.Translate(0,-Time.deltaTime,0);
		else Die();
		if(timer>0 && rocket)
		{
			rocket.position=transform.position+pos;
			scale.y=0;
			core.Min(Time.deltaTime*2);
		}
		if(timer <0)
		{
			core.Add(Time.deltaTime*2);
			if(rocket)
			{
				spd+=Time.deltaTime/2;
				if(spd>1)spd=1;
				scale.y=spd*5;
				burst.localScale=scale;
				Vector3 v=rocket.position-target.position;
				v.z=0;
				v.Normalize();
				if(timer>-4)rocket.Rotate(Vector3.Cross(v,rocket.up)*Time.deltaTime*270f*spd);
				rocket.Translate(0,Time.deltaTime*8*spd,0);
				ParticleManager.Emit(rocketTrail,burst.position,1);
				if(rocket.position.x<-Scaler.sizeX/2-2 || rocket.position.x>Scaler.sizeX/2+2 || rocket.position.y<-Scaler.sizeY-2 || rocket.position.y>Scaler.sizeY+4)Destroy(rocket.gameObject);
			}
			else Create();
		}

	}
	protected override void Die()
	{
		base.Die();
		if(rocket)rocket.GetComponent<Missile>().release=true;
	}
}
