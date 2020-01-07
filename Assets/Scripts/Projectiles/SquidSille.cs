using UnityEngine;

public class SquidSille : EnemyBase
{
    public Vector3 target;
    static int shotId;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=15;
        shotId=ei.bulletsID[0];
    }
    
	public override void Position(int i){
		base.Position(i);
        target.Set(transform.position.x, -Scaler.sizeY/2, 0.1f);
        transform.up=Vector3.down;
    }
	void Shot(int c){
		SoundManager.PlayEffects(12, 0.5f, 0.8f);
		float degrees=360f/c;
		for (int i = 0; i < c; i++)
		{
			GameObject go = new GameObject("enemybullet");
			go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
			go.AddComponent<CircleCollider2D>();
			Bullet bu=go.AddComponent<Bullet>();
			bu.owner=name;
			bu.spriteID=shotId;
			bu.bulleSpeed=10;
			bu.Timer(0.1f);
			go.transform.position=transform.position;
			go.transform.eulerAngles=new Vector3(0,0,degrees*i);
		}
	}
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Rotate(Vector3.Cross(transform.up,target - transform.position)*Time.deltaTime*60);
        transform.position=Vector3.MoveTowards(transform.position,target,Time.deltaTime*8);
        if(transform.position == target)
        {
            Shot(12);
            Die();
        }
    }
}
