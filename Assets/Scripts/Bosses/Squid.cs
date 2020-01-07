using UnityEngine;

public class Squid : EnemyBase {
	public EnemyInfo info;
	Transform[] tentacle;
	public override void SetSprites(EnemyInfo ei)
	{
		//BossWarning.Show();
		name+="Boss";
		//SoundManager.Play(2);
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1200;
		//if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		GameObject go;
		tentacle=new Transform[8];
		for (int i = 0; i < tentacle.Length; i++)
		{
			go=new GameObject("tentacle"+i);
			go.AddComponent<SpriteRenderer>().sprite=i==tentacle.Length-1?ei.sprites[2]:ei.sprites[1];
			tentacle[i]=go.transform;
			if(i>0){
				tentacle[i].parent=tentacle[i-1];
				tentacle[i].localPosition=new Vector3(-0.7f,-1.2f,0);
			}
			else {
				tentacle[i].parent=transform;
				tentacle[i].localPosition=new Vector3(-1,-5,0.1f);
			}
		}
	}
	void Start()
	{
		SetSprites(info);
	}
	protected new void Update(){
		if(Ship.paused) return;
		base.Update();
		for (int i = 1; i < tentacle.Length; i++)
		{
			tentacle[i].localEulerAngles=new Vector3(0,0,Mathf.Cos(Time.time+i*-45)*15);
		}
	}
	private void Round(Vector3 v)
	{
		GameObject go = new GameObject("enemy");
	//	go.AddComponent<SpriteRenderer>().sprite=round;
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.AddComponent<Round>();
		go.transform.position=v;
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
		if(true) base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(16,col.collider.transform.position,1);
	}
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
}
