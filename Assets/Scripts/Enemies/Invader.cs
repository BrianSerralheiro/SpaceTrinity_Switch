using UnityEngine;
using System.Collections.Generic;

public class Invader : EnemyBase {
	private bool left;
	int prev=1,cicles=2,shots,moves;
	private Vector3 position;
	List<Transform> invaders=new List<Transform>();
	static  Vector3 vec=Vector3.left/10,mod=Vector3.forward/10;
	private float shoottimer = 1;
	private static int shootId,trailID,impactID;
	Del  movement;
	static Vector3[] dir={Vector3.left,Vector3.down,Vector3.right};
	public override void SetSprites(EnemyInfo ei)
	{
		SetHP(50,ei.lifeproportion);
		hp=20;
		shootId=ei.bulletsID[0];
		Instantiate(ei.particles[0],transform);
		trailID=ei.particleID[1];
		impactID=ei.particleID[2];
		fallSpeed=-4;
		movement=Pathing;
		CircleCollider2D cir=gameObject.AddComponent<CircleCollider2D>();
		cir.radius=3;
		cir.isTrigger=true;
	}
	public override void Position(int i){
		base.Position(i);
		position=transform.position+dir[1]*3;
	}
	void Shoot()
	{
		//SoundManager.PlayEffects(12, 0.1f, 0.5f);
		if(shoottimer>Time.time)return;
		shoottimer=Time.time+0.5f;
		GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shootId];
		go.AddComponent<CircleCollider2D>().radius=0.3f;
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shootId;
		b.particleID=trailID;
		b.impactID=impactID;
		go.transform.position=transform.position+vec*(left?1:-1)+mod;
		go.transform.up=-transform.up;
		left=!left;
		if(++shots==4){
			movement=Pathing;
			shots=0;
			cicles--;
		}
	}
	void Pathing(){
		transform.position=Vector3.MoveTowards(transform.position,position,Time.deltaTime*3);
		if(transform.position==position){
			if(moves++>2){
				movement=Shoot;
				moves=0;
				return;
			}
			int i=0;
			while (true)
			{
				i=Random.Range(0,3);
				if(position.x<-Scaler.sizeX/2+4 &&  i==0)continue;
				if(position.x>Scaler.sizeX/2-4 &&  i==2)continue;
				foreach (Transform t in invaders)
				{
					if(i==0 && t.position.x<transform.position.x){
						prev=0;
						i=1;
						break;
					}
					if(i==2 && t.position.x>transform.position.x){
						prev=0;
						i=1;
						break;
					}
				}
				if(i!=prev){
					prev=i;
					position+=dir[i]*3;
					break;
				}
			}
			if(cicles<=0)movement=SlowFall;
		}
	}
	new void Update () {
		if(Ship.paused)return;
		base.Update();
		movement?.Invoke();
	}
	void OnTriggerExit2D(Collider2D col)
	{
		invaders.Remove(col.transform);
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.GetComponent<Invader>())invaders.Add(col.transform);
	}
}
