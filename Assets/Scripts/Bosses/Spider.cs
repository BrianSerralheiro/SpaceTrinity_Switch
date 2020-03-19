using UnityEngine;

public class Spider : EnemyBase {

	private Transform back,bitting,target;
	private float speed=5,timer;
	private int charges,spawns,webshot;
	private Transform[] legs;
	private Core crystal,headCrystal;
	private Spiderling diver;
	private EnemyInfo spiderling,info;
	private Vector3[] rot;
	private Transform[] webs;
	Del update;
	private Vector3 vector = new Vector3();
	private float dir=1;
	public override void SetSprites(EnemyInfo ei)
	{
		info=ei;
		BossWarning.Show();
		name+="Boss";
		SoundManager.Play(2);
		damageEffect = true;
		EnemySpawner.boss=true;
		BoxCollider2D collider2D=GetComponent<BoxCollider2D>();
		collider2D.size=new Vector2(3.7f,9);
		hp=1000;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		BoxCollider2D trigger=gameObject.AddComponent<BoxCollider2D>();
		trigger.isTrigger=true;
		trigger.offset=Vector2.up*-5;
		trigger.size=Vector2.one*2;
		update=Building;
		spiderling=((CarrierInfo)ei).spawnable;
		EnemySpawner.freeze=true;
		webshot=ei.bulletsID[0];
	}

