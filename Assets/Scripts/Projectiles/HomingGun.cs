﻿using UnityEngine;

public class HomingGun : Gun
{
	public override void Load(int i,int j)
	{
		if(Ship.skinID[i]>=0 && Locks.Skin(j*3+Ship.skinID[i])){
            shotId=Bullet.Register(shots[(Ship.skinID[j]+1)*4]);
            for(int c=1;c<4;c++){
                Bullet.Register(shots[(Ship.skinID[j]+1)*4+c]);
            }
        }else{
            shotId=Bullet.Register(shots[0]);
            for(int c=1;c<4;c++){
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
