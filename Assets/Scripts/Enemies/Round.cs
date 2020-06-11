using UnityEngine;

public class Round : EnemyBase
{

	private float shoottimer = 1.4f;
	public static int shootId,trailID,impactID;
	public static Material shotMaterial;

	private int shootCount;
	public override void SetSprites(EnemyInfo ei)
	{
		SetHP(30,ei.lifeproportion);
		points = 150;
		shootId=ei.bulletsID[0];
		trailID = ei.particleID[0];
		impactID = ei.particleID[1];
		if(!shotMaterial)
		{
			shotMaterial = ei.material;
		}
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
		SpriteRenderer r = go.AddComponent<SpriteRenderer>(); 
		r.sprite=Bullet.sprites[shootId];
		r.material = shotMaterial;
		r.color = new Color(0.5f,0.5f,0.5f);
		go.AddComponent<CircleCollider2D>();
		Bullet bu = go.AddComponent<Bullet>();
		bu.particleID=trailID;
		bu.impactID=impactID;
		bu.owner=transform.name;
		bu.spriteID=shootId;
		Vector3 v= new Vector3(i%2,i/2,0) *2 -Vector3.one;
		go.transform.position=transform.position+v;
		go.transform.rotation=Quaternion.Euler(0,0,((1-i/2)*90+45)*(i%2==0?1:-1));
	}

}
