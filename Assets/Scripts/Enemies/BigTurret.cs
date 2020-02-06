using UnityEngine;

public class BigTurret : EnemyBase
{
    float timer,delay=0.5f,reload=5f;

    int shotId,counter=5,shots=5,cicles=4;
    Transform turret;
	public override void SetSprites(EnemyInfo ei){
        hp=30;
        GameObject go=new GameObject("enemybig");
        go.transform.parent=transform;
        turret=go.transform;
        turret.localPosition=Vector3.zero+Vector3.back/10;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        go.AddComponent<CircleCollider2D>();
        Destroy(GetComponent<Collider2D>());
        shotId=ei.bulletsID[0];
    }
    void Start()
    {
        _renderer=turret.GetComponent<SpriteRenderer>();
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        timer-=Time.deltaTime;
        transform.Translate(0,-Time.deltaTime*2,0);
        if(cicles>0){
            Vector3 v=GetPlayer(turret.position).position-turret.position;
            v.z=0;
            turret.Rotate(Vector3.Cross(-turret.up,v)*Time.deltaTime*25);
            if(timer<=0)Shot();
        }
        else turret.rotation=Quaternion.RotateTowards(turret.rotation,Quaternion.identity,Time.deltaTime);
        if(transform.position.y<-Scaler.sizeY-1)Die();
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-turret.up;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulleSpeed=8;
        bu.spriteID=shotId;
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-turret.up;
        if(counter--==0){
            timer=reload;
            counter=shots;
            cicles--;
        }else
            timer=delay;
    }
    protected override void Die(){
        GameObject g=new GameObject("hole");
        g.transform.position=transform.position;
        g.AddComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
        g.AddComponent<TurretHole>();
        base.Die();
    }
}
