using UnityEngine;

public class Shooter : EnemyBase
{
	private int position;
	private Vector3 finalpoint;
	private float shoottimer;
	int shots;
	private Transform armL;
	private Transform armR;
	private Transform legL;
	private Transform legR;
	private Core crystal;
	private Vector3 vector = new Vector3();
	private Vector3 rot = new Vector3();
	Del movement;
	private static int shootId;
    private int shotCount,cicles;
    private float shotDelay,reloadTime;
	private BulletPath path;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 100;
		explosionID=8;
		GameObject go = new GameObject("legL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		legL=go.transform;
		go=new GameObject("legR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		legR=go.transform;
		go=new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];
		armR=go.transform;
		go=new GameObject("crystal");
		crystal=go.AddComponent<Core>().Set(ei.sprites[5],new Color(0.4f,0f,0.4f));
		crystal.transform.parent=armL.parent=armR.parent=legL.parent=legR.parent=transform;
		armL.localPosition=new Vector3(0.4f,-1f,0.1f);
		armR.localPosition=new Vector3(-0.4f,-1f,0.1f);
		legL.localPosition=new Vector3(0,1.2f,0.1f);
		legR.localPosition=new Vector3(0,1.2f,0.1f);
		crystal.transform.localPosition=new Vector3(0,-1.27f);
		fallSpeed=-9;
		movement=Moving;
		shootId=ei.bulletsID[0];
		PathEnemy pe=(PathEnemy)ei;
		shotCount=pe.shotCount;
		cicles=pe.cicles;
		shotDelay=pe.shotDelay;
		reloadTime=pe.reloadTime;
		path=pe.bulletPath;
	}

	public override void Position(int i)
	{
		base.Position(i);
		position=i;
		finalpoint=new Vector3((i*Scaler.sizeX/20-Scaler.sizeX/2)*0.9f,Scaler.sizeY/2,0);
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		movement();
		armL.localEulerAngles=vector;
		armR.localEulerAngles=-vector;
		legL.localEulerAngles=-vector;
		legR.localEulerAngles=vector;
	}
	void Moving(){
		transform.position=Vector3.MoveTowards(transform.position,finalpoint,4*Time.deltaTime);
		transform.up=transform.position-finalpoint;
		if(finalpoint==transform.position)
		{
			movement=Shooting;
		}
		vector.Set(0,0,Mathf.PingPong(Time.time*80,45f));
	}
	void Shooting(){
		transform.Rotate(Vector3.Cross(Vector3.down,transform.up)*Time.deltaTime*90f);
		shoottimer-=Time.deltaTime;
		if(shoottimer<-shotDelay) 
		{
			if(shots-->0){
				shoottimer=0;
				Shoot();
			}
			else if(cicles-->0){
				shoottimer=reloadTime;
				shots=shotCount;
			}
			else
			movement=SlowFall;
		}
		crystal.Set(Mathf.Lerp(0,1,-shoottimer/shotDelay));
		vector.Set(0,0,Mathf.PingPong(Time.time*67.5f,45f));
	}
	protected override void SlowFall(){
		base.SlowFall();
		transform.Rotate(Vector3.Cross(Vector3.down,transform.up)*Time.deltaTime*190f);
		crystal.Min(Time.deltaTime);
		vector.Set(0,0,Mathf.PingPong(Time.time*200,45f));
	}
	void Shoot()
	{
		SoundManager.PlayEffects(12, 0.5f, 0.8f);
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<BoxCollider2D>();
		PathBullet bu=go.AddComponent<PathBullet>();
		bu.owner=transform.name;
		bu.spriteID=shootId;
		bu.path=path;
		go.transform.position=crystal.transform.position;
		go.transform.up=-transform.up;
	}
}
