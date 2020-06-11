using UnityEngine;

public class BatGirl : EnemyBase {

	private Transform wingL, wingR;
	private SpriteRenderer render;
	static Sprite left,closed;
	private Vector3 vector = new Vector3();
	private Vector3 pos = new Vector3(0.7f,1.32f,0.1f);
	private static Vector3[] poses={new Vector3(-1,-1,0).normalized,new Vector3(0,-1,0).normalized,new Vector3(1,-1,0).normalized };
	private float timer=3;
	static  EnemyInfo bat;
	private Del movement;
	private int count;
	static int pullID,birthID;
	public override void SetSprites(EnemyInfo ei)
	{
		SetHP(120,ei.lifeproportion);
		name+="big";
		points=150;
		GameObject go = new GameObject("wingL");
		render=go.AddComponent<SpriteRenderer>();
		left=ei.sprites[2];
		closed=ei.sprites[4];
		pullID=ei.particleID[0];
		birthID=ei.particleID[1];
		render.sprite=left;
		wingL=go.transform;
		go = new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(0.7f,1.32f,0.1f);
		wingR.localPosition=new Vector3(-0.7f,1.32f,0.1f);
		go = new GameObject("head");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0f,1.8f,-0.1f);
		bat=(ei as CarrierInfo).spawnable;
		movement=Fall;
		movement+=UpdateSpawn;
	}
	void Fall()
	{
		transform.Translate(0,-Time.deltaTime*2,0);
		if(transform.position.y<Scaler.sizeY / 12 && movement.GetInvocationList().Length>1)
		{
			movement=UpdateSpawn;
			count=2;
		}
		else if(transform.position.y<-Scaler.sizeY-2)Die();
	}
	void UpdateSpawn(){
		timer-=Time.deltaTime;
		if(timer<2)
		{
			render.sprite=closed;
			wingR.gameObject.SetActive(false);
			vector.Set(0,0,0);
			ParticleManager.Emit(pullID,transform.position,1);
			pos.z=-0.1f;
		}
		if(timer<0)
		{
			render.sprite=left;
			wingR.gameObject.SetActive(true);
			pos.z=0.1f;
			ParticleManager.Emit(birthID,transform.position,1);
			for(int i = 0; i<3; i++)
			{
				Bat(i);
			}
			timer=5;
			if(count>0){
				count--;
				if(count==0)movement=Fall;
			}
		}
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		vector.Set(0,0,Mathf.PingPong(Time.time*100,60f)-45f);
		movement();
		wingL.localPosition=pos;
		wingL.eulerAngles=vector;
		wingR.eulerAngles=-vector;
	}
	void Bat(int i)
	{
		GameObject go=new GameObject("enemy");
		Bat b=go.AddComponent<Bat>();
		b.target=transform.position+poses[i]*3f;
		b.SetSprites(bat);
		go.AddComponent<SpriteRenderer>().sprite=bat.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position;
	}
	protected override void Die()
	{
		// killed=true;
		base.Die();
		ParticleManager.Emit(1,transform.position,3,1);
	}
}
