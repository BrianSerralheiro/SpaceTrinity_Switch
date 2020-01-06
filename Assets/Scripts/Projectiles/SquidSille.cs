using UnityEngine;

public class SquidSille : EnemyBase
{
    public Vector3 target;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=15;
    }
    
	public override void Position(int i){
		base.Position(i);
        transform.up=Vector3.down;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Rotate(Vector3.Cross(transform.position,target)*Time.deltaTime*60);
        transform.position=Vector3.MoveTowards(transform.position,target,Time.deltaTime*8);
    }
}
