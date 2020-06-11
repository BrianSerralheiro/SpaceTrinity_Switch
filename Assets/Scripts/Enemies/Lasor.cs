using UnityEngine;

public class Lasor : EnemyBase
{
	private float timer=1.5f;
	private Vector3 dir;
	private new BoxCollider2D collider;
	private Core core;
	GameObject laser;
	public override void SetSprites(EnemyInfo ei)
	{
		SetHP(30,ei.lifeproportion);
		fallSpeed=-1;
		laser=Instantiate(ei.particles[0].gameObject,transform);
		points = 120;
		GameObject go = laser.gameObject;
		go.name="enemylaser";
		collider=go.AddComponent<BoxCollider2D>();
		collider.enabled=false;
		collider.size=new Vector2(2,30);
		collider.offset=new Vector2(0,-15);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[4],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,1.86f);
	}
	public override void Position(int i){
		base.Position(i);
		if(i%2==1)dir=new Vector3(0,0,90);
		else dir=new Vector3(0,0,-90);
	}
	new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="enemylaser") return;
		base.OnCollisionEnter2D(col);
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		if(transform.position.y>Scaler.sizeY/1.3f){
			transform.Translate(0,-Time.deltaTime*2,0,Space.World);
			transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(dir),Time.deltaTime*45);
		}
		else{
			timer-=Time.deltaTime;
			if(timer<0){
				collider.enabled=!collider.enabled;
				if(timer<-1){
					timer=3;
					collider.enabled=false;
				}
			}
			else if(timer<1)
			{
				core.Set(timer);
				collider.enabled=!collider.enabled;
			}else if(timer<2)
			{
				laser.SetActive(true);
				core.Set(2f-timer);
			}
			SlowFall();
		}
	}
}
