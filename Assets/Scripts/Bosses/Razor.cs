using UnityEngine;

public class Razor : EnemyBase
{
    int shotID;
    float time,spd=6;
    bool revert;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=1000;
        points=1000;
        shotID=ei.bulletsID[0];
        name+="Boss";
        EnemySpawner.boss=true;
        fallSpeed=-4;
    }
    
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>Scaler.sizeY-4){
            SlowFall();
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
        go.transform.position=transform.position-transform.up*2;
        go.transform.up=-transform.up;
        time=Time.time+3;
    }
    
    protected override void Die()
	{
        EnemySpawner.boss=false;
        base.Die();
    }
}
