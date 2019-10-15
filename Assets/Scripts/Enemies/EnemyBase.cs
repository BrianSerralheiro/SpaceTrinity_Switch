using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {
	protected int points;
	protected int hp=8;
	public static Transform player;
	protected float damageTimer,fallSpeed=-1;
	protected SpriteRenderer _renderer;

	protected bool damageEffect;
	protected delegate void Del();
	protected int explosionID;

	public abstract void SetSprites(EnemyInfo ei);
	void Start(){
		_renderer = GetComponent<SpriteRenderer>();
	}
	public void SetHP(int i)
	{
		hp=i;
	}
	protected virtual void SlowFall(){
		transform.Translate(0,fallSpeed*Time.deltaTime,0,Space.World);
		if(transform.position.y<-Scaler.sizeY-2)Die();
	}
	public void Update()
	{
		if(Ship.paused) return;
		if(damageTimer > 0)
		{
			damageTimer -= Time.deltaTime;
			_renderer.color = Color.Lerp(Color.white,Color.red,damageTimer);
		}
	}
	public void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.name=="enemybullet") return;
		if(col.gameObject.name=="enemy") return;
		int i=1;
		Bullet bull=col.gameObject.GetComponent<Bullet>();
		if(bull)i=bull.damage;
		ParticleManager.Emit(1,col.collider.transform.position,1);
		hp-=i;
		if(hp<=0)Die();
		if(!damageEffect || damageTimer <= 0)
		{
			damageTimer = 1;
		}
	}
	protected virtual void Die()
	{
		
		Destroy(gameObject);
		if(hp<=0)
		{
			SoundManager.PlayEffects(15, 0.8f, 1.2f);
			EnemySpawner.points+=points;
			InGame_HUD._special += 0.01f;
			ParticleManager.Emit(explosionID, transform.position,1);
		}
	}
	public virtual void Position(int i)
	{
		float f= Scaler.sizeX/20;
		transform.position=new Vector3(i*f-f*9f,Scaler.sizeY+2,0);
	}
	private void OnDestroy()
	{
		
	}
}
