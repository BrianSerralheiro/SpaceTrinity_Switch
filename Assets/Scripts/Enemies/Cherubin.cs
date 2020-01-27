using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherubin : EnemyBase
{
    Del update;
    float time;
    int shots,shotID;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=40;
        points=60;
        update=In;
        shotID=ei.bulletsID[0];
        fallSpeed=-4;
    }
    void In(){
        SlowFall();
        if(transform.position.y<Scaler.sizeY/4)update=Wait;
    }
    void Wait(){
        if(time<Time.time){
            shots++;
            switch (Random.Range(0,3))
            {
                case 0: 
                    update=Shot1;
                    break;
                case 1: 
                    update=Shot2;
                    break;
                case 2: 
                    update=Shot3;
                    break;
            }
        }
    }
    void Shot1(){
        for (int i = 0; i < 4; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.bulleSpeed=12;
            bu.spriteID=shotID;
            go.transform.position=transform.position+Vector3.left/2-Vector3.forward/10+Vector3.down*i;
            go.transform.up=Vector3.down;
        }
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }
    void Shot2(){
        for (int i = 0; i < 12; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.spriteID=shotID;
            bu.bulleSpeed=6;
            bu.Timer(4);
            go.transform.position=transform.position-Vector3.forward/10;
            go.transform.rotation=Quaternion.Euler(0,0,i*30);
        }
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }
    void Shot3(){
        GameObject go=new GameObject("enemybullet");
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
        go.AddComponent<BoxCollider2D>();
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner=name;
        bu.spriteID=shotID;
        bu.bulleSpeed=12;
        go.transform.position=transform.position+Vector3.right/2-Vector3.forward/10;
        Vector3 vector=GetPlayer().position-go.transform.position;
        vector.z=0;
        go.transform.up=vector;
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }

    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
    }
}
