using System.Collections.Generic;
using UnityEngine;

public class Nun : EnemyBase
{
    EnemyInfo miracle;
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
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>Scaler.sizeY/2)SlowFall();
        if(time<Time.time)Spawn();
        foreach (Miracle m in miracles)
        {
            if(m==null)fred.Add(m);
            else {
                m?.Spawn();
                if(m.enabled)fred.Add(m);
            }
        }
        foreach (Miracle m in fred)
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
