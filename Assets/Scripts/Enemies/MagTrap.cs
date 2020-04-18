using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagTrap : EnemyBase
{
    Transform[] parts=new Transform[3];
    float timer;
    LineRenderer ring;
    CircleCollider2D circle;
    static Material material;
    static float rad;
    public override void SetSprites(EnemyInfo ei)
    {
        hp=40;
        points=40;
        circle=gameObject.AddComponent<CircleCollider2D>();
        for (int i = 0; i < 3; i++)
        {
            GameObject go=new GameObject("part");
            go.transform.rotation=Quaternion.Euler(0,0,120*i);
            go.transform.parent=transform;
            go.transform.Translate(0,0.3f,0.1f);
            parts[i]=go.transform;
            go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
            
        }
        if(!material)material=new Material(Shader.Find("Sprites/Default"));
        ring=gameObject.AddComponent<LineRenderer>();
        ring.loop=true;
        ring.enabled=false;
        ring.positionCount=20;
        ring.material=material;
        ring.startColor=ring.endColor=new Color(0.6f,1,1,0.6f);
        ring.widthMultiplier=0;
        rad=2*Mathf.PI/ring.positionCount;
        ring.useWorldSpace=false;
        for (int i = 0; i < ring.positionCount; i++)
        {
            ring.SetPosition(i,new Vector3(Mathf.Cos(i*rad),Mathf.Sin(i*rad),0.11f));
        }
        fallSpeed=-3;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Rotate(0,0,fallSpeed*10*Time.deltaTime);
        if(timer>0){
            timer-=Time.deltaTime;
            ring.widthMultiplier=Mathf.MoveTowards(ring.widthMultiplier,0.3f,Time.deltaTime/3);
            circle.radius=Mathf.MoveTowards(circle.radius,1.2f,Time.deltaTime);
            ring.enabled=true;
            fallSpeed=-1;
            foreach (Transform t in parts)
            {
                t.Translate(0,1*Time.deltaTime/2,0);
            }
        }
        if(timer==0){
            if(Vector3.Distance(GetPlayer(transform.position).position,transform.position)<3)timer=1;
        }
        else
        {
            for (int i = 0; i < ring.positionCount; i++)
            {
                float f=1+Mathf.Cos((Time.time+i)*60)/10f;
                ring.SetPosition(i,new Vector3(Mathf.Cos(i*rad),Mathf.Sin(i*rad),0.2f)*f*(1f-timer));
            }
        }
        SlowFall();
    }
}
