using UnityEngine;

public class Spiderling : EnemyBase
{
    Transform follow;
    Transform[] legs;
    public override void SetSprites(EnemyInfo ei)
    {
        legs=new Transform[8];
        GameObject go;
        for (int i = 0; i < 8; i++)
        {
            go=new GameObject("leg"+i);
            go.AddComponent<SpriteRenderer>().sprite=ei.sprites[(i/2)+1];
            legs[i]=go.transform;
            legs[i].parent=transform;
            i++;
            go=new GameObject("leg"+i);
            SpriteRenderer s=go.AddComponent<SpriteRenderer>();
            s.sprite=ei.sprites[(i/2)+1];
            s.flipX=true;
            legs[i]=go.transform;
            legs[i].parent=transform;
        }
        legs[0].position=new Vector3(0.35f,0.35f);
        legs[1].position=new Vector3(-0.35f,0.35f);
        legs[2].position=new Vector3(0.35f,0.2f);
        legs[3].position=new Vector3(-0.35f,0.2f);
        legs[4].position=new Vector3(0.3f,-0.2f);
        legs[5].position=new Vector3(-0.3f,-0.2f);
        legs[6].position=new Vector3(0.25f,-0.4f);
        legs[7].position=new Vector3(-0.25f,-0.4f);
    }
    public void MoveTo(Transform t){
        follow=t;
    }
    public void Web(Transform t){
        transform.parent=t;
        transform.localRotation=Quaternion.identity;
        transform.localScale=new Vector3(1,0.5f);
        transform.localPosition=Vector3.zero;
    }
    public void OnDie(){
        Die();
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.parent){
            transform.Translate(0,-Time.deltaTime*14,0);
            if(transform.position.y<-Scaler.sizeY-2)Die();
            legs[0].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90+180)*50f);
            legs[1].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90)*50f);
            legs[2].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90+45)*50f);
            legs[3].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90)*50f);
            legs[4].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90+200)*40f);
            legs[5].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90)*40f);
            legs[6].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90+30)*15f);
            legs[7].eulerAngles=new Vector3(0,0,Mathf.Cos(Time.time*90)*15f);
            
        }
        else {
            transform.Rotate(0,0,Time.deltaTime*720);
            transform.position=Vector3.MoveTowards(transform.position,follow.position+Vector3.back/2,Vector3.Distance(transform.position,follow.position)*4*Time.deltaTime);
            if(transform.position==follow.position+Vector3.back/2)Web(follow);
        }
    }
}
