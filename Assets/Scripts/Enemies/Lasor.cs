using UnityEngine;

public class Lasor : EnemyBase
{
	private float timer=1.5f;
	private Vector3 dir;
	private new BoxCollider2D collider;
	private Core core;
	ParticleSystem charge,laser;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=30;
		fallSpeed=-1;
		charge=Instantiate(ei.particles[0]);
		laser=Instantiate(ei.particles[1]);
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 120;
		charge.transform.parent=transform;
		GameObject go = laser.gameObject;
		go.name="enemylaser";
		collider=go.AddComponent<BoxCollider2D>();
		collider.enabled=false;
		collider.size=new Vector2(2,30);
		collider.offset=new Vector2(0,-15);
		go.transform.parent=transform;
		go.transform.localPosition=new Vector3();
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
				dir.x=1+timer;
				collider.enabled=!collider.enabled;
				if(timer<-1){
					timer=3;
					collider.enabled=false;
					laser.gameObject.SetActive(false);
				}
			}
			else if(timer<1)
			{
				dir.Set(1-timer,1,1);
				laser.gameObject.SetActive(true);
				charge.gameObject.SetActive(false);
				core.Set(timer);
				collider.enabled=!collider.enabled;
			}else if(timer<2)
			{
				charge.gameObject.SetActive(true);
				core.Set(2f-timer);
			}
			SlowFall();
		}
	}
}
