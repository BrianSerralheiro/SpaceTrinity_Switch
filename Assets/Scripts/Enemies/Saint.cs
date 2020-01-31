﻿using System.Collections.Generic;
using UnityEngine;

public class Saint : EnemyBase
{
    LineRenderer line;
    Transform mask;
    Vector3[] positions=new Vector3[20];
    float time,timer,posX,offset=2;
    static EnemyInfo miracle;
    int ap,shotId;
    HashSet<Miracle> miracles=new HashSet<Miracle>(),fred=new HashSet<Miracle>();
    Del update;
    new BoxCollider2D collider;
    public override void SetSprites(EnemyInfo ei)
	{
        if(!miracle)miracle=((CarrierInfo)ei).spawnable;
        hp=1000;
        points=800;
        name+="Boss";
        EnemySpawner.boss=true;
        damageEffect=true;
        GameObject g;
        for (int i = 0; i < 6; i++)
        {
            g=new GameObject("wing"+i);
            SpriteRenderer sp=g.AddComponent<SpriteRenderer>();
            sp.sprite=ei.sprites[1];
            sp.flipX=i%2==1;
            Transform t=g.transform;
            t.parent=transform;
            t.localPosition=Vector3.forward/10;
            t.rotation=Quaternion.Euler(0,0,(-135-i/2*45)*(i%2==1?1:-1));
            t.Translate(0,1,0);
            g=new GameObject("ligh"+i);
            sp=g.AddComponent<SpriteRenderer>();
            sp.sprite=ei.sprites[2];
            sp.flipX=i%2==1;
            sp.maskInteraction=SpriteMaskInteraction.VisibleOutsideMask;
            g.transform.parent=t;
            g.transform.localPosition=Vector3.back/20;
            g.transform.localRotation=Quaternion.identity;
        }
        g=new GameObject("enemylaser");
        g.transform.parent=transform;
        collider=g.AddComponent<BoxCollider2D>();
        collider.enabled=false;
        line=g.AddComponent<LineRenderer>();
        line.positionCount=positions.Length;
        line.enabled=false;
        timer=Time.time+5;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].y=i*Scaler.sizeY/9-Scaler.sizeY-1;
            positions[i].z=-0.2f;
        }
        
        g=new GameObject("wings nask");
        g.AddComponent<SpriteMask>().sprite=ei.sprites[3];
        mask=g.transform;
        mask.parent=transform;
        mask.localScale=Vector3.one*5;
        timer=Time.time+3;
        update=Intro;
        shotId=ei.bulletsID[0];
        fallSpeed=-3;
    }
    void Intro(){
        SlowFall();
        if(transform.position.y<2)update=Divining;
    }
    void Divining(){
        mask.localScale=Vector3.MoveTowards(mask.localScale,Vector3.zero,Time.deltaTime*3);
        if(time<Time.time)Divine();
    }
    void Spawning(){
        mask.localScale=Vector3.MoveTowards(mask.localScale,Vector3.one*5,Time.deltaTime*5);
        if(time<Time.time)Spawn();
    }
    void Shooting(){
        Shot();
    }
    void Spawn(){
        if(ap++>10){
            update=Shooting;
            ap=0;
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
        go.transform.position=new Vector3(transform.position.x+Random.Range(-4f,4f),Scaler.sizeY,-0.1f);
        time=Time.time+offset;
        if(offset>0.6f)offset-=0.1f;
    }
    void Divine(){
        if(!line.enabled && time<Time.time){
            Show();
        }
        else{
            if(line.enabled){
                timer-=Time.deltaTime;
                if(timer<1)collider.enabled=!collider.enabled;        
                float f=4.5f/line.positionCount;
                float f1=40f;
                float t=Time.time*10;
                float t1=Time.time*40;
                line.widthMultiplier=2-timer;
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i].x=posX+(Mathf.Sin(t+f*i)*0.6f+Mathf.Cos(t1+f1*i)*0.4f)*(2-timer);
                }
                line.SetPositions(positions);
                collider.size=line.bounds.size;
                collider.offset=line.bounds.center-transform.position;
                if(timer<0){
                    line.enabled=false;
                    collider.enabled=false;
                    time=Time.time+2;
                    if(ap++>3){
                        update=Spawning;
                        ap=0;
                        offset=2;
                    }
                }
            }
        }
    }
    void Shot(){
        Vector3 v =new Vector3(transform.position.x,Scaler.sizeY);
        for (int i = 0; i < 10; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.spriteID=shotId;
            bu.bulleSpeed=6;
            bu.Timer(8);
            go.transform.position=v-Vector3.forward/10;
            go.transform.rotation=Quaternion.Euler(0,0,-90+i*-18);
        }
        time=Time.time+4;
        update=Divining;
    }
    new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="enemylaser") return;
		base.OnCollisionEnter2D(col);
	}
    public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+4,0);
	}
    void Show(){
        line.enabled=true;
        line.widthMultiplier=0;
        posX=GetPlayer().position.x;
        timer=2;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].x=posX;
        }
        line.SetPositions(positions);
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
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
    }
    void Dying(){
        mask.localScale=Vector3.MoveTowards(mask.localScale,Vector3.zero,Time.deltaTime);
        transform.GetChild(0).Translate(0,-Time.deltaTime*2,0,Space.World);
        transform.Translate(0,-Time.deltaTime,0);
        if(transform.position.y<-Scaler.sizeY/2){
            Destroy(gameObject);
            EnemySpawner.boss=true;
        }
    }
    protected override void Die()
	{
		update=Dying;
		EnemySpawner.points[killerid]+=points;
        foreach (Miracle m in miracles)
        {
            m?.Kill(killerid);
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
	}
}
