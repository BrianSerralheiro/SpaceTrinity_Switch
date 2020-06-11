using UnityEngine;

public class Dasher : EnemyBase
{
    float maxSpeed=-18;
    public override void SetSprites(EnemyInfo ei)
    {
        SetHP(20,ei.lifeproportion);
        points=60;
        fallSpeed=-6;
        Instantiate(ei.particles[0],transform).transform.localPosition=new Vector3(0,2);
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        SlowFall();
        fallSpeed=Mathf.MoveTowards(fallSpeed,maxSpeed,Time.deltaTime*4);
    }
}
