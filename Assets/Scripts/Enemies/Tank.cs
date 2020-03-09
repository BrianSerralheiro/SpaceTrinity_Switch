using UnityEngine;

public class Tank : EnemyBase
{
    float timer,speed,wait;
    Transform turret;
    int shotId;
    
	public override void SetSprites(EnemyInfo ei)
    {
        hp=60;
        name+="biggr";
        GameObject go=new GameObject("enemy");
        go.transform.parent=transform;
        turret=go.transform;
        turret.localPosition=Vector3.zero+Vector3.back/10;
        turret.Rotate(0,0,180);
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
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
        if(transform.position.y>-Scaler.sizeY/2)
        {
            transform.Translate(speed*Time.deltaTime,-Time.deltaTime,0,Space.World);
            Aim();
        }
        else
        {
            if(wait>5)
            {
                transform.Translate(0,-Time.deltaTime*2.5f,0,Space.World);
                turret.rotation=Quaternion.RotateTowards(turret.rotation,Quaternion.Euler(0,0,180),Time.deltaTime*30);
            }
            else 
            {
                wait+=Time.deltaTime;
                transform.Translate(speed*Time.deltaTime,0,0,Space.World);
                Aim();

            }
        }
        if(transform.position.y<-Scaler.sizeY-2)Die();
    }
    void Aim()
    {
        Vector3 v=GetPlayer(transform.position).position;
        float s=v.x>transform.position.x+2?2:v.x+2<transform.position.x?-1:0;
        speed=Mathf.MoveTowards(speed,s,Time.deltaTime/2);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-15*s),7*Time.deltaTime);
        // v-=turret.position;
        // v.z=0;
        // v=Vector3.Cross(-turret.up,v);
        // turret.Rotate(v*Time.deltaTime*100);
        turret.rotation = Quaternion.Euler(0,0,Mathf.RoundToInt(Vector2.SignedAngle(Vector3.down,v-turret.position)/15)*15);

        if(timer<=0)Shot();
    }
    void Shot()
    {
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
    void OnCollisionStay2D(Collision2D col)
    {
        Tank tank=col.gameObject.GetComponent<Tank>();
        if(tank)
        {
            float f=Mathf.Abs(speed);
            if(tank.transform.position.x<transform.position.x)transform.Translate(f*Time.deltaTime,0,0,Space.World);
            else transform.Translate(-f*Time.deltaTime,0,0,Space.World);
        }
    }
    protected override void Die()
    {
        GameObject g=new GameObject("hole");
        g.transform.position=transform.position;
        g.transform.rotation=transform.rotation;
        g.AddComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
        g.AddComponent<TurretHole>();
        base.Die();
    }
}
