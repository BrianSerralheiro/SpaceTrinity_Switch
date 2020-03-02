using UnityEngine;

public class BatAlly : MonoBehaviour 
{
	private Transform wingL,wingR,enemy;
	public Transform player;
	private Vector3 vector=new Vector3();
	public Vector3 target=new Vector3();
	SpriteRenderer _renderer;
	float damageTimer;
	public int hp;
	public void SetSprites(EnemyInfo ei)
	{
		_renderer=gameObject.AddComponent<SpriteRenderer>();
		_renderer.sprite=ei.sprites[0];
		gameObject.AddComponent<BoxCollider2D>();
		CircleCollider2D cir=gameObject.AddComponent<CircleCollider2D>();
		cir.radius=10;
		cir.isTrigger=true;
		Rigidbody2D rb2=gameObject.AddComponent<Rigidbody2D>();
		rb2.isKinematic=true;
		rb2.useFullKinematicContacts=true;
		GameObject go=new GameObject("wingL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		wingL=go.transform;
		go = new GameObject("wingR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		wingR=go.transform;
		wingL.parent=wingR.parent=transform;
		wingL.localPosition=new Vector3(0.2f,-0.2f,0.1f);
		wingR.localPosition=new Vector3(-0.2f,-0.2f,0.1f);
	}
	void Update () 
	{
		if(Ship.paused) return;
		if(damageTimer > 0)
		{
			damageTimer -= Time.deltaTime;
			_renderer.color = Color.Lerp(Color.white,Color.red,damageTimer);
		}
		vector.Set(0,0,Mathf.PingPong(Time.time*300,60f)-60f);
		wingL.eulerAngles=vector;
		wingR.eulerAngles=-vector;
		Vector3 pos=transform.position;
		pos=Vector3.MoveTowards(pos,target,Time.deltaTime*3);
		if(target==pos){
			if(enemy){
				target=pos+(enemy.position-pos).normalized*3;
			}
			else
				target=pos+(player.position-pos).normalized*5;
		}
		transform.position=pos;
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if(!enemy &&  col.name.Contains("enemy") && !col.name.Contains("bullet") && !col.name.Contains("laser")){
			enemy=col.transform;
			target=transform.position+(enemy.position-transform.position).normalized*3;
		}
	}
	public void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.name.Contains("Player"))return;
		if(--hp<=0){
			ParticleManager.Emit(1,col.collider.transform.position,1);
			Destroy(gameObject);
		}
		damageTimer = 1;

	}

}
