using UnityEngine;

public class Batzilla : EnemyBase {
	private Transform body;
	private Transform head;
	private Transform wingL;
	private Transform wingR;
	private SpriteRenderer henderer;
	private Core slash;
	private Core dark;
	private BoxCollider2D slashcod;
	private Vector3 slashscl=new Vector3(5000,0,0);
	private Vector3 slashrot=new Vector3(0,0,0);
	private float timer=1.5f;
	private float time=0;
	private Vector3 left=new Vector3(-0.64f,0.24f,-0.2f);
	private Vector3 right=new Vector3(0.64f,0.24f,-0.2f);
	private EnemyInfo bat;
	private int shotId;
	enum State
	{
		intro,
		wating,
		shooting,
		calling,
		slashing,
		dead
	}
	State state;
	public override void SetSprites(EnemyInfo ei)
	{
		BossWarning.Show();
		SoundManager.Play(2);
		name+="Boss";
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1800;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		GameObject go=new GameObject("body");
		_renderer=go.AddComponent<SpriteRenderer>();
		_renderer.sprite=ei.sprites[1];
		body=go.transform;
		go=new GameObject("head");
		henderer=go.AddComponent<SpriteRenderer>();
		henderer.sprite=ei.sprites[2];
		head=go.transform;
		body.parent=head.parent=transform;
		go=new GameObject("eyes");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[5];
		go.transform.parent=head;
		body.parent=head.parent=transform;
		go.transform.localPosition=new Vector3(0,-0.29f,-0.1f);
		go=new GameObject("wingL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		go.transform.parent=transform;
		wingL=go.transform;
		go=new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];
		go.transform.parent=transform;
		wingR=go.transform;
		wingL.localPosition=new Vector3(-0.95f,1);
		wingR.localPosition=new Vector3(0.95f,1);
		body.localPosition=new Vector3(0,-0.9f,-0.01f);
		head.localPosition=new Vector3(0,1.5f,-0.02f);

		go=new GameObject("slash");
		slash=go.AddComponent<Core>().Set(Sprite.Create(new Texture2D(1,1),new Rect(0,0,1,1),new Vector2(0.5f,0.5f)),new Color(0.6f,0f,0.1f));
		slash.transform.localScale=slashscl;
		slash.Set(1);
		slashcod=go.AddComponent<BoxCollider2D>();
		slashcod.enabled=false;

		go=new GameObject("dark");
		Texture2D t=new Texture2D(1,1);
		t.SetPixels(new Color[]{Color.black});
		t.Apply(false);
		dark=go.AddComponent<Core>().Set(Sprite.Create(t,new Rect(0,0,1,1),new Vector2(0.5f,0.5f)),new Color(0f,0f,0f,0f));
		dark.white=new Color(0f,0f,0f,1f);
		go.transform.localScale=new Vector3(5000,5000);
		go.transform.position=new Vector3(0,0,-0.1f);
		bat=(ei as CarrierInfo).spawnable;
		shotId=ei.bulletsID[0];
	}
	
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		if(head){
			henderer.color=_renderer.color;
		}
		timer-=Time.deltaTime;
		if(state==State.intro)
		{
			transform.Translate(0,-Time.deltaTime,0);
			dark.Add(Time.deltaTime);
			if(transform.position.y<Scaler.sizeY/2f){
				state=State.slashing;
				timer=1.5f;
				slash.transform.position=GetPlayer().position;
				slashrot.z=Random.Range(-45f,45f);
				slash.transform.eulerAngles=slashrot;
			}
		}
		else if(state==State.wating)
		{
			time+=Time.deltaTime;
			transform.Translate(Mathf.Cos(time)*Time.deltaTime*2,0,0);
			head.Translate(0,Mathf.Cos(time/2)*Time.deltaTime/10,0);
			wingR.Translate(0,Mathf.Cos(time*5)*Time.deltaTime/5,0);
			wingL.Translate(0,Mathf.Cos(time*5)*Time.deltaTime/5,0);
			if(timer<=0)
			{
				timer=1.5f;
				float f=Random.value;
				if(f>0.5f)state=State.shooting;
				else if(f>0.2f)state=State.calling;
				else {
					state=State.slashing;
					slash.transform.position=GetPlayer().position;
					slashrot.z=Random.Range(-45f,45f);
					if(slashrot.z>0)wingR.eulerAngles=Vector3.forward*45f;
					else wingL.eulerAngles=Vector3.forward*-45f;
					slash.transform.eulerAngles=slashrot;
				}
			}
		}
		else if(state==State.shooting)
		{
			state=State.wating;
			timer=1;
			Shoot(left);
			Shoot(right);
		}
		else if(state==State.slashing)
		{
			if(timer>1f)
			{
				//slashcol.a=(1.5f-timer)*2;
				dark.Add(Time.deltaTime*5);
			}
			else if(timer>0.5f)
			{
				SoundManager.PlayEffects(16, 5, 2);
				slashscl.y=(1-timer)*200;
			}
			else if(timer>0)
			{
				slash.Min(Time.deltaTime*10);
				slashcod.enabled=true;
			}
			else
			{
				state=State.wating;
				timer=1;
				slash.Set(1);
				dark.Set(0);
				slashscl.y=0;
				slashcod.enabled=false;
				wingL.rotation=wingR.rotation=Quaternion.identity;
			}
			slash.transform.localScale=slashscl;
		}
		else if(state==State.calling)
		{
			state=State.wating;
			Bat();
			timer=1;
		}
		else if(state==State.dead)
		{
			if(timer>0)
			{
				ParticleManager.Emit(1,(Vector3)Random.insideUnitCircle*1.5f+transform.position,1);
			}
			else
			{
				if(body){
					Destroy(body.gameObject);
					Destroy(wingL.gameObject);
					Destroy(wingR.gameObject);
				}
				if(timer<-0.2f){
					ParticleManager.Emit(1,(Vector3)Random.insideUnitCircle*1.5f+transform.position,1);
					timer=0;
				}
				transform.Translate(0,-Time.deltaTime*4,0,Space.World);
				transform.Rotate(0,0,Time.deltaTime*4);
				if(transform.position.y<-Scaler.sizeY-2)
				{
					SoundManager.Play(7);
					Destroy(gameObject);
					EnemySpawner.boss=false;
				}
			}
		}
	}
	protected override void Die()
	{
		for(int i = 0; i<10; i++)
		{
			ParticleManager.Emit(9,(Vector3)Random.insideUnitCircle*1.5f+head.transform.position,1);
		}
		Locks.Boss(5,true);
		Destroy(head.gameObject);
		Destroy(slash.gameObject);
		Destroy(dark.gameObject);
		state=State.dead;
		EnemySpawner.points[killerid]+=1000;
		timer=1;
		GetComponent<BoxCollider2D>().enabled = false;
	}
	void Shoot(Vector3 v)
	{
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[12];
		go.AddComponent<BoxCollider2D>();
		Bullet b=go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shotId;
		go.transform.position=transform.position+v;
		go.transform.up=-transform.up;
		//go.transform.localScale=Vector3.one*2;
	}

	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(state!=State.slashing && state!=State.dead && state!=State.intro) base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(16,col.collider.transform.position,1);
	}
	void Bat()
	{
		GameObject go = new GameObject("enemy");
		Bat b=go.AddComponent<Bat>();
		b.target=head.position+(GetPlayer().position-head.position)*5f;
		b.SetSprites(bat);
		go.AddComponent<SpriteRenderer>().sprite=bat.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=head.position;
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+4,0);
	}
}
