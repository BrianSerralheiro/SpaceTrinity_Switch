using UnityEngine;

public class Ascended : EnemyBase
{
    public override void SetSprites(EnemyInfo ei)
	{
        SetHP(10,ei.lifeproportion);
        points=60;
        transform.localScale=new Vector3(1,-1,1);
        fallSpeed=-6;
        Instantiate(ei.particles[0],transform);
        GameObject go =new GameObject("col");
        go.transform.SetParent(transform,false);
        CapsuleCollider cap=go.AddComponent<CapsuleCollider>();
        cap.height=4;
        cap.radius=1f;
        cap.direction=1;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.position.y>-Scaler.sizeY-1)SlowFall();
        else{
            transform.localScale=Vector3.MoveTowards(transform.localScale,Vector3.one,Time.deltaTime*5);
            if(transform.localScale==Vector3.one){
                fallSpeed=8;
                SlowFall();
            }
        }
        if(transform.position.y>Scaler.sizeY+3)Die();
    }
}
