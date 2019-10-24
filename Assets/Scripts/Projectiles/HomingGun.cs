using UnityEngine;

public class HomingGun : Gun
{
	protected override void Awake()
	{
		shotId=Bullet.Register(shots[(Ship.skinID+1)*4+0]);
		Bullet.Register(shots[(Ship.skinID+1)*4+1]);
		Bullet.Register(shots[(Ship.skinID+1)*4+2]);
		Bullet.Register(shots[(Ship.skinID+1)*4+3]);
		shots=null;
	}
    public override void Shoot()
	{
        if(!gameObject.activeSelf)return;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId+((int)Time.time%4)];
		go.AddComponent<BoxCollider2D>();
		CircleCollider2D col=go.AddComponent<CircleCollider2D>();
        col.isTrigger=true;
        col.radius=10;
		Homing bull= go.AddComponent<Homing>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=pierce;
		bull.particleID=particleID;
		bull.spriteID=shotId;

		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
    }
}
