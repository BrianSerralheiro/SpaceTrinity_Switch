using UnityEngine;

public class SmallTurret : EnemyBase
{
    Transform turret,lid;
    float timer,delay=0.1f,reload=2;
    int shotId,counter,shots=4,dir;
    static int trailID,impactID;
	public override void SetSprites(EnemyInfo ei){
        SetHP(5,ei.lifeproportion);
        gameObject.AddComponent<SpriteMask>().sprite=ei.sprites[0];
        GameObject go=new GameObject("lid");
        lid=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=Vector3.back/10f;
        SpriteRenderer sr=go.AddComponent<SpriteRenderer>();
        sr.sprite=ei.sprites[1];
        sr.maskInteraction=SpriteMaskInteraction.VisibleInsideMask;
        go=new GameObject("enemybigr");
        turret=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=Vector3.back/20f;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
        go.AddComponent<CircleCollider2D>();
        shotId=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        Destroy(GetComponent<Collider2D>());
        dir=Random.Range(0,8);
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
        if(timer>reload-1){
            turret.gameObject.SetActive(false);
            lid.localPosition=Vector3.MoveTowards(lid.localPosition,Vector3.back/10f,Time.deltaTime*2);
            if(lid.localPosition!=Vector3.back/10f)lid.Rotate(0,0,60*Time.deltaTime);
        }
        else if(timer<1){
            lid.localPosition=Vector3.MoveTowards(lid.localPosition,Vector3.right+Vector3.back/10,Time.deltaTime);
            if(lid.localPosition==Vector3.right+Vector3.back/10){
                turret.rotation=Quaternion.Euler(0,0,dir*45);
                turret.gameObject.SetActive(true);
            }
            if(lid.localPosition!=Vector3.right+Vector3.back/10f)lid.Rotate(0,0,-40*Time.deltaTime);
            if(timer<=0)
                Shot();
        }
        if(transform.position.y<-Scaler.sizeY-1)Die();
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=turret.position-turret.up;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=5;
        bu.spriteID=shotId;
        bu.particleID=trailID;
        bu.impactID=impactID;
        bu.Timer(10);
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-turret.up;
        if(counter--==0){
            timer=reload;
            counter=shots;
            dir=Random.Range(0,8);
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
