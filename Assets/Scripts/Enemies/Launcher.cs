using UnityEngine;

public class Launcher : EnemyBase 
{
	private float timer=5,spd;
	private Transform rocket,target;
	private Vector3 rot, pos=new Vector3(0,0.4f,0.1f);
	private Core core;
	private static Sprite rocketSprite;
	private int rocketTrail;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 100;
		SetHP(20,ei.lifeproportion);
		timer=5;
		rocketSprite=ei.sprites[1];
		rocketTrail=ei.particleID[0];
		GameObject go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[3],Color.black);
		core.white=new Color(0.5f,0.1f,0.05f);
		EnemySpawner.AddPost(go);
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,0.22f,-0.1f);
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
		Missile mis=go.AddComponent<Missile>();
		mis.SetHP(20,1.2f);
		mis.trailID=rocketTrail;
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		rocket=go.transform;
		rocket.position=transform.position;
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
			core.Min(Time.deltaTime*2);
		}
		if(timer <0)
		{
			core.Add(Time.deltaTime*2);
			if(rocket)
			{
				spd+=Time.deltaTime/2;
				if(spd>1)spd=1;
				Vector3 v=rocket.position-target.position;
				v.z=0;
				v.Normalize();
				if(timer>-4)rocket.Rotate(Vector3.Cross(v,rocket.up)*Time.deltaTime*270f*spd);
				rocket.Translate(0,Time.deltaTime*8*spd,0);
				ParticleManager.Emit(rocketTrail,rocket.position-rocket.up+Vector3.forward/5f,1);
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
