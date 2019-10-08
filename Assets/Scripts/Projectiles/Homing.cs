using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Bullet
{
    Transform target;
    float speed=5;
    void Update()
    {
        if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		if(bulletTime<=0)renderer.sprite=SpriteBase.I.bullets[spriteID+(blink?0:1)];
        if(target){
            timer=2;
            speed+=Time.deltaTime;
            if(speed>10)speed=10;
            Vector3 v=target.position-transform.position;
            v.z=0;
            v.Normalize();
            target.Rotate(Vector3.Cross(v,transform.up));
        }
        else {
            timer-=Time.deltaTime;
            speed=5;
        }
		transform.Translate(0,Time.deltaTime*speed,0);
		if(timer<=0) Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D col) {
        if(col.name=="Enemy"){
            if(target){
                if((target.position-transform.position).sqrMagnitude>(col.transform.position-transform.position).sqrMagnitude)
                    target=col.transform;
            }
            else target=col.transform;
        }
    }
}
