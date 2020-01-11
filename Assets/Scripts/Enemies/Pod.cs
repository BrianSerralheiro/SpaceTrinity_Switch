using UnityEngine;
using System.Collections.Generic;

public class Pod : EnemyBase
{
    HashSet<Transform> missiles=new HashSet<Transform>(),toremove=new HashSet<Transform>();
    static Sprite missile;
    Transform target;
    Transform[] doors=new Transform[2];
    float timer;
    int shots=6;
	public override void SetSprites(EnemyInfo ei){
        if(missile==null)missile=ei.sprites[1];
        name+="Big";
        BoxCollider2D col=GetComponent<BoxCollider2D>();
        col.isTrigger=true;
        col.size=new Vector2(3,6);
        col.offset=new Vector2(0,-4);
        GameObject go=new GameObject("door");
        doors[0]=go.transform;
        doors[0].parent=transform;
        doors[0].localPosition=Vector3.zero;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
        go=new GameObject("door");
        doors[1]=go.transform;
        doors[1].parent=transform;
        doors[1].localPosition=Vector3.zero;
        SpriteRenderer sr=go.AddComponent<SpriteRenderer>();
        sr.sprite=ei.sprites[2];
        sr.flipX=true;

    }
    // Update is called once per frame
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Translate(0,-Time.deltaTime*2.5f,0);
        timer-=Time.deltaTime;
        if(shots>0 && target && timer<0)Shot();
        if(target){
            doors[0].localPosition=Vector3.MoveTowards(doors[0].localPosition,Vector3.left+Vector3.back/10,Time.deltaTime*2);
            doors[1].localPosition=-doors[0].localPosition+Vector3.back/5;
        }else {
            doors[0].localPosition=Vector3.MoveTowards(doors[0].localPosition,Vector3.zero+Vector3.back/10,Time.deltaTime*2);
            doors[1].localPosition=-doors[0].localPosition+Vector3.back/5;
        }
        if(shots<=0){
            doors[0].localPosition=Vector3.MoveTowards(doors[0].localPosition,Vector3.zero+Vector3.back/10,Time.deltaTime*2);
            doors[1].localPosition=-doors[0].localPosition+Vector3.back/5;
        }
        foreach (Transform t in toremove)
        {
            missiles.Remove(t);
        }
        foreach (Transform t in missiles)
        {
            if(t){
                t.Translate(0,-Time.deltaTime*10,0);
                if(t.position.x<-Scaler.sizeX-1 || t.position.x>Scaler.sizeX+1 || t.position.y<-Scaler.sizeY-1 || t.position.y>Scaler.sizeX+1)Destroy(t.gameObject);
            }
            else toremove.Add(t);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.GetComponent<Ship>() && (!target || Vector3.Distance(target.position,transform.position)>Vector3.Distance(col.transform.position,transform.position)))target=col.transform;
        Debug.Log(target);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform==target)target=null;
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position;
        go.transform.localScale=Vector3.one*2;
        Missile bu=go.AddComponent<Missile>();
        go.AddComponent<SpriteRenderer>().sprite=missile;
        go.AddComponent<BoxCollider2D>();
        go.transform.up=transform.position-target.position;
        timer=0.5f;
        shots--;
        missiles.Add(go.transform);
    }
}
