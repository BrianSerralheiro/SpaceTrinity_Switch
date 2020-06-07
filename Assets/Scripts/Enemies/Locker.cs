using System.Collections.Generic;
using UnityEngine;

public class Locker : EnemyBase
{
    Transform target;
    HashSet<Missile> rockets=new HashSet<Missile>();
    Core aim;
    float time,delay,wait;
    int count,bateries=2;
    static Sprite rocket;
    bool right;
    static int trailID,impactID;
    public override void SetSprites(EnemyInfo ei)
    {
        hp=130;
        name+="big";
        points=200;
		GameObject go=new GameObject("aim");
		aim=go.AddComponent<Core>().Set(ei.sprites[2],new Color(1,1,1,0));
        aim.transform.parent=transform;
        if(!rocket)rocket=ei.sprites[1];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>Scaler.sizeY/2 || bateries==0){
            SlowFall();
            transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,Time.deltaTime*15);
            if(bateries>0 && transform.position.y<Scaler.sizeY/2){
                time=Time.time+3;
                target=GetPlayer();
                aim.transform.position=transform.position;
            }
        }
        else
        {
            if(wait>Time.time){
                transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,Time.deltaTime*15);
            }
            else if(time>Time.time){
                aim.transform.position=Vector3.MoveTowards(aim.transform.position,target.position-Vector3.forward,10*Time.deltaTime);
                float f=time-Time.time;
                aim.Set(Time.time%f/f);
                Vector3 v =target.position-transform.position;
                v.z=0;
                transform.Rotate(Vector3.Cross(-transform.up,v)*Time.deltaTime*15);
            }else
            {
                if(count<4){
                    aim.Set(0);
                    if(delay<Time.time){
                        Create();
                    }
                }
                else
                {
                    time=Time.time+10;
                    wait=Time.time+7;
                    target=GetPlayer();
                    aim.transform.position=transform.position;
                    bateries--;
                    fallSpeed=-4;
                    count=0;
                }
            }
        }
        foreach (Missile rocket in rockets)
        {
            if(!rocket){
                rockets.Remove(rocket);
                break;
            }
            Vector3 v=target.position-rocket.transform.position;
            v.z=0;
            rocket.transform.Rotate(Vector3.Cross(rocket.transform.up,v)*Time.deltaTime*15);
            rocket.transform.Translate(0,Time.deltaTime*8,0);
            ParticleManager.Emit(trailID,rocket.transform.position,1);
            if(rocket.time<Time.time){
                rocket.release=true;
                rockets.Remove(rocket);
                break;
            }
        }
    }
    
	protected override void Die()
	{
		base.Die();
		foreach (Missile t in rockets)
        {
            if(t)t.release=true;
        }
	}
    private void Create()
	{
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=rocket;
		Missile mi=go.AddComponent<Missile>();
        mi.SetHP(20);
        mi.time=Time.time+3;
        mi.trailID=trailID;
        mi.impactID=impactID;
		rockets.Add(mi);
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		go.transform.position=transform.position+transform.right*(right?1.2f:-1.2f);
        go.transform.up=-transform.up;
        right=!right;
        count++;
        delay=Time.time+0.5f;
	}
}
