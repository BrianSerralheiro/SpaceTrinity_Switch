using UnityEngine;

public class Squid : EnemyBase {
	public EnemyInfo info;
	Transform[,] tentacle;
	Transform[] claws,lids;
	SquidSille[] missiles=new SquidSille[3];
	int side,slot;
	static readonly Vector3 offsetX=new Vector3(-3.55f,0),offsetY=new Vector3(0,0.35f,-0.1f);
	static readonly int tentacleSize=8;
	LineRenderer line;
	float timer=2;
	new BoxCollider2D collider;
	Vector3[] vectors;
	EnemyInfo squidsille;
	Del update;
	public override void SetSprites(EnemyInfo ei)
	{
		BossWarning.Show();
		name+="Boss";
		SoundManager.Play(2);
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1200;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		GameObject go;
		SpriteRenderer sr;
		tentacle=new Transform[2,tentacleSize];
		claws=new Transform[2];
		lids=new Transform[2];
		for (int i = 0; i < tentacleSize; i++)
		{
			go=new GameObject("tentacleL"+i);
			go.AddComponent<SpriteRenderer>().sprite=i==tentacleSize-1?ei.sprites[2]:ei.sprites[1];
			tentacle[0,i]=go.transform;
			if(i>0){
				tentacle[0,i].parent=tentacle[0,i-1];
				tentacle[0,i].localPosition=new Vector3(-0.7f,-1.2f,0);
				go.transform.localEulerAngles=new Vector3(0,0,360/tentacleSize);
			}
			else {
				tentacle[0,i].parent=transform;
				tentacle[0,i].localPosition=new Vector3(-1,-5,0.1f);
			}
			go=new GameObject("tentacleR"+i);
			sr= go.AddComponent<SpriteRenderer>();
			sr.sprite=i==tentacleSize-1?ei.sprites[2]:ei.sprites[1];
			sr.flipX=true;
			tentacle[1,i]=go.transform;
			if(i>0){
				tentacle[1,i].parent=tentacle[1,i-1];
				tentacle[1,i].localPosition=new Vector3(0.7f,-1.2f,0);
				go.transform.localEulerAngles=new Vector3(0,0,-360/tentacleSize);
			}
			else {
				tentacle[1,i].parent=transform;
				tentacle[1,i].localPosition=new Vector3(1,-5,0.1f);
			}
		}
		go=new GameObject("clawL");
		claws[0]=go.transform;
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(-2,-4,0.05f);
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];

