using UnityEngine;

public class Zep : EnemyBase
{
    static EnemyInfo balloon;
    float time;
    int spawn=6;
    static int puffID;
    Vector3 scale=Vector3.one*0.9f;
    
    public override void SetSprites(EnemyInfo ei)
	{
        hp=80;
        points=150;
        name+="big";
        fallSpeed=-2;
        puffID=ei.particleID[0];
        time=Time.time+2;
        if(!balloon)balloon=((CarrierInfo)ei).spawnable;
    }

    new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
        SlowFall();
        transform.localScale=Vector3.MoveTowards(transform.localScale,scale,Time.deltaTime/10);
        if(transform.localScale==scale){
            if(scale==Vector3.one*0.9f)scale=Vector3.one*1.1f;
            else scale=Vector3.one*0.9f;
            ParticleManager.Emit(puffID,transform.position+transform.up*2+transform.right,1);
            ParticleManager.Emit(puffID,transform.position+transform.up*2-transform.right,1);
        }
        if(time<Time.time && spawn>0)Spawn();
    }
    void Spawn(){
        time=Time.time+1;
        for (int i = 0; i < 2; i++)
        {
            GameObject go=new GameObject("enemy");
            go.AddComponent<SpriteRenderer>().sprite=balloon.sprites[0];
            go.AddComponent<BoxCollider2D>();
            Rigidbody2D rb=go.AddComponent<Rigidbody2D>();
            rb.isKinematic=true;
            rb.useFullKinematicContacts=true;
            Baloon bal=go.AddComponent<Baloon>();
            bal.SetSprites(balloon);
            bal.rotation=(7-spawn)*30;
            go.transform.rotation=Quaternion.Euler(0,0,-90+i*180);
            go.transform.position=transform.position+new Vector3(-2+i*4,0,0.1f);
        }
        spawn--;
    }
}
