using UnityEngine;

public class Dasher : EnemyBase
{
    float maxSpeed=-18;
    static int trailID;
    public override void SetSprites(EnemyInfo ei)
    {
        hp=50;
        fallSpeed=-12;
        trailID=ei.particleID[0];
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        ParticleManager.Emit(trailID,transform.position+transform.up,1);
        SlowFall();
        fallSpeed=Mathf.MoveTowards(fallSpeed,maxSpeed,Time.deltaTime*2);
    }
}
