using UnityEngine;

public class MagTrap : EnemyBase
{
    Transform[] parts=new Transform[3];
    float timer;
    LineRenderer ring;
    CircleCollider2D circle;
    static Material material;
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
        if(!material)material=ei.material;
        ring=gameObject.AddComponent<LineRenderer>();
        ring.loop=true;
        ring.enabled=false;
        ring.positionCount=6;
        ring.material=material;
        ring.startColor=ring.endColor=new Color(0.6f,1,1,0.6f);
        ring.widthMultiplier=0;
        ring.useWorldSpace=false;
        ring.textureMode=LineTextureMode.RepeatPerSegment;
        // for (int i = 0; i < ring.positionCount; i++)
        // {
        //     ring.SetPosition(i,parts[i].localPosition+parts[i].up);
        // }
        fallSpeed=-3;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Rotate(0,0,fallSpeed*10*Time.deltaTime);
        if(timer>0){
            timer-=Time.deltaTime;
            ring.widthMultiplier=Mathf.MoveTowards(ring.widthMultiplier,0.8f,Time.deltaTime/3);
            circle.radius=Mathf.MoveTowards(circle.radius,1.2f,Time.deltaTime);
            ring.enabled=true;
            fallSpeed=-1;
            foreach (Transform t in parts)
            {
                t.Translate(0,1*Time.deltaTime/2,0);
            }
        }
        if(timer==0){
            if(Vector3.Distance(GetPlayer(transform.position).position,transform.position)<6)timer=1;
        }
        else
        {
            for (int i = 0; i < ring.positionCount; i++)
            {
                if(i%2==0)ring.SetPosition(i,parts[i/2].localPosition*1.2f+Vector3.forward/10f);
                else ring.SetPosition(i,(parts[i/2].localPosition+parts[(i/2+1)%3].localPosition)*1.2f+Vector3.forward/10f);
            }
        }
        SlowFall();
    }
}
