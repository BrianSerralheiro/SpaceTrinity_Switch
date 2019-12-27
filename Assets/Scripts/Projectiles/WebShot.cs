using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour
{
    private Vector3 target;
    private float speed,timer, maxScale;
    private int spriteID;
    private string owner;
    new private SpriteRenderer renderer;
    private Dictionary<Transform,Vector3> glued=new Dictionary<Transform, Vector3>();
    void Start()
    {
        
    }
    public void Set(int p,Vector3 v,float s,float t,float m,float size,string o){
        target=v;
        speed=s;
        timer=Time.time+t;
        maxScale=m;
        transform.localScale=Vector3.one*size;
        renderer=gameObject.AddComponent<SpriteRenderer>();
        spriteID=p;
        renderer.sprite=Bullet.sprites[p];
        owner=o;
        gameObject.AddComponent<BoxCollider2D>().isTrigger=true;
    }
    void Update()
    {
    	if(Bullet.bulletTime<=0)renderer.sprite=Bullet.sprites[Bullet.blink?spriteID:spriteID+1];
        if(transform.position==target){
            foreach (Transform t in glued.Keys)
            {
                t.position=glued[t];
            }
            transform.localScale=Vector3.MoveTowards(transform.localScale,Vector3.one*maxScale,Time.deltaTime*maxScale*2);
        }
        else {
            transform.position=Vector3.MoveTowards(transform.position,target,Time.deltaTime*speed);
            transform.Rotate(0,0,45*Time.deltaTime);
        }
        if(Time.time>timer)Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name==owner)return;
        if(col.name.Contains("enemy"))return;
        target=transform.position;
        if(!glued.ContainsKey(col.transform))glued.Add(col.transform,col.transform.position);
    }
}
