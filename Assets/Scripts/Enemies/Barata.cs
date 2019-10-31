using UnityEngine;

public class Barata : EnemyBase {

	private Transform wingL;
	private Transform wingR;
	private float speed=4;
	private Vector3 dir=Vector3.right+Vector3.up;
	private Transform[] legs;
	private Core crystal;
	private Vector3 rot = new Vector3();
    Del update;
	enum State
	{
		intro,
		moving,
		charging,
		dead
	}
	State state;
	private Vector3 vector = new Vector3();
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
		crystal.transform.localPosition=new Vector3(0,0.1f);
        update=Intro;
	}
    void Intro(){
        transform.Translate(0,-Time.deltaTime*2,0);
		if(transform.position.y<0)update=null;
    }
    void Charge1(){
        if(vector.z>45)transform.Translate(0,Time.deltaTime*speed,0);
		crystal.Add(Time.deltaTime);
		if(transform.position.y>Scaler.sizeY){
	    	state=State.moving;
			float f=Random.value*(hp<200?4:2);
			dir.y=Mathf.Max(f,2f-f);
			dir.x=2f-dir.y;
		}
    }
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
        update?.Invoke();
		if(state==State.moving)
		{
			if(transform.position.y>Scaler.sizeY-4)dir.y=-Mathf.Abs(dir.y);
			if(transform.position.x>Scaler.sizeX/2f-2)dir.x=-Mathf.Abs(dir.x);
			if(transform.position.x<-Scaler.sizeX/2f+2)dir.x=Mathf.Abs(dir.x);
			transform.Translate(dir*Time.deltaTime*(speed/4));
			crystal.Min(Time.deltaTime);
			rot.Set(0,0,Mathf.PingPong(Time.time*50,45f));
			if(transform.position.y<-Scaler.sizeY+2)
			{
				state=State.charging;
				SoundManager.PlayEffects(16);
				rot.Set(0,0,0);
			}
		}
		else if(state==State.charging)
		{
			if(vector.z>45)transform.Translate(0,Time.deltaTime*speed,0);
			crystal.Add(Time.deltaTime);
			if(transform.position.y>Scaler.sizeY){
				state=State.moving;
				float f=Random.value*(hp<200?4:2);
				dir.y=Mathf.Max(f,2f-f);
				dir.x=2f-dir.y;
			}
		}
		else if(state==State.dead)
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
		if(state==State.charging && vector.z<45)vector.z+=Time.deltaTime*90;
		if(state!=State.charging && vector.z>0)vector.z-=Time.deltaTime*20;
		if(state!=State.dead){
			wingL.localEulerAngles=vector;
			wingR.localEulerAngles=-vector;
			
			legs[0].localEulerAngles=vector/4;
			legs[1].localEulerAngles=-vector/4;
			legs[2].localEulerAngles=vector;
			legs[3].localEulerAngles=-vector;
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
		EnemySpawner.points+=1000;
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(vector.z>5 && state!=State.dead) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(16,col.collider.transform.position,1);
		speed=hp<350 ? 12 : 8;
	}
}
