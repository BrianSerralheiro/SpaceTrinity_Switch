using UnityEngine;

public class Tank : EnemyBase
{
    float timer,speed,wait;
    Transform turret;
    int shotId;
    static int trailID,impactID;
	public override void SetSprites(EnemyInfo ei)
    {
        SetHP(60,ei.lifeproportion);
        name+="bigr";
        GameObject go=new GameObject("enemy");
        go.transform.parent=transform;
        turret=go.transform;
        turret.localPosition=Vector3.zero+Vector3.back/10;
        turret.Rotate(0,0,180);
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        shotId=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        WindZone zone=gameObject.AddComponent<WindZone>();
        zone.radius=1;
        zone.windMain=5;
        zone.windTurbulence=0;
        zone.mode=WindZoneMode.Spherical;
    }
	public override void Position(int i)
	{
		base.Position(i);
        transform.Translate(0,0,1,Space.World);
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
        v-=turret.position;
        v.z=0;
        v=Vector3.Cross(-turret.up,v);
        turret.Rotate(v*Time.deltaTime*100);
        // turret.rotation = Quaternion.Euler(0,0,Mathf.RoundToInt(Vector2.SignedAngle(Vector3.down,v-turret.position)/15)*15);

        if(timer<=0)Shot();
    }
    void Shot()
    {
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-turret.up*2+Vector3.back;
        go.transform.localScale=Vector3.one*2;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=12;
        bu.spriteID=shotId;
        bu.particleID=trailID;
        bu.impactID=impactID;
        SpriteRenderer s=go.AddComponent<SpriteRenderer>();
        s.sprite=Bullet.sprites[shotId];
        s.color=Color.gray/5f;
        EnemySpawner.AddPost(go);
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-turret.up;
        timer=2;
        WindZone zone=go.AddComponent<WindZone>();
        zone.radius=1f;
        zone.windMain=100;
        zone.windTurbulence=0;
        zone.mode=WindZoneMode.Spherical;
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
