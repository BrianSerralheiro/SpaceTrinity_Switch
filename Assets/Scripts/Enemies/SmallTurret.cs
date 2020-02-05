using UnityEngine;

public class SmallTurret : EnemyBase
{
    Transform turret,lid;
    float timer,delay,reload;
    int shotId,counter,shots,cicles,pathID;
    static BulletPath[] paths;
    bool mirror;
	public override void SetSprites(EnemyInfo ei){
        hp=5;
        gameObject.AddComponent<SpriteMask>().sprite=ei.sprites[0];
        GameObject go=new GameObject("lid");
        lid=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=Vector3.zero;
        SpriteRenderer sr=go.AddComponent<SpriteRenderer>();
        sr.sprite=ei.sprites[1];
        sr.maskInteraction=SpriteMaskInteraction.VisibleInsideMask;
        go=new GameObject("enemy");
        turret=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=Vector3.zero;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
        go.AddComponent<CircleCollider2D>();
        shotId=ei.bulletsID[0];
        MultiPathEnemy multi=(MultiPathEnemy)ei;
        delay=multi.shotDelay;
        counter=shots=multi.shotCount;
        cicles=multi.cicles;
        reload=multi.reloadTime;
        Destroy(GetComponent<Collider2D>());
        if(paths==null)paths=multi.paths;
        pathID=Random.Range(0,paths.Length);
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
        if(timer>reload){
            turret.gameObject.SetActive(false);
            lid.localPosition=Vector3.MoveTowards(lid.localPosition,Vector3.zero+Vector3.back/10,Time.deltaTime*2);
            lid.Rotate(0,0,40*Time.deltaTime);
        }
        else if(timer<1){
            lid.localPosition=Vector3.MoveTowards(lid.localPosition,Vector3.right+Vector3.back/10,Time.deltaTime);
            if(lid.localPosition==Vector3.right+Vector3.back/10){
                turret.up=-paths[pathID].GetNode0(mirror).normalized;
                turret.gameObject.SetActive(true);
            }
            lid.Rotate(0,0,-40*Time.deltaTime);
            if(timer<=0)
                Shot();
        }
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=turret.position;
        PathBullet bu=go.AddComponent<PathBullet>();
        bu.owner="enemy";
        bu.bulleSpeed=5;
        bu.spriteID=shotId;
        bu.mirror=mirror;
        bu.path=paths[pathID];
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<CircleCollider2D>();
        go.transform.up=-transform.up;
        if(counter--==0){
            timer=reload+1;
            counter=shots;
            pathID=Random.Range(0,paths.Length);
            mirror=Random.value>0.5f;
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