	void Building(){
		if(!back){
			GameObject go=new GameObject("Back");
			go.AddComponent<SpriteRenderer>().sprite=info.sprites[1];
			back=go.transform;
			back.parent=transform;
			back.localPosition=new Vector3(0,0.26f,0.1f);

			go=new GameObject("Back Crystal");
			crystal=go.AddComponent<Core>().Set(info.sprites[2],new Color(0.4f,0f,0.4f));
			go.transform.parent=transform;
			go.transform.localPosition=new Vector3(0,-0.8f,-0.1f);
			
			go=new GameObject("Head Crystal");
			headCrystal=go.AddComponent<Core>().Set(info.sprites[3],new Color(0.4f,0f,0.4f));
			go.transform.parent=transform;
			go.transform.localPosition=new Vector3(0,-3.74f,-0.1f);

			webs=new Transform[5];
			for (int i = 0; i < webs.Length; i++)
			{
				go=new GameObject("web"+i);
				go.AddComponent<SpriteRenderer>().sprite=info.sprites[8];
				webs[i]=go.transform;
				webs[i].position=new Vector3(-Scaler.sizeX/2+ i* Scaler.sizeX/4,Scaler.sizeY,1);
				webs[i].localScale=Vector3.right;
				webs[i].Rotate(0,0,Mathf.Cos(i*45)*15);
			}
		}else if(legs==null){
			legs=new Transform[12];
			rot=new Vector3[12];
			GameObject go;
			SpriteRenderer p;
			for (int i = 0; i < 6; i+=2)
			{
				go=new GameObject("Leg"+(i/2)+"L");
				go.AddComponent<SpriteRenderer>().sprite=info.sprites[4];				
				go.transform.parent=transform;
				legs[i]=go.transform;

				go=new GameObject("Leg"+(i/2)+"R");
				p=go.AddComponent<SpriteRenderer>();
				p.sprite=info.sprites[4];
				p.flipX=true;			
				go.transform.parent=transform;
				legs[i+1]=go.transform;
			}
			for (int i = 0; i < 4; i+=2)
			{
				go=new GameObject("Arm"+(i/2)+"L");
				go.AddComponent<SpriteRenderer>().sprite=info.sprites[5+i/2];				
				go.transform.parent=transform;
				legs[6+i]=go.transform;

				go=new GameObject("Arm"+(i/2)+"R");
				p=go.AddComponent<SpriteRenderer>();
				p.sprite=info.sprites[5+i/2];
				p.flipX=true;
				go.transform.parent=transform;
				legs[7+i]=go.transform;
			}
			go=new GameObject("ButArm10L");
			go.AddComponent<SpriteRenderer>().sprite=info.sprites[7];				
			go.transform.parent=back;
			legs[10]=go.transform;

			go=new GameObject("ButArm11R");
			p=go.AddComponent<SpriteRenderer>();
			p.sprite=info.sprites[7];
			p.flipX=true;
			go.transform.parent=back;
			legs[11]=go.transform;
			update=Intro;
			timer=6;
			legs[0].localPosition=new Vector3(-1.97f,-1.78f,0.1f);
			legs[1].localPosition=new Vector3(1.97f,-1.78f,0.1f);
			legs[2].localPosition=new Vector3(-3.21f,-0.28f,0.1f);
			legs[3].localPosition=new Vector3(3.21f,-0.28f,0.1f);
			legs[4].localPosition=new Vector3(-3.21f,1.66f,0.1f);
			legs[5].localPosition=new Vector3(3.21f,1.66f,0.1f);
			legs[6].localPosition=new Vector3(-1.25f,-4.1f,0.1f);
			legs[7].localPosition=new Vector3(1.25f,-4.1f,0.1f);
			legs[8].localPosition=new Vector3(-0.5f,-4.5f,0.1f);
			legs[9].localPosition=new Vector3(0.5f,-4.5f,0.1f);
			legs[10].localPosition=new Vector3(-0.6f,3.65f,0.1f);
			legs[11].localPosition=new Vector3(0.6f,3.65f,0.1f);
		}
	}
	void Intro(){
		timer-=Time.deltaTime;
		transform.Translate(timer*dir*Time.deltaTime,-Time.deltaTime*2,0);
		rot[0].z=Mathf.Cos(Time.time)*-15f-10;
		rot[1].z=Mathf.Cos(Time.time+40)*15+10;
		rot[2].z=Mathf.Cos(Time.time*3)*-30;
		rot[3].z=Mathf.Cos(Time.time*3+25)*30;
		rot[4].z=Mathf.Cos(Time.time*2)*-30-30;
		rot[5].z=Mathf.Cos(Time.time*2+60)*30+30;
		rot[6].z=rot[8].z=Mathf.Sin(Time.time)*-15;
		rot[7].z=rot[9].z=Mathf.Sin(Time.time)*15;
		rot[10].z=-35;
		rot[11].z=35;
		for (int i = 0; i < webs.Length; i++)
		{
			webs[i].localScale=Vector3.MoveTowards(webs[i].localScale,new Vector3(1,2),Time.deltaTime*8);
		}
		if(transform.position.x>3)dir=-1;
		if(transform.position.x<-3)dir=1;
		if(timer<=0){
			update=Charge;
			target=GetPlayer();
		}
	}
	void Charge(){
		// headCrystal.Min(Time.deltaTime);
		if(timer>0){
			timer-=Time.deltaTime;
			if(timer<2.5 && charges<0){
				Shoot();
				charges++;
			}
			if(transform.position.y<Scaler.sizeY)transform.Translate(0,Time.deltaTime*speed,0);
			if((int)Time.time%5==0)dir*=-1;
			if(transform.position.x<target.position.x-3)dir=1;
			if(transform.position.x>target.position.x+3)dir=-1;
			transform.Translate(dir*Time.deltaTime,0,0);
			rot[0].z=Mathf.Sin(Time.time*4)*-15f-15;
			rot[1].z=Mathf.Sin(Time.time*4+60)*15+15;
			rot[2].z=Mathf.Sin(Time.time*4)*-20-20;
			rot[3].z=Mathf.Sin(Time.time*4+15)*20+20;
			rot[4].z=Mathf.Sin(Time.time*4)*-40-30;
			rot[5].z=Mathf.Sin(Time.time*4+30)*40+30;
			rot[6].z=rot[8].z*=0.9f;
			rot[7].z=rot[9].z*=0.9f;
			headCrystal.Min(Time.deltaTime);
		}
		else {
			transform.Translate(0,-Time.deltaTime*speed*2,0);
			rot[0].z=Mathf.Cos(Time.time*10)*-15f;
			rot[1].z=Mathf.Cos(Time.time*10+60)*15;
			rot[2].z=Mathf.Cos(Time.time*10)*-30;
			rot[3].z=Mathf.Cos(Time.time*10+45)*30;
			rot[4].z=Mathf.Cos(Time.time*10)*-30-30;
			rot[5].z=Mathf.Cos(Time.time*10+90)*30+30;
			if(rot[6].z<25f)rot[6].z=rot[8].z-=Time.deltaTime*20f;
			rot[7].z=rot[9].z=-rot[6].z;
			headCrystal.Add(Time.deltaTime*2);
		}
		if(transform.position.y<-Scaler.sizeY+4){
			if(charges++>2){
				timer=2;
				spawns=0;
				target=GetPlayer();
				update=Spawning;
			}else{
				timer=3;
			}
		}
	}
	void Bite(){
		if(!bitting){
			spawns=0;
			update=Spawning;
			return;
		}
		headCrystal.Add(Time.deltaTime);
		bitting.position=transform.position-Vector3.up*5f;
		rot[4].z=rot[6].z=-15;
		rot[5].z=rot[7].z=-rot[4].z;
		rot[8].z=Mathf.Sin(Time.time*5)*30;
		rot[9].z=-rot[8].z;
		bitting.Rotate(0,0,90*Time.deltaTime);
		if(timer>0)timer-=Time.deltaTime;
		else{
			bitting.GetComponent<Ship>().enabled=true;
			bitting.rotation=Quaternion.identity;
			bitting=null;
			timer=2;
			spawns=0;
			update=Spawning;
		}
	}
	void Spawning(){
		headCrystal.Min(Time.deltaTime);
		rot[6].z=Mathf.PingPong(timer,1)*25f;
		rot[7].z=-rot[6].z;
		rot[8].z=Mathf.PingPong(timer,1)*15f;
		rot[9].z=-rot[8].z;
		rot[10].z=Mathf.PingPong(timer,1)*35f;
		rot[11].z=-rot[10].z;
		back.localScale=new Vector3(0.8f+Mathf.PingPong(timer,1)*0.1f,1);
		crystal.Set(Mathf.PingPong(timer,1));
		if(timer>0)timer-=Time.deltaTime;
		else {
			int i=Random.Range(1,webs.Length-1);
			spawns++;
			Spawn(i);
			Spawn(i-1);
			Spawn(i+1);
		}
		if(spawns>4){
			update=Charge;
			back.localScale=Vector3.one;
			charges=-1;
		}
	}
	void Dying(){
		timer-=Time.deltaTime;
		if(timer<0)Loader.Scene("SelectionTest");
		ParticleManager.Emit(1,transform.position+Random.onUnitSphere,1,5);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		Ship ship=other.GetComponent<Ship>();
		if(ship){
			ship.enabled=false;
			bitting=ship.transform;
			timer=2;
			update=Bite;
		}
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		update?.Invoke();
		for (int i = 0;legs!=null && i < legs.Length; i++)
		{
			legs[i].eulerAngles=rot[i];
		}
	}
	protected override void Die()
	{
		update=Dying;
		timer=3;
		Locks.Boss(0,true);
		EnemySpawner.points[killerid]+=1000;
	}
	void Spawn(int i)
	{
		timer=2;
		GameObject go=new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=spiderling.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		Spiderling ling=go.AddComponent<Spiderling>();
		ling.SetSprites(spiderling);
		ling.MoveTo(webs[i]);
		ling.transform.position=back.position+Vector3.up*3-Vector3.back*0.2f;
	}
	void Shoot(){
		GameObject go=new GameObject("webshot");
		go.transform.position=back.position+Vector3.up*4;
		go.AddComponent<WebShot>().Set(webshot,(target.position-(back.position+Vector3.up*4)).normalized*15+go.transform.position,16,4,3,1,"enemy");
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(update!=Dying && update!=Intro && update!=Building) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(16,col.collider.transform.position,1);
		//speed=hp<350 ? 12 : 8;
	}
}
