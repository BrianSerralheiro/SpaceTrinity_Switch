using UnityEngine;

public class HomingGun : Gun
{
    public override void Shoot()
	{
        if(!gameObject.activeSelf)return;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[spriteID+(Bullet.blink ? 0 : 1)];
		go.AddComponent<BoxCollider2D>();
		CircleCollider2D col=go.AddComponent<CircleCollider2D>();
        col.isTrigger=true;
        col.radius=10;
		Homing bull= go.AddComponent<Homing>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=pierce;
		bull.particleID=particleID;
		bull.spriteID=spriteID;

		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
    }
}
