using UnityEngine;

public class GravGun : Gun
{
    [SerializeField]
    private int explosionDamage;
    public override void Load(int i)
	{
        GravBullet.impact=impactParticle.gameObject;
        impactParticle=null;
        base.Load(i);
    }
    //     finalDamage=damage;
	// 	if(Ship.skinID[i]>=0 && Locks.Skin(i*3+Ship.skinID[i])){
    //         shotId=Bullet.Register(shots[(Ship.skinID[i]+1)*count]);
    //         for(int c=1;c<count;c++){
    //             Bullet.Register(shots[(Ship.skinID[i]+1)*count+c]);
    //         }
    //     }else{
    //         shotId=Bullet.Register(shots[0]);
    //         for(int c=1;c<count;c++){
    //             Bullet.Register(shots[c]);
    //         }
    //     }
    //     particles=GetComponent<ParticleSystem>();
	// 	shots=null;
	// }
	public override void Shoot()
	{ 
        if(!gameObject.activeSelf || shotTimer > 0)return;
        shotTimer = fireRate;
		//ParticleManager.Emit(17,transform.position,1);
		GameObject go = Shot?Instantiate(Shot): new GameObject("playerbullet");
		go.name = "playerbullet";
		GravBullet bull= go.AddComponent<GravBullet>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.areaDamage=explosionDamage;
		bull.particleID=particleID;
        bull.impactID=impactID;
		bull.spriteID=shotId;
        bull.bulletSpeed=bulletSpeed;
        bull.Size(level);

		go.transform.position=transform.position;
    }
    // protected override void Update()
    // {
    //     if(shotTimer > 0)
	// 	{
	// 		shotTimer -= Time.deltaTime;
	// 	}
    //     var main=particles.main;
    //     Color c=main.startColor.color;
    //     c.a=0;
    //     if(timer<2)timer+=Time.deltaTime;
    //     if(2<timer && Bullet.blink)c.a=1f;
    //     else if(timer>1 && Bullet.blink)c.a=0.5f;
    //     main.startColor=c;
    // }
	public override void Level(int i)
	{
        if(i<4){
            level=i;
        }
    }
}
