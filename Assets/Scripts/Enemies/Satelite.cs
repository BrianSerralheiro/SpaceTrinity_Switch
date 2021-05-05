using UnityEngine;

public class Satelite : EnemyBase
{
    static int shotID,trailID,impactID;
    bool up=true;
    float time;
    public override void SetSprites(EnemyInfo ei)
    {
        SetHP(90,ei.lifeproportion);
        points=300;
        name+="big";
        shotID=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        fallSpeed=-2;
        Destroy(GetComponent<BoxCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.layer=9;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        SlowFall();
        if(transform.position.y<Scaler.sizeY/2 && time<Time.time)Shot();
    }
    void Shot(){
        for (int i = 0; i < 2; i++)
        {
            GameObject go = new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<CircleCollider2D>();
            Bullet bu = go.AddComponent<Bullet>();
            bu.owner=name;
            bu.spriteID=shotID;
            bu.particleID=trailID;
            bu.impactID=impactID;
            Vector3 v =up?new Vector3(1.8f,1.5f):new Vector3(0.6f,-3.5f);
            if(i==0)v.x*=-1;
            go.transform.position=transform.position+v;
            go.transform.rotation=Quaternion.Euler(0,0,up?135*(i==0?1:-1):180);
        }
        up=!up;
        time=Time.time+1;
    }
}
