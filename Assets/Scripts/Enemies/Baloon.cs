using UnityEngine;

public class Baloon : EnemyBase
{
    public int rotation;
    Vector3 scale=Vector3.one*0.9f;
    static int puffID;
    
	public override void SetSprites(EnemyInfo ei)
	{
        hp=20;
        points=20;
        puffID=ei.particleID[0];
    }
    new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
        transform.Translate(0,-Time.deltaTime*4,0);
        if(rotation>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,rotation*Time.deltaTime);
        transform.localScale=Vector3.MoveTowards(transform.localScale,scale,Time.deltaTime * 0.5f);
        if(transform.localScale==scale){
            if(scale==Vector3.one*0.9f)scale=Vector3.one*1.3f;
            else scale=Vector3.one*0.9f;
            ParticleManager.Emit(puffID,transform.position+transform.up,1);
        }
        if(transform.position.y<-Scaler.sizeY-2)Die();
    }
}
