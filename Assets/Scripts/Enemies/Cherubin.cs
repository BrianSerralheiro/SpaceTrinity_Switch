using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherubin : EnemyBase
{
    public override void SetSprites(EnemyInfo ei)
	{
        hp=40;
        points=60;
    }

    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        
    }
}
