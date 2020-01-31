using UnityEngine;

public class Miracle : EnemyBase
{
    int shots=100,shotId;
    float angle,time,spawn;
    Core core;
    Transform tail,wingL,wingR;
	public override void SetSprites(EnemyInfo ei)
	{
        hp=20;
        points=20;
        GameObject g=new GameObject("wingR");
        g.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        wingR=g.transform;
        g=new GameObject("wingL");
        SpriteRenderer sp=g.AddComponent<SpriteRenderer>();
        sp.sprite=ei.sprites[1];
        sp.flipX=true;
        wingL=g.transform;
        g=new GameObject("tail");
        g.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
        tail=g.transform;
        tail.parent=wingL.parent=wingR.parent=transform;
        wingR.localPosition=Vector3.right*0.4f+Vector3.forward/10;
        wingL.localPosition=Vector3.left*0.4f+Vector3.forward/10;
        // tail.localPosition=Vector3.down*0.3f;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>-Scaler.sizeY/2){
            SlowFall();
            wingR.rotation=Quaternion.Euler(0,0,Mathf.Cos(Time.time*45)*30);
            wingL.rotation=Quaternion.Euler(0,0,-Mathf.Cos(Time.time*45)*30);
        }else{
            if(shots>0){
                if(time<Time.time)Shot();
            }
            else {
                SlowFall();
                wingR.rotation=Quaternion.Euler(0,0,Mathf.Cos(Time.time*45)*30);
                wingL.rotation=Quaternion.Euler(0,0,-Mathf.Cos(Time.time*45)*30);
            }
        }
    }
    public void Spawn(){
        if(!core){
            core=gameObject.AddComponent<Core>().Set(new Color(1,1,1,0.8f),new Color(1,1,1,0.2f));
        }
        spawn+=Time.deltaTime;
        core.Set(Mathf.Cos(Time.time*(10+20*spawn)));
        if(spawn>4){
            enabled=true;
            GetComponent<SpriteRenderer>().color=Color.white;
            Destroy(core);
        }
    }
    void Shot(){
        time=Time.time+0.01f;
        GameObject g=new GameObject("enemybullet");
        g.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        g.AddComponent<CircleCollider2D>();
        Bullet bu=g.AddComponent<Bullet>();
        bu.owner=name;
        bu.Timer(10);
        bu.bulleSpeed=5;
        g.transform.position=transform.position;
        g.transform.rotation=Quaternion.Euler(0,0,angle);
        tail.Rotate(0,0,90*Time.deltaTime);
        angle+=81;
        shots--;
    }
}
