using UnityEngine;

public class Tank : EnemyBase
{
    float timer;
    Transform turret;
    int shotId;
    
	public override void SetSprites(EnemyInfo ei){
        hp=200;
        GameObject go=new GameObject("enemy");
        go.transform.parent=transform;
        turret=go.transform;
        turret.localPosition=Vector3.zero+Vector3.back/10;
        turret.Rotate(0,0,180);
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
        if(transform.position.y>0){
            transform.Translate(0,-Time.deltaTime,0);
            Vector3 v=player.position-turret.position;
            v.z=0;
            turret.Rotate(Vector3.Cross(-turret.up,v)*Time.deltaTime*25);
            if(timer<=0)Shot();
        }
        else{
            transform.Translate(0,-Time.deltaTime*2.5f,0);
            turret.rotation=Quaternion.RotateTowards(turret.rotation,Quaternion.Euler(0,0,180),Time.deltaTime*30);
        }
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-turret.up*2;
        go.transform.localScale=Vector3.one*2;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulleSpeed=12;
        bu.spriteID=shotId;
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-turret.up;
        timer=2;
    }
    protected override void Die(){
        GameObject g=new GameObject("hole");
        g.transform.position=transform.position;
        g.AddComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
        g.AddComponent<TurretHole>();
        base.Die();
    }
}
