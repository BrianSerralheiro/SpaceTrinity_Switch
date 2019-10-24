using UnityEngine;

public class GravGun : Gun
{
    [SerializeField]
    private Material material;
    private int count=13;
    [SerializeField]
    private float timer;
    protected override void Awake()
	{
		shotId=Bullet.Register(shots[(Ship.skinID+1)*count+0]);
        for(int i=1;i<count;i++){
            Bullet.Register(shots[(Ship.skinID+1)*count+i]);
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
        bull.Size((int)(timer/2));

		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
        timer=0;
    }
    void Update()
    {
        if(timer<4)timer+=Time.deltaTime;
    }
	public override void Level(int i)
	{
        if(i<4)level=i;
    }
}