		go=new GameObject("lidL");
		go.transform.parent=claws[0];
		lids[0]=go.transform;
		go.transform.localPosition=offsetX+offsetY;
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];

		go=new GameObject("clawR");
		claws[1]=go.transform;
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3(2,-4,0.05f);
		sr=go.AddComponent<SpriteRenderer>();
		sr.sprite=ei.sprites[3];
		sr.flipX=true;

		go=new GameObject("lidR");
		go.transform.parent=claws[1];
		lids[1]=go.transform;
		go.transform.localPosition=-offsetX+offsetY;
		sr=go.AddComponent<SpriteRenderer>();
		sr.sprite=ei.sprites[4];
		sr.flipX=true;
		go=new GameObject("enemylaserBoss");
		go.transform.parent=transform;
		line=go.AddComponent<LineRenderer>();
		line.positionCount=10;
		line.useWorldSpace=false;
		Gradient gradient=new Gradient();
		gradient.SetKeys(new GradientColorKey[]{new GradientColorKey(new Color(1,0.6f,0),0)},new GradientAlphaKey[]{new GradientAlphaKey(0f,0),new GradientAlphaKey(1,0.1f)});
		line.colorGradient=gradient;
		for (int i = 0; i < line.positionCount; i++)
		{
			line.SetPosition(i,new Vector3(0,-i*2-1,-0.1f));
		}
		vectors=new Vector3[line.positionCount];
		line.GetPositions(vectors);
		line.material=new Material(Shader.Find("Sprites/Default"));
		collider=go.AddComponent<BoxCollider2D>();
		line.enabled=collider.enabled=false;
		update=Intro;
		squidsille=((CarrierInfo)ei).spawnable;
	}
	void Intro(){
		transform.Translate(0,-Time.deltaTime,0);
		if(transform.position.y<Scaler.sizeY/2)update=Wait;
	}
	protected new void Update(){
		if(Ship.paused) return;
		base.Update();
		update?.Invoke();
		
	}
	void Wait(){
		timer-=Time.deltaTime;
		lids[side].localPosition=Vector3.MoveTowards(lids[side].localPosition,(side==0?1:-1)*offsetX+offsetY,Time.deltaTime);
		for (int i = 1; i < tentacleSize; i++)
		{
			tentacle[0,i].rotation=Quaternion.RotateTowards(tentacle[0,i].rotation,Quaternion.Euler(0,0,Mathf.Cos(Time.time+i*-45)*15),Time.deltaTime*30);
			tentacle[1,i].rotation=Quaternion.RotateTowards(tentacle[1,i].rotation,Quaternion.Euler(0,0,Mathf.Cos(Time.time+i*-45)*-15),Time.deltaTime*30);
		}
		if(timer<0){
			switch(Random.Range(0,3)){
				case 0:
					Debug.Log("Punch");
					break;
				case 1:
					update=Laser;
					timer=5;
					collider.enabled=line.enabled=true;
					line.widthMultiplier=0;
					break;
				case 2:
					side=Random.Range(0,2);
					Load();
					update=Missile;
					timer=2;
					break;
			}
			
		}
	}
	void Laser(){
		timer-=Time.deltaTime;
		for (int i = 1; i < tentacleSize; i++)
		{
			tentacle[0,i].localRotation=Quaternion.RotateTowards(tentacle[0,i].localRotation,Quaternion.Euler(0,0,-45),Time.deltaTime*90);
			tentacle[1,i].localRotation=Quaternion.RotateTowards(tentacle[1,i].localRotation,Quaternion.Euler(0,0,45),Time.deltaTime*90);
		}
		collider.enabled=!collider.enabled;
		float f=4.5f/line.positionCount;
		float f1=40f;
		float t=Time.time*10;
		float t1=Time.time*40;
		for (int i = 1; i < vectors.Length; i++)
		{
			vectors[i].x=(Mathf.Sin(t+f*i)*0.6f+Mathf.Cos(t1-f1*i)*0.4f)*Mathf.Min(5-timer,2);
		}
		line.SetPositions(vectors);
		line.widthMultiplier=Mathf.MoveTowards(line.widthMultiplier,1,Time.deltaTime/2);
		collider.size=line.bounds.size;
		collider.offset=line.bounds.center;
		if(timer<0){
			timer=3;
			update=Wait;
			collider.enabled=line.enabled=false;
		}
	}
	void Missile(){
		timer-=Time.deltaTime;
		lids[side].localPosition=Vector3.MoveTowards(lids[side].localPosition,(side==0?1:-1)*offsetX*0.8f+offsetY,Time.deltaTime);
		if(timer<0){
			if(slot<missiles.Length){
				if(missiles[slot]){
					missiles[slot].enabled=true;
					missiles[slot].target=player.position;
				}
				slot++;
				timer=1;
			}
			else{
				timer=2;
				update=Wait;
			}
		}
	}
	void Load(){
		slot=0;
		for (int i = 0; i < missiles.Length; i++)
		{
			GameObject go=new GameObject("enemy");
			missiles[i]=go.AddComponent<SquidSille>();
			go.AddComponent<SpriteRenderer>().sprite=squidsille.sprites[0];
			go.AddComponent<BoxCollider2D>();
			go.transform.position=claws[side].position+(side==0?1:-1)*offsetX+offsetY*((i-1)*2)+Vector3.forward*0.5f;
			Vector3 v=go.transform.position-claws[side].position;
			v.z=0;
			go.transform.up=v.normalized;
			missiles[i].enabled=false;
			missiles[i].SetSprites(squidsille);
		}
	}
	void Shoot(int i)
	{
		// GameObject go = new GameObject("enemybullet");
		// go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
		// go.AddComponent<CircleCollider2D>();
		// Bullet bu=go.AddComponent<Bullet>();
		// bu.owner="enemy";
		// bu.spriteID=shotId;
		// go.transform.position=eyes.transform.position+pos[i]+Vector3.back*0.5f;
		// go.transform.up=(pos[i]+Vector3.down).normalized;
	}
	protected override void Die()
	{
		EnemySpawner.points[killerid]+=1000;
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name.Contains("enemylaser"))return;
		if(true) base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(16,col.collider.transform.position,1);
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
}
