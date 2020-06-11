using UnityEngine;

public class Razor : EnemyBase
{
    int shotID,trailID,impactID;
    float time,spd=6;
    bool revert;
    public override void SetSprites(EnemyInfo ei)
	{
        SetHP(150,ei.lifeproportion);
        points=500;
        shotID=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        name+="big";
        fallSpeed=-4;
        // lifetime=Time.time+12;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>Scaler.sizeY-4){
            SlowFall();
            transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,Time.deltaTime*10);
        }
        else{
            Transform target=GetPlayer(transform.position);
            if(revert){
                transform.Translate(transform.position.x>0?-spd*Time.deltaTime:spd*Time.deltaTime,0,0,Space.World);
                if(Mathf.Abs(transform.position.x)<Scaler.sizeX/4)revert=false;
            }else{
                if(Mathf.Abs(target.position.x-transform.position.x)<5)transform.Translate(target.position.x>transform.position.x?-spd*Time.deltaTime:spd*Time.deltaTime,0,0,Space.World);
                if(Mathf.Abs(transform.position.x)>Scaler.sizeX/2-2)revert=true;
            }
            Vector3 v=target.position-transform.position;
            v.z=0;
            transform.Rotate(Vector3.Cross(-transform.up,v)*30*Time.deltaTime);
            if(time<Time.time)Shot();
        }
    }
    void Shot(){
        GameObject go = new GameObject("enemybullet");
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
        go.AddComponent<CircleCollider2D>();
        Bullet bu = go.AddComponent<Bullet>();
        bu.owner=name;
        bu.spriteID=shotID;
        bu.particleID=trailID;
        bu.impactID=impactID;
        go.transform.position=transform.position-transform.up*2;
        go.transform.up=-transform.up;
        time=Time.time+3;
    }
    
    protected override void Die()
	{
        base.Die();
    }
}
