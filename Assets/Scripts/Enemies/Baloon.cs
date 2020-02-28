using UnityEngine;

public class Baloon : EnemyBase
{
    public int rotation;
    Vector3 scale=Vector3.one*0.9f;
    
	public override void SetSprites(EnemyInfo ei)
	{
        hp=30;
        points=50;
    }
    new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
        transform.Translate(0,-Time.deltaTime*2,0);
        if(rotation>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,rotation*Time.deltaTime);
        transform.localScale=Vector3.MoveTowards(transform.localScale,scale,Time.deltaTime/10);
        if(transform.localScale==scale){
            if(scale==Vector3.one*0.9f)scale=Vector3.one*1.1f;
            else scale=Vector3.one*0.9f;
        }
        if(transform.position.y<-Scaler.sizeY-2)Die();
    }
}
