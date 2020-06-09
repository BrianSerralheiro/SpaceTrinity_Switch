using UnityEngine;

public class Bomber : EnemyBase {
	private float bombtime,shottime;
	static Vector3 local=new Vector3(-4.5f,0,-0.5f);
	private static Sprite bomb;
	static int explosionID,spawnID,shotID,trailID,impactID;
	static GameObject trail;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		name+="big";
		hp=150;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		bombtime=Time.time+4;
		explosionID=ei.particleID[0];
		spawnID=ei.particleID[1];
		trailID=ei.particleID[3];
		impactID=ei.particleID[4];
		shotID=ei.bulletsID[0];
		if(!trail)trail=ei.particles[2].gameObject;
		bomb=ei.sprites[1];
	}

	public override void Position(int i)
	{
		transform.position=new Vector3(-Scaler.sizeX/2,Scaler.sizeY/20*i,0.1f);
		// base.Position(i);
		// transform.Rotate(0,0,-90f);
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		transform.Translate(Time.deltaTime,0,0);
		if(bombtime<Time.time)Bomb();
		float y=GetPlayer().position.y;
		if(transform.position.y>y-3 && transform.position.y<y+2)Shot();
		if(transform.position.x>Scaler.sizeX/2f+4.2F || transform.position.y<-Scaler.sizeY-8.4F) Die();

	}
	void Shot(){
		if(Time.time<shottime)return;
		shottime=Time.time+0.5f;
		GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position+transform.right*0.6f+Vector3.up*1.5f;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=10;
        bu.spriteID=shotID;
        bu.particleID=trailID;
        bu.impactID=impactID;
		bu.Timer(10);
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
        go.AddComponent<CircleCollider2D>().radius=0.3f;
        go.transform.up=transform.right;
	}
	void Bomb()
	{
		bombtime=Time.time+0.8f;
		GameObject go = new GameObject("enemy");
		Instantiate(trail,go.transform);
		go.AddComponent<SpriteRenderer>().sprite=bomb;
		go.transform.position=transform.position+local;
		ParticleManager.Emit(spawnID,go.transform.position,1);
		GameObject ex=new GameObject("enemy");
		ex.SetActive(false);
		ex.AddComponent<BoxCollider2D>().size=new Vector2(1f,8f);
		ex.AddComponent<BoxCollider2D>().size=new Vector2(8f,1f);
		go.AddComponent<Bomb>().Set(5,90,explosionID,new Vector3(go.transform.position.x,-Scaler.sizeY/2),2,8,ex);
	}

	protected override void Die()
	{
		killed=true;
		base.Die();
		if(hp<=0)ParticleManager.Emit(1,transform.position-transform.right*3,3,1);
	}
}