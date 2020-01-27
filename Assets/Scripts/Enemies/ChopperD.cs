using UnityEngine;
public class ChopperD : EnemyBase
{
    Helix helix;
	public override void SetSprites(EnemyInfo ei){
        hp=10;
        GameObject go=new GameObject("Helix");
        go.transform.parent=transform;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        helix=go.AddComponent<Helix>();
        go.AddComponent<BoxCollider2D>();
        fallSpeed-=10;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        SlowFall();
    }
    
    protected override void Die(){
        if(hp<=0){
            helix.transform.parent=null;
            Bullet b=helix.gameObject.AddComponent<Bullet>();
            b.enabled=false;
            b.damage=40;
            b.owner="hel";
            helix.time=Time.time+1;
        }
        base.Die();
    }
}
