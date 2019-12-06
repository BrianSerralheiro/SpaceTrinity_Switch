using UnityEngine;

public class GravGun : Gun
{
    [SerializeField]
    private Material material;
    private int count=14;
    [SerializeField]
    private float timer;
    public override void Load(int i)
	{
        
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
		shots=null;
	}
	public override void Shoot()
	{ 
        if(!gameObject.activeSelf)return;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		SpriteRenderer sp= go.AddComponent<SpriteRenderer>();
        sp.sprite=Bullet.sprites[shotId];
        sp.material=material;
		go.AddComponent<BoxCollider2D>();
		GravBullet bull= go.AddComponent<GravBullet>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=false;
		bull.particleID=particleID;
		bull.spriteID=shotId;
        bull.Size((int)(timer/(level/2f)));

		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
        timer=0;
    }
    void Update()
    {
        if(timer<level)timer+=Time.deltaTime;
    }
	public override void Level(int i)
	{
        if(i<4)level=4-i;
    }
}
