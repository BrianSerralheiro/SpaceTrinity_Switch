using UnityEngine;

public class Ascended : EnemyBase
{
    
    public override void SetSprites(EnemyInfo ei)
	{
        hp=40;
        points=60;
        transform.localScale=new Vector3(1,-1,1);
        fallSpeed=-6;
    }

    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>-Scaler.sizeY-1)SlowFall();
        else{
            transform.localScale=Vector3.MoveTowards(transform.localScale,Vector3.one,Time.deltaTime*5);
            if(transform.localScale==Vector3.one){
                fallSpeed=12;
                SlowFall();
            }
        }
        if(transform.position.y>Scaler.sizeY+3)Die();
    }
}
