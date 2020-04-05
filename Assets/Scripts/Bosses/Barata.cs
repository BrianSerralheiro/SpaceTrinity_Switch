using UnityEngine;

public class Barata : EnemyBase {

	private Transform wingL, wingR;
	private float speed=12,timer,dir=0.5f;
	private Transform[] legs;
	private Core crystal;
	private Vector3 rot = Vector3.zero,vector=Vector3.zero,mouthRot=Vector3.zero;
    Del update;
	int charges,spawns,shotId;
	private EnemyInfo div;
	/*REMOVER AQUI*/float time;
	public override void SetSprites(EnemyInfo ei)
	{
		/*REMOVER*/time=Time.time;
		name+="Boss";
		damageEffect = true;
		hp=700;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		GameObject go = new GameObject("BosswingL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		BoxCollider2D box = go.AddComponent<BoxCollider2D>();
		box.size = new Vector2(2.4f,8);
		box.offset = new Vector2(0,-4f);
		wingL=go.transform;
		go = new GameObject("BosswingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		box = go.AddComponent<BoxCollider2D>();
		box.size = new Vector2(2.4f,8);
		box.offset = new Vector2(0,-4f);
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(1.4f,3.8f,-0.1f);
		wingR.localPosition=new Vector3(-1.4f,3.8f,-0.1f);
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
		legs[0].localPosition=new Vector3(-0.6f,5f,0.1f);
		legs[1].localPosition=new Vector3(0.6f,5f,0.1f);
		legs[2].localPosition=new Vector3(-1.1f,4.2f,0.1f);
		legs[3].localPosition=new Vector3(1.1f,4.2f,0.1f);
		legs[4].localPosition=new Vector3(-1.7f,3f,0.1f);
		legs[5].localPosition=new Vector3(1.7f,3f,0.1f);
		legs[6].localPosition=new Vector3(-2,1.3f,0.1f);
		legs[7].localPosition=new Vector3(2,1.3f,0.1f);
		legs[8].localPosition=new Vector3(-1.94f,-1.4f,0.1f);
		legs[9].localPosition=new Vector3(1.94f,-1.4f,0.1f);
		legs[10].localPosition=new Vector3(-0.8f,-4.8f,0.1f);
		legs[11].localPosition=new Vector3(0.8f,-4.8f,0.1f);

		crystal.transform.parent=transform;
		crystal.transform.localPosition=new Vector3(0,0.2f,-0.01f);
        update=Intro;
		div=(ei as CarrierInfo).spawnable;
		shotId=ei.bulletsID[0];
	}
    void Intro(){
        transform.Translate(0,-Time.deltaTime*2,0);
		rot.z=Mathf.Cos(Time.time*2)*25-15;
		if(transform.position.y<0)update=Charge;
    }
    void Charge(){
        if(transform.position.y<-Scaler.sizeY-1)transform.Translate(0,Time.deltaTime*speed/4,0);
        else if(mouthRot.z<45)mouthRot.z+=30*Time.deltaTime;
			else {
				transform.Translate(0,Time.deltaTime*speed,0);
				crystal.Add(Time.deltaTime);
				rot.z=crystal.Value()*30;
			}
		if(transform.position.y>Scaler.sizeY+5){
			if(charges++>2){
				EnemySpawner.boss=true;
				/*REMOVER*/Debug.Log(Time.time-time);
				Shot(12);
				update=Spawning;
			}
	    	else {
				transform.position=new Vector3(Random.Range(-Scaler.sizeX,Scaler.sizeX)/2,-Scaler.sizeY-8);
				crystal.Set(0);
				mouthRot.z=rot.z=0;
			}
		}
    }
	void Spawning(){
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*2,0);
		if(charges>2 && vector.z<45)vector.z+=Time.deltaTime*15;
		if(spawns<3){
			timer+=Time.deltaTime;
			rot.z=Mathf.PingPong(timer,1)*25;
			if(timer>1.2)Spawn();
			if(transform.position.x>Scaler.sizeX/2-2)dir=-Mathf.Abs(dir);
			if(transform.position.x<-Scaler.sizeX/2+2)dir=Mathf.Abs(dir);
			transform.Translate(Time.deltaTime*speed*dir,0,0);
			crystal.Set(timer/2);
		}else if(transform.position.y>Scaler.sizeY+5){
				transform.position=new Vector3(Random.Range(-Scaler.sizeX,Scaler.sizeX)/2,-Scaler.sizeY-8);
				crystal.Set(0);
				mouthRot.z=0;
				spawns=0;
				update=Charge;
			}else {
				transform.Translate(0,Time.deltaTime*speed,0);
				if(rot.z>-30)rot.z-=Time.deltaTime*30;
			}
	}
	void Dying(){
		crystal.Set(0);
		transform.Translate(0,-Time.deltaTime*3,0,Space.World);
		transform.Rotate(0,0,Time.deltaTime*3);
		ParticleManager.Emit(1,transform.position,1);
		if(transform.position.y<-Scaler.sizeY-5)
		{
			Destroy(gameObject);
			EnemySpawner.boss=false;
			SoundManager.Play(3);
		}
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
        update?.Invoke();
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
		update=Dying;
		EnemySpawner.points[killerid]+=500;
		foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
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
		diver.SetTimer(0);
		diver.transform.position=transform.position;
		diver.transform.rotation=transform.rotation;
	}
	void Shot(int c){
		// SoundManager.PlayEffects(12, 0.5f, 0.8f);
		float degrees=180f/c;
		for (int i = 0; i < c; i++)
		{
			GameObject go = new GameObject("enemybullet");
			go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
			go.AddComponent<CircleCollider2D>();
			Bullet bu=go.AddComponent<Bullet>();
			bu.owner=name;
			bu.spriteID=shotId;
			bu.bulleSpeed=8f;
			bu.Timer(10);
			go.transform.position=crystal.transform.position;
			go.transform.eulerAngles=new Vector3(0,0,90f+degrees*i);
		}
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(vector.z>15 && update!=Dying) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(3,col.collider.transform.position,1);
	}
}
