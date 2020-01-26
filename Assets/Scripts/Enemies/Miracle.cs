using UnityEngine;

public class Miracle : EnemyBase
{
    int shots=100,shotId;
    float angle,time,spawn;
    Core core;
	public override void SetSprites(EnemyInfo ei)
	{
        hp=20;
        points=20;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>-Scaler.sizeY/2)SlowFall();
        else{
            if(shots>0){
                if(time<Time.time)Shot();
            }
            else SlowFall();
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
        angle+=81;
        shots--;
    }
}
