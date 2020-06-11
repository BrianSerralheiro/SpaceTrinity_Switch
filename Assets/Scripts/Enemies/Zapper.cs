using UnityEngine;

public class Zapper : EnemyBase {
	private float timer = 5;
	GameObject zap;
	new BoxCollider2D collider2D;
	private Vector3 rot = new Vector3();
	public override void SetSprites(EnemyInfo ei)
	{
		points = 150;
		SetHP(80,ei.lifeproportion);
		fallSpeed=-1;
		zap=Instantiate(ei.particles[0],transform).gameObject;
		collider2D=zap.AddComponent<BoxCollider2D>();
		collider2D.size=new Vector2(1f,17);
		collider2D.offset=new Vector2(0,8f);
		collider2D.enabled=false;
		// zap.transform.localPosition=new Vector3(0,2,0.1f);
		
	}
	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider==collider2D) return;
		base.OnCollisionEnter2D(col);
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		if(transform.position.y<-Scaler.sizeY/2){
			transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,10*Time.deltaTime);
			SlowFall();
			return;
		}
		timer-=Time.deltaTime;
		if(timer>1){
			SlowFall();
			Vector3 v=GetPlayer(transform.position).position-transform.position;
            v.z=0;
            v=Vector3.Cross(transform.up,v);
            transform.Rotate(v*Time.deltaTime*15);
		}else if(timer >0.1f)
		{
			zap.SetActive(true);
		}else if(timer >0)
		{
			collider2D.enabled=true;
		}
		else
		{
			timer=5;
			collider2D.enabled=false;
		}
	}
}
