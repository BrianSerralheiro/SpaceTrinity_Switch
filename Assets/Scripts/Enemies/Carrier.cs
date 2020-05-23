using UnityEngine;

public class Carrier : EnemyBase {
	private float timer=-2f;
	private Diver diver;

	private Transform[] legs;
	private Core crystal;
	private Vector3 vector = new Vector3();
	private EnemyInfo div;
	public override void SetSprites(EnemyInfo ei)
	{
		name+="big";
		hp=75;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 150;
		legs=new Transform[6];
		GameObject go = new GameObject("crystal");
		crystal=go.AddComponent<Core>().Set(ei.sprites[7],new Color(0.4f,0f,0.4f));
		for(int i = 1; i<7; i++)
		{
			go = new GameObject("leg"+i);
			go.AddComponent<SpriteRenderer>().sprite=ei.sprites[i];
			legs[i-1]=go.transform;
			go.transform.parent=transform;
			go.transform.rotation=transform.rotation;
		}
		legs[0].localPosition=new Vector3(0.9f,0.6f,0.1f);
		legs[1].localPosition=new Vector3(-0.9f,0.6f,0.1f);
		legs[2].localPosition=new Vector3(1.4f,3.4f,0.1f);
		legs[3].localPosition=new Vector3(-1.4f,3.4f,0.1f);
		legs[4].localPosition=new Vector3(1.8f,5.8f,0.1f);
		legs[5].localPosition=new Vector3(-1.8f,5.8f,0.1f);
		crystal.transform.parent=transform;
		crystal.transform.rotation=transform.rotation;
		crystal.transform.localPosition=new Vector3(0,6.2f);
		div=(ei as CarrierInfo).spawnable;
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		transform.Translate(0,-Time.deltaTime * 2,0);
		timer+=Time.deltaTime;
		if(timer<0)crystal.Min(Time.deltaTime);
		else  if(timer<2)
		{
			crystal.Add(Time.deltaTime*2);
		}
		else 
		{
			Spawn();
			timer =-1;
		}
		vector.Set(0,0,Mathf.PingPong(Time.time*50,45f));
		legs[0].localEulerAngles=vector;
		legs[1].localEulerAngles=-vector;
		legs[2].localEulerAngles=-vector;
		legs[3].localEulerAngles=vector;
		legs[4].localEulerAngles=vector;
		legs[5].localEulerAngles=-vector;

		if(transform.position.x<-Scaler.sizeX/2f-4.2F || transform.position.x>Scaler.sizeX/2f+4.2F || transform.position.y<-Scaler.sizeY-8.2F) Die();
	}
	void Spawn()
	{
		GameObject go=new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=div.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		diver =go.AddComponent<Diver>();
		diver.SetSprites(div);
		diver.Fall(2);
		diver.SetTimer(1.4f);
		diver.transform.position=transform.position+new Vector3(0,5.3f,0.5f);
		diver.transform.rotation=transform.rotation;
	}
	protected override void Die()
	{
		killed=true;
		base.Die();
		ParticleManager.Emit(1,transform.position+transform.up*3,10);
	}
}
