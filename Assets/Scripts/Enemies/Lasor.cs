using UnityEngine;

public class Lasor : EnemyBase
{
	private float timer=3;
	private Vector3 dir;
	private Transform laser;
	private SpriteRenderer charge;
	private new BoxCollider2D collider;
	private Core core;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=30;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 120;
		GameObject go = new GameObject("charge");
		go.transform.localScale=new Vector3();
		charge=go.AddComponent<SpriteRenderer>();
		charge.sprite=ei.sprites[1];
		go.transform.parent=transform;
		go.transform.localScale=new Vector3();
		go = new GameObject("laserbase");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		go.transform.parent=transform;
		go.transform.localScale=new Vector3();
		laser=go.transform;
		laser.localPosition=charge.transform.localPosition=new Vector3();
		go = new GameObject("enemylaser");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
		collider=go.AddComponent<BoxCollider2D>();
		collider.enabled=false;
		go.transform.parent=laser;
		go.transform.localPosition=new Vector3(0,-1.1f,0);
		go.transform.localScale=Vector3.right+Vector3.up*50;
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
				if(dir.x<=0){
					timer=3;
					dir.x=0;
					collider.enabled=false;
				}
				laser.localScale=dir;
			}
			else if(timer<1)
			{
				charge.transform.localScale=laser.localPosition;
				dir.Set(1-timer,1,1);
				core.Set(timer);
				laser.localScale=dir;
				collider.enabled=!collider.enabled;
			}else if(timer<2)
			{
				laser.localScale=laser.localPosition;
				Color c=charge.color;
				c.a=2f-timer;
				charge.color=c;
				dir.Set(1,(timer-1)*5,1);
				charge.transform.localScale=dir;
				core.Set(2f-timer);
			}
			transform.Translate(0,-Time.deltaTime,0,Space.World);
			if(transform.position.y<-Scaler.sizeY-1)Die();
		}
	}
}
