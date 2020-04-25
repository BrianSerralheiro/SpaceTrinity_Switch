using UnityEngine;

public class Gun : MonoBehaviour {
	[SerializeField]
	protected int level;
	[SerializeField]
	protected Sprite[] shots;
	protected int shotId;
	[SerializeField]
	protected int damage=1;
	[SerializeField]
	public float bulletSpeed=12.5f,maxSpeed;
	[SerializeField]
	protected bool pierce;

	[SerializeField]
	protected float fireRate = 0.5f;

	protected float shotTimer;

	[SerializeField]
	protected ParticleSystem shotParticle;
	protected int particleID;

	public bool minusPower;
	public virtual void Load(int i)
	{
		if(Ship.skinID[i]>=0 && Locks.Skin(i*3+Ship.skinID[i])){
			shotId=Bullet.Register(shots[(Ship.skinID[i]+1)*2]);
			Bullet.Register(shots[(Ship.skinID[i]+1)*2+1]);
		}
		else {
			shotId=Bullet.Register(shots[0]);
			Bullet.Register(shots[1]);
		}
		particleID=ParticleManager.Register(shotParticle);
		shotParticle=null;
		shots=null;
	}
	public virtual void Shoot()
	{
		if(!gameObject.activeSelf || shotTimer > 0)return;
		shotTimer = fireRate;		
		// muzzle flash ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId+(Bullet.blink ? 0 : 1)];
		go.AddComponent<BoxCollider2D>();
		Bullet bull= go.AddComponent<Bullet>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=pierce;
		bull.particleID=particleID;
		bull.spriteID=shotId;
		bull.bulletSpeed=bulletSpeed;
		bull.maxSpeed=maxSpeed;	
		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
	}
	public virtual void Level(int i)
	{
		shotTimer=0;
		if(minusPower && i==1)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(level<=i);
		}
	}

	protected virtual void Update()
	{
		if(shotTimer > 0)
		{
			shotTimer -= Time.deltaTime;
		}
	}
}
