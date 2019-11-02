using UnityEngine;

public class Barata : EnemyBase {

	private Transform wingL, wingR;
	private float speed=4,timer,dir=0.5f;
	private Transform[] legs;
	private Core crystal;
	private Vector3 rot = Vector3.zero,vector=Vector3.zero,mouthRot=Vector3.zero;
    Del update;
	int charges,spawns;
	private EnemyInfo div;
	enum State
	{
		intro,
		moving,
		charging,
		dead
	}
	State state;
	public override void SetSprites(EnemyInfo ei)
	{
		BossWarning.Show();
		SoundManager.Play(2);
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1000;
		GameObject go = new GameObject("wingL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		BoxCollider2D box = go.AddComponent<BoxCollider2D>();
		box.size = new Vector2(1.2f,4);
		box.offset = new Vector2(0,-2f);
		wingL=go.transform;
		go = new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		box = go.AddComponent<BoxCollider2D>();
		box.size = new Vector2(1.2f,4);
		box.offset = new Vector2(0,-2f);
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(0.7f,1.9f,-0.1f);
		wingR.localPosition=new Vector3(-0.7f,1.9f,-0.1f);
		legs=new Transform[12];
		go = new GameObject("crystal");
		crystal=go.AddComponent<Core>().Set(ei.sprites[15],new Color(0.4f,0f,0.4f));
		for(int i = 3; i<15; i++)
		{
			go = new GameObject("leg"+(i-3));
			go.AddComponent<SpriteRenderer>().sprite=ei.sprites[i];
			legs[i-3]=go.transform;
			go.transform.parent=transform;
		}
		legs[0].localPosition=new Vector3(-0.3f,2.5f,0.1f);
		legs[1].localPosition=new Vector3(0.3f,2.5f,0.1f);
		legs[2].localPosition=new Vector3(-0.55f,2.1f,0.1f);
		legs[3].localPosition=new Vector3(0.55f,2.1f,0.1f);
		legs[4].localPosition=new Vector3(-0.85f,1.5f,0.1f);
		legs[5].localPosition=new Vector3(0.85f,1.5f,0.1f);
		legs[6].localPosition=new Vector3(-1,0.65f,0.1f);
		legs[7].localPosition=new Vector3(1,0.65f,0.1f);
		legs[8].localPosition=new Vector3(-0.97f,-0.7f,0.1f);
		legs[9].localPosition=new Vector3(0.97f,-0.7f,0.1f);
		legs[10].localPosition=new Vector3(-0.4f,-2.4f,0.1f);
		legs[11].localPosition=new Vector3(0.4f,-2.4f,0.1f);

		crystal.transform.parent=transform;
		crystal.transform.localPosition=new Vector3(0,0.1f,-0.01f);
        update=Intro;
		div=(ei as CarrierInfo).spawnable;
	}
    void Intro(){
        transform.Translate(0,-Time.deltaTime*2,0);
		rot.z=Mathf.Cos(Time.time*2)*25-15;
		if(transform.position.y<0)update=Charge;
    }
    void Charge(){
        if(transform.position.y<-Scaler.sizeY-1)transform.Translate(0,Time.deltaTime,0);
        else if(mouthRot.z<45)mouthRot.z+=30*Time.deltaTime;
			else {
				transform.Translate(0,Time.deltaTime*speed,0);
				crystal.Add(Time.deltaTime);
				rot.z=crystal.Value()*30;
			}
		if(transform.position.y>Scaler.sizeY+3){
			if(charges++>=5){
				update=Spawning;
			}
	    	else {
				transform.position=new Vector3(Random.Range(-Scaler.sizeX,Scaler.sizeX)/2,-Scaler.sizeY-5);
				crystal.Set(0);
				mouthRot.z=rot.z=0;
			}
		}
    }
	void Spawning(){
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*2,0);
		if(vector.z<45)vector.z+=Time.deltaTime*30;
		if(spawns<3){
			timer+=Time.deltaTime;
			rot.z=Mathf.PingPong(timer,1)*25;
			if(timer>2)Spawn();
			if(transform.position.x>Scaler.sizeX/2-2)dir=-Mathf.Abs(dir);
			if(transform.position.x<-Scaler.sizeX/2+2)dir=Mathf.Abs(dir);
			transform.Translate(Time.deltaTime*speed*dir,0,0);
			crystal.Set(timer/2);
		}else if(transform.position.y>Scaler.sizeY+3){
				transform.position=new Vector3(Random.Range(-Scaler.sizeX,Scaler.sizeX)/2,-Scaler.sizeY-5);
				crystal.Set(0);
				mouthRot.z=vector.z=0;
				spawns=0;
				update=Charge;
			}else {
				transform.Translate(0,Time.deltaTime*speed,0);
				if(rot.z>-30)rot.z-=Time.deltaTime*30;
			}
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
        update?.Invoke();
		if(state==State.dead)
		{
			crystal.Set(0);
			transform.Translate(0,-Time.deltaTime*3,0,Space.World);
			transform.Rotate(0,0,Time.deltaTime*3);
			ParticleManager.Emit(8,(Vector3)Random.insideUnitCircle*2+transform.position,1);
			if(transform.position.y<-Scaler.sizeY-4){
				Destroy(gameObject);
				EnemySpawner.boss=false;
				SoundManager.Play(3);
			}
		}
		if(hp>0){
			wingL.localEulerAngles=vector;
			wingR.localEulerAngles=-vector;
			
			legs[0].localEulerAngles=mouthRot/4;
			legs[1].localEulerAngles=-mouthRot/4;
			legs[2].localEulerAngles=mouthRot;
			legs[3].localEulerAngles=-mouthRot;
			legs[4].localEulerAngles=rot;
			legs[5].localEulerAngles=-rot;
			legs[6].localEulerAngles=rot;
			legs[7].localEulerAngles=-rot;
			legs[8].localEulerAngles=rot;
			legs[9].localEulerAngles=-rot;
			legs[10].localEulerAngles=rot;
			legs[11].localEulerAngles=-rot;
		}
	}
	protected override void Die()
	{
		state=State.dead;
		EnemySpawner.points+=500;
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
	void Spawn()
	{
		spawns++;
		timer=0;
		GameObject go=new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=div.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		Diver diver=go.AddComponent<Diver>();
		diver.SetSprites(div);
		diver.transform.position=transform.position;
		diver.transform.rotation=transform.rotation;
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(vector.z>5 && state!=State.dead) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(16,col.collider.transform.position,1);
		speed=hp<350 ? 12 : 8;
	}
}
