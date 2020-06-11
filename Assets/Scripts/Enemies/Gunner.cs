using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : EnemyBase
{
    static int shotID,trailID,impactID;
    float time;
    int shot=1;
    public override void SetSprites(EnemyInfo ei)
	{
		points = 30;
		SetHP(20,ei.lifeproportion);
		shotID=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        Instantiate(ei.particles[2],transform).transform.localPosition=new Vector3(0,1);
        fallSpeed=-4;
	}

    
	new void Update () {
		if(Ship.paused) return;
		base.Update();
        if(transform.position.y>Scaler.sizeY/2){
            SlowFall();
            time=Time.time+1.5f;
        }
        else{
            if(shot<=0)transform.Translate(0,-Time.deltaTime*6,0);
            else{
                Vector3 v=GetPlayer(transform.position).position-transform.position;
                v.z=0;
                transform.Rotate(Vector3.Cross(-transform.up,v)*Time.deltaTime*30);
                if(time<Time.time)Shot();
            }
            if(transform.position.y<-Scaler.sizeY-1 || transform.position.y>Scaler.sizeY+1 || transform.position.x<-Scaler.sizeX/2-1 || transform.position.x>Scaler.sizeX/2+1)Die();
        }
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-transform.up;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=10;
        bu.Timer(8);
        bu.spriteID=shotID;
        bu.particleID=trailID;
        bu.impactID=impactID;
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-transform.up;
        shot--;
        time=Time.time+1.5f;
    }
}
