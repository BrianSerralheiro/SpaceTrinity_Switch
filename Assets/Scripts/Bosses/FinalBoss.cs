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
	private float timer=0.2f;
	private float time=0;
	private float ap=1f;
	private State prev;
	private bool left;
	private Vector3 vec = Vector3.left;
	private Vector3 mod = Vector3.forward/10;
	private Vector3 pos = new Vector3();
	private Vector3 rot = new Vector3();
	private Vector3 scale = Vector3.one;
	private Vector3 local =new Vector3(0,2f,-0.1f);
	private SpriteRenderer energy;
	private SpriteRenderer zap;
	private Transform final;
	private Sprite[] screens;
	private Sprite screen;
	private SpriteRenderer screenren;
	private SpriteRenderer overlay;
	private float screentimer;

	public static bool last;
	public override void SetSprites(EnemyInfo ei)
	{
		SoundManager.Play(last?2:3);
		BossWarning.Show();
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1600;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		screens=SpriteBase.I.screens;
		GameObject go=new GameObject("screen");
		screenren=go.AddComponent<SpriteRenderer>();
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(0.01f,-0.05f,-0.01f);
		go=new GameObject("energy");
		energy=go.AddComponent<SpriteRenderer>();
		//energy.sprite=SpriteBase.I.zapper[3];
		energy.transform.parent=transform;
		energy.transform.localPosition=local;
		energy.gameObject.SetActive(false);
		go = new GameObject("zap");
		zap=go.AddComponent<SpriteRenderer>();
		//zap.sprite=SpriteBase.I.zapper[1];
		BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size=new Vector2(0.7f,8);
		col.offset=new Vector2(0,2.2f);
		go.transform.localScale=new Vector3(2,2);
		go.transform.parent=energy.transform;
		go.transform.localPosition=new Vector3();
		go.SetActive(false);
		//Screen(1,2.5f);
		if(last)
		{
			go.transform.localScale+=Vector3.up;
			hp=3200;
			local.Set(4.4f,-1.7f,-0.1f);
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
			energy.transform.localScale=Vector3.zero;
			timer=4;
		}
	}
	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="zap") return;
		if(state!=State.intro && state!=State.flee && state!=State.dead) base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(16,col.collider.transform.position,1);
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
				if(state==State.zap)timer=last?2:4;
			}
		}
		else if(state==State.shot)
		{
			if(!last){
				if(player.position.x<transform.position.x) transform.Translate(-Time.deltaTime,0,0);
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
					pos=Vector3.MoveTowards(transform.position,player.position,Time.deltaTime*2);
					pos.z=0;
					transform.position=pos;
				}
				energy.transform.localPosition=local;
				rot.Set(0,0,Mathf.Atan2(energy.transform.position.x-player.position.x,player.position.y-energy.transform.position.y)*Mathf.Rad2Deg);
				energy.transform.eulerAngles=rot;
			}
			else if(timer >0.1f)
			{
				energy.gameObject.SetActive(true);
				//energy.sprite=Bullet.blink ? SpriteBase.I.zapper[3] : SpriteBase.I.zapper[4];
				scale.x=scale.y=1.1f-timer;
				energy.transform.localScale=scale;
				Screen(5,1);
			}
			else if(timer >0)
			{
				zap.gameObject.SetActive(true);
				//energy.sprite=Bullet.blink ? SpriteBase.I.zapper[3] : SpriteBase.I.zapper[4];
				//zap.sprite=Bullet.blink ? SpriteBase.I.zapper[1] : SpriteBase.I.zapper[2];
			}
			else
			{
				timer=last?1:3;
				zap.gameObject.SetActive(false);
				energy.gameObject.SetActive(false);
				local.x*=-1;
				state=State.waiting;
			}
		}
		else if(state==State.flee)
		{
			ParticleManager.Emit(10,(Vector3)Random.insideUnitCircle*2+transform.position,1);
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
			ParticleManager.Emit(10,(Vector3)Random.insideUnitCircle*4+transform.position,1);
			if(timer<0)
			{
				Destroy(gameObject);
				EnemySpawner.boss=false;
			}
		}
	}
	void Shoot()
	{
		SoundManager.PlayEffects(12, 0.1f, 0.5f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[14];
		go.AddComponent<BoxCollider2D>();
		Bullet b = go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=14;
		go.transform.position=transform.position+vec*(left ? 1 : -1)+mod;
		go.transform.up=-transform.up;
		go.transform.localScale=Vector3.one*2f;
		left=!left;
		Screen(5,0.8f);

	}
	void Slash()
	{
		SoundManager.PlayEffects(16, 0.1f, 0.5f);
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[16];
		go.AddComponent<BoxCollider2D>();
		go.AddComponent<Slash>().spriteID=16;
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+local;
		if(last){
			go.transform.up=Vector3.right*local.x/10+Vector3.up;
			local.x*=-1f;
		}
		go.transform.localScale=Vector3.one*(last?3:2);
		Screen(5,1);

	}
	void Bomb()
	{
		GameObject go = new GameObject("enemy");
		//go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bomber[1];
		go.AddComponent<Bomb>();
		go.transform.position=transform.position+vec*(left ? 1 : -1)+mod;
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
