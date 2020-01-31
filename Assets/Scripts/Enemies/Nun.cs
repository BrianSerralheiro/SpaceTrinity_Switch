using System.Collections.Generic;
using UnityEngine;

public class Nun : EnemyBase
{
    Transform[] wings=new Transform[4];
    static EnemyInfo miracle;
    float time,offset=2;
    int spawns;

    HashSet<Miracle> miracles=new HashSet<Miracle>(),fred=new HashSet<Miracle>();
	public override void SetSprites(EnemyInfo ei)
	{
        if(!miracle)miracle=((CarrierInfo)ei).spawnable;
        name+="Big";
        points=120;
        hp=200;
        time=Time.time+5;
        for (int i = 0; i < 4; i++)
        {
            GameObject g=new GameObject("wing"+i);
            SpriteRenderer sp=g.AddComponent<SpriteRenderer>();
            sp.sprite=ei.sprites[1];
            sp.flipX=i%2==1;
            wings[i]=g.transform;
            wings[i].parent=transform;
            wings[i].localScale=Vector3.one*(1-i/2*0.2f);
            wings[i].localPosition=new Vector3(-0.25f+i%2*0.5f,0.4f-i/2*0.6f,0.1f);
        }
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>Scaler.sizeY/2)SlowFall();
        if(time<Time.time)Spawn();
        for (int i = 0; i < 4; i++)
                wings[i].rotation=Quaternion.Euler(0,0,Mathf.PingPong(Time.time*60,45)*(i%2==1?-1:1));
        foreach(Miracle m in miracles)
        {
            if(m==null)fred.Add(m);
            else{
                m?.Spawn();
                if(m.enabled)fred.Add(m);
            }
        }
        foreach(Miracle m in fred)
        {
            miracles.Remove(m);
        }
        fred.Clear();
        if(transform.position.y>Scaler.sizeY+3)Die();
    }
    void Spawn(){
        if(spawns++>15){
            fallSpeed=3;
            SlowFall();
        }
        GameObject go=new GameObject("enemy");
        go.AddComponent<SpriteRenderer>().sprite=miracle.sprites[0];
        go.AddComponent<BoxCollider2D>();
        Rigidbody2D r2=go.AddComponent<Rigidbody2D>();
        r2.useFullKinematicContacts=true;
        r2.isKinematic=true;
        Miracle m=go.AddComponent<Miracle>();
        m.SetSprites(miracle);
        m.enabled=false;
        miracles.Add(m);
        go.transform.position=new Vector3(transform.position.x+Random.Range(-1f,1f),-Scaler.sizeY/2,-0.1f);
        time=Time.time+offset;
        if(offset>0.6f)offset-=0.1f;
    }
    
	protected override void Die()
	{
        foreach (Miracle m in miracles)
        {
            m?.Kill(killerid);
        }
        base.Die();
    }
}
