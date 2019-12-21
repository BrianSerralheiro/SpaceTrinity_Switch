using UnityEngine;

public class GravGun : Gun
{
    private int count=14;
    [SerializeField]
    private int damageBySize,damageByLevel;
    [SerializeField]
    private Material material;
    private int finalDamage;
    private float timer;
    private ParticleSystem particles;
    public override void Load(int i)
	{
        finalDamage=damage;
		if(Ship.skinID[i]>=0 && Locks.Skin(i*3+Ship.skinID[i])){
            shotId=Bullet.Register(shots[(Ship.skinID[i]+1)*count]);
            for(int c=1;c<count;c++){
                Bullet.Register(shots[(Ship.skinID[i]+1)*count+c]);
            }
        }else{
            shotId=Bullet.Register(shots[0]);
            for(int c=1;c<count;c++){
                Bullet.Register(shots[c]);
            }
        }
        particles=GetComponent<ParticleSystem>();
		shots=null;
	}
	public override void Shoot()
	{ 
        if(!gameObject.activeSelf || shotTimer > 0)return;
        shotTimer = fireRate;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		SpriteRenderer sp= go.AddComponent<SpriteRenderer>();
        sp.sprite=Bullet.sprites[shotId];
        sp.material=material;
		go.AddComponent<BoxCollider2D>();
		GravBullet bull= go.AddComponent<GravBullet>();
		bull.owner=transform.parent.name;
		bull.damage=finalDamage;
		bull.sizeDamage=damageBySize;
		bull.particleID=particleID;
		bull.spriteID=shotId;
        bull.bulleSpeed=bulletSpeed;
        bull.Size((int)(timer/(level/2f)));

		go.transform.position=transform.position;
        go.transform.localScale=Vector3.one*(1+(3-level)/2f);
		//go.transform.rotation=transform.rotation;
        timer=0;
    }
    protected override void Update()
    {
        if(shotTimer > 0)
		{
			shotTimer -= Time.deltaTime;
		}
        var main=particles.main;
        Color c=main.startColor.color;
        c.a=0;
        if(timer<level)timer+=Time.deltaTime;
        if(level<timer && Bullet.blink)c.a=1f;
        else if(timer>level/2f && Bullet.blink)c.a=0.5f;
        main.startColor=c;
    }
	public override void Level(int i)
	{
        if(i<4){
            level=4-i;
            finalDamage=damage+damageByLevel*(i-1);
        }
    }
}
