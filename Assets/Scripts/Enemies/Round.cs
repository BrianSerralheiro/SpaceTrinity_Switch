using UnityEngine;

public class Round : EnemyBase
{

	private float shoottimer = 1.4f;
	private static int shootId;

	private int shootCount;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=30;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 150;
		shootId=ei.bulletsID[0];
	}

	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		shoottimer-=Time.deltaTime;
		transform.Translate(0,-Time.deltaTime * 2,0,Space.World);
		if(transform.position.y<-Scaler.sizeY-1)Destroy(gameObject);
		if(!stopMovement)transform.rotation=Quaternion.Euler(0,0,shoottimer/0.2f*90f);
		if(shoottimer<=0)
		{
			SoundManager.PlayEffects(12, 0.1f, 0.5f);
			shootCount++;
			if(shootCount > 3)
			{
				shootCount = 0;
				shoottimer = 0.6f;
			}
			else
			{
				shoottimer = 0.2f;
			}
			
			for(int i=0;i<4;i++){
				Shoot(i);
			}
		}
	}
	void Shoot(int i)
	{
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<CircleCollider2D>();
		Bullet bu = go.AddComponent<Bullet>();
		bu.owner=transform.name;
		bu.spriteID=shootId;
		Vector3 v= new Vector3(i%2,i/2,0) *2 -Vector3.one;
		go.transform.position=transform.position+v;
		go.transform.eulerAngles=new Vector3(0,0,135+i*90)*(i<2?1:-1);
	}

}
