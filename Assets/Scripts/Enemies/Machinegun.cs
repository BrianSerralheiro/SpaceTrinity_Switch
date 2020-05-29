using UnityEngine;

public class Machinegun : EnemyBase
{
    static int shotId,trailID,impactID;
    float time,heat,firerate=1,minrate=1;
    bool overheat;
    Transform turret;
    public override void SetSprites(EnemyInfo ei){
        hp=30;
        points=40; 
        GameObject go=new GameObject("enemybiggr");
        go.transform.parent=transform;
        turret=go.transform;
        turret.localPosition=Vector3.zero+Vector3.back/10;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        go.AddComponent<CircleCollider2D>();
        Destroy(GetComponent<Collider2D>());
        shotId=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        time=Time.time+2;
    }
    void Start()
    {
        _renderer=turret.GetComponent<SpriteRenderer>();
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Translate(0,-Time.deltaTime*2,0);
        if(overheat){
            heat=Mathf.MoveTowards(heat,0,Time.deltaTime/3);
            Vector3 v=GetPlayer(turret.position).position-turret.position;
            v.z=0;
            damageTimer=Mathf.Cos(Time.time*10);
            turret.Rotate(Vector3.Cross(-turret.up,v)*Time.deltaTime*5);
            if(heat==0)overheat=false;
        }
        else{
            firerate=Mathf.MoveTowards(firerate,0.1f,Time.deltaTime/10);
            Vector3 v=GetPlayer(turret.position).position-turret.position;
            v.z=0;
            turret.Rotate(Vector3.Cross(-turret.up,v)*Time.deltaTime*25);
            if(time<Time.time){
                Shot();
                if(heat>1){
                    overheat=true;
                    firerate=minrate;
                }
            }
        }
        if(transform.position.y<-Scaler.sizeY-1)Die();
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-turret.up;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=8;
        bu.Timer(8);
        bu.spriteID=shotId;
		bu.particleID=trailID;
		bu.impactID=impactID;
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-turret.up;
        time=Time.time+firerate;
        heat+=0.05f;
    }
    protected override void Die(){
        GameObject g=new GameObject("hole");
        g.transform.position=transform.position;
        g.AddComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
        g.AddComponent<TurretHole>();
        base.Die();
    }
}
