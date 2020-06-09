using UnityEngine;

public class FinalBoss : EnemyBase {
	enum State
	{
		intro=0,
		waiting=1,
		shot=2,
		slash=3,
		bomb=4,	
		zap=5,
		flee=6,
		dead
	}
	State state;
	private float timer=0.2f,time=0,ap=1f;
	private State prev;
	private bool left;
	private Vector3 vec = Vector3.left,mod = Vector3.forward/10,pos = new Vector3();
	private Vector3 local =new Vector3(0,2f,-0.1f);
	private Transform final;
	private Sprite[] screens;
	private Sprite screen,bomb;
	private SpriteRenderer screenren,overlay;
	private float screentimer;
	Transform target;
	public static bool last;
	int shotID,slashID,trailID,impactID,spawnID,explosionID;
	GameObject zap,sparkles1,sparkles2;
	new BoxCollider2D collider2D;
	public override void SetSprites(EnemyInfo ei)
	{
		// SoundManager.Play(last?2:3);
		name+="Boss";
		damageEffect = true;
		EnemySpawner.boss=true;
		shotID=ei.bulletsID[0];
		slashID=ei.bulletsID[2];
		trailID=ei.particleID[0];
		impactID=ei.particleID[1];
		sparkles1=ei.particles[2].gameObject;
		spawnID=ei.particleID[4];
		sparkles2=ei.particles[5].gameObject;
		explosionID=ei.particleID[6];
		bomb=ei.sprites[4];
		hp=1600;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		screens=SpriteBase.I.screens;
		GameObject go=new GameObject("screen");
		screenren=go.AddComponent<SpriteRenderer>();
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0.01f,-0.05f,-0.01f);
		go=Instantiate(ei.particles[3].gameObject,transform);
		zap=go;
		collider2D = go.AddComponent<BoxCollider2D>();
		collider2D.size=new Vector2(1f,17);
		collider2D.offset=new Vector2(0,8);
		collider2D.enabled=false;
		go.transform.localScale=new Vector3(2,2,2);
		go.transform.localPosition=new Vector3(0,4);
		//Screen(1,2.5f);
		if(last)
		{
			go.transform.localScale+=Vector3.up;
			hp=3200;
			local.Set(0,-2.8f,-0.1f);
			screenren.sprite=screens[4];
			screen=null;
			go = new GameObject("enemy");
			go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
			BoxCollider2D c = go.AddComponent<BoxCollider2D>();
			c.size=new Vector2(11,2);
			c.offset=new Vector2(0,-1f);
			final=go.transform;
			final.position=new Vector3(0,Scaler.sizeY+5,0.1f);
			go=new GameObject("overlay");
			overlay=go.AddComponent<SpriteRenderer>();
			overlay.sprite=ei.sprites[2];
			go.transform.parent=final;
			go.transform.localPosition=new Vector3(0,0,-0.12f);
			go=new GameObject("fill");
			go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
			go.transform.parent=final;
			go.transform.localPosition=new Vector3(0,-1.3f);
			timer=4;
		}
	}
	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider==collider2D) return;
		if(state!=State.intro && state!=State.flee && state!=State.dead) base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(3,col.collider.transform.position,1);
		if(damageTimer>0)Screen(1,0.5f);
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+4,0);
	}
	protected override void Die()
	{
		EnemySpawner.points[killerid]+=1000;
		if(last)
		{
			state=State.dead;
			timer=5;
			return;
		}
		//last=true;
		state=State.flee;
		timer=5;
		
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		timer-=Time.deltaTime;
		if(last)overlay.color=screenren.color=_renderer.color;
		else
		if(screen!=null)
		{
			screenren.sprite=screen;
			screentimer-=Time.deltaTime;
			if(screentimer<=0.1f)screenren.sprite=screens[0];
			if(screentimer<=0)screenren.sprite=screen=null;
		}
		if(state==State.intro)
		{
			if(last)
			{
				if(final.position.y==Scaler.sizeY+5 && timer>0)
				{
					transform.Translate(0,-Time.deltaTime*2,0);
					screenren.sprite=screens[5];
				}
				else
				{
					if(final.position.y>Scaler.sizeY)
					{
						if(final.position.y==Scaler.sizeY+5)
						SoundManager.PlayEffects(19);
						final.Translate(0,-Time.deltaTime,0);
						if(timer<=0)
						{
							timer=0.2f;
							screenren.sprite=screenren.sprite==screens[3] ? screens[2] : screens[3];
						}

					}
					else
					{
						final.Translate(0,Scaler.sizeY-final.position.y,0);
						if(timer<=0)
						{
							timer=0.2f;
							screenren.sprite=screenren.sprite==screens[3] ? screens[2] : screens[3];
						}
						if(transform.position.y<Scaler.sizeY) transform.Translate(0,Time.deltaTime,0);
						else
						{
							transform.position=final.position;
							transform.Translate(0,0,-transform.position.z);
							final.parent=transform;
							screenren=_renderer;
							_renderer=final.GetComponent<SpriteRenderer>();
							state=State.waiting;
							timer=2;
						}
					}
				}
			}
			else{
			transform.Translate(0,-Time.deltaTime,0);
			if(timer<=0)
			{
				timer=0.2f;
				screenren.sprite=screenren.sprite==screens[3] ? screens[2] : screens[3];
			}
			if(transform.position.y<Scaler.sizeY/2){
				state=State.waiting;
				timer=1;
				Screen(0,0.1f);
			}
			}
		}
		else if(state==State.waiting)
		{
			if(!last){
				if(transform.position.x<-0.1f)transform.Translate(Time.deltaTime*2,0,0);
				else if(transform.position.x>0.1f)transform.Translate(-Time.deltaTime*2,0,0);
				if(transform.position.y<Scaler.sizeY/2) transform.Translate(0,Time.deltaTime*2,0);
			}
			if(timer<=0)
			{
				do
					state=(State)Random.Range(2,6);
				while(state==prev);
				prev=state;
				target=GetPlayer();
				if(state==State.zap)timer=last?2:4;
			}
		}
		else if(state==State.shot)
		{
			if(!last){
				if(target.position.x<transform.position.x) transform.Translate(-Time.deltaTime,0,0);
				else transform.Translate(Time.deltaTime,0,0);
			}
			if(timer<=0)
			{
				Shoot();
				timer=last?0.1f:0.5f;
				ap-=0.1f;
			}
			if(ap<=0)
			{
				state=State.waiting;
				ap=1;
			}
		}
		else if(state==State.slash)
		{
			if(timer<=0)
			{
				Slash();
				timer=1f;
				ap-=last?0.3f:0.4f;
			}
			if(ap<=0)
			{
				state=State.waiting;
				ap=1;
			}
		}
		else if(state==State.bomb)
		{
			if(!last){
				transform.Translate(Mathf.Cos(time)*Time.deltaTime*2,0,0);
				time+=Time.deltaTime;
			}
			if(timer<=0)
			{
				Bomb();
				timer=last?1:2f;
				ap-=last?0.1f:0.3f;
			}
			if(ap<=0)
			{
				state=State.waiting;
				ap=1;
				time=0;
				timer=last?1:3;
			}
		}
		else if(state==State.zap)
		{
			if(timer>1)
			{
				if(!last){
					pos=Vector3.MoveTowards(transform.position,target.position,Time.deltaTime*2);
					pos.z=0;
					transform.position=pos;
				}
				else
					zap.transform.localPosition=local+vec*7.5f*(left?-1:1);
			}
			else if(timer >0.1f)
			{
				if(!zap.activeSelf){
					// zap.transform.rotation=Quaternion.identity;
					Vector3 v=target.position-zap.transform.position;
					v.z=0;
					left=!left;
					zap.transform.up=v;
				}
				zap.SetActive(true);
				Screen(5,1);
			}
			else if(timer >0)
			{
				collider2D.enabled=true;
			}
			else
			{
				timer=last?1:3;
				collider2D.enabled=false;
				state=State.waiting;
			}
		}
		else if(state==State.flee)
		{
			ParticleManager.Emit(1,transform.position,1,2);
			screenren.sprite=screens[4];
			transform.Translate(Random.value*2*Time.deltaTime,Random.value*3*Time.deltaTime,0);
			if(transform.position.y>Scaler.sizeY+2 || transform.position.x>Scaler.sizeX+3)
			{
				Destroy(gameObject);
				last=true;
				EnemySpawner.boss=false;
			}
		}
		else if(state==State.dead)
		{
			ParticleManager.Emit(1,(Vector3)Random.insideUnitCircle*4+transform.position,1);
			if(timer<0)
			{
				Destroy(gameObject);
				EnemySpawner.boss=false;
			}
		}
	}
	void Shoot()
	{
		// SoundManager.PlayEffects(12, 0.1f, 0.5f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
		go.AddComponent<CircleCollider2D>().radius=0.3f;
		Bullet b = go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shotID;
		b.particleID=trailID;
		b.impactID=impactID;
		go.transform.position=transform.position+vec*2*(left ? 1 : -1)+mod+Vector3.down*3.5f;
		go.transform.up=-transform.up;
		go.transform.localScale=Vector3.one*2f;
		left=!left;
		Screen(5,0.8f);

	}
	void Slash()
	{
		// SoundManager.PlayEffects(16, 0.1f, 0.5f);
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[slashID];
		go.AddComponent<BoxCollider2D>();
		Instantiate(sparkles1,go.transform);
		Slash s=go.AddComponent<Slash>();
		s.spriteID=slashID;
		s.impactID=impactID;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+local;
		if(last){
			go.transform.position=transform.position+local+vec*(left?-7.5f:7.5f);
			go.transform.up=go.transform.position-Vector3.down*Scaler.sizeY/2;
		}
		go.transform.localScale=Vector3.one*(last?3:2);
		left=!left;
		Screen(5,1);

	}
	void Bomb()
	{
		GameObject go = new GameObject("enemy");
		Instantiate(sparkles2,go.transform);
		go.AddComponent<SpriteRenderer>().sprite=bomb;
		go.transform.position=transform.position+vec*2*(left?1:-1)+mod+Vector3.down*3.5f;
		ParticleManager.Emit(spawnID,go.transform.position-mod,1);
		GameObject ex=new GameObject("enemy");
		ex.SetActive(false);
		// go.transform.rotation=transform.rotation;
		ex.AddComponent<BoxCollider2D>().size=new Vector2(1f,8f);
		ex.AddComponent<BoxCollider2D>().size=new Vector2(8f,1f);
		go.AddComponent<Bomb>().Set(50,90,explosionID,GetPlayer().position,2,8,ex);
		left=!left;
		Screen(5,1);

	}
	private void Screen(int i,float f)
	{
		if(screentimer>0 || last)return;
		screen=screens[i];
		screentimer=f;
		screenren.sprite=screen;
	}
}
