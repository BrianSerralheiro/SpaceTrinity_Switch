using UnityEngine;
public class ChopperD : EnemyBase
{
    Helix helix;
	public override void SetSprites(EnemyInfo ei)
    {
        transform.localScale = Vector3.one * 0.8f;
        hp=10;
        GameObject go=new GameObject("Helix");
        go.transform.parent=transform;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        helix=go.AddComponent<Helix>();
        fallSpeed=-8;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        SlowFall();
    }
    
    protected override void Die()
    {
        if(hp<=0 && Random.value < 0.2)
        {
            helix.transform.parent=null;
            helix.time=Time.time+1;
        }
        base.Die();
    }
}
