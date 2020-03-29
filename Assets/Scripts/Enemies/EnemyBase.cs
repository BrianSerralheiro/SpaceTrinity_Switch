using UnityEngine;

public class EnemyBase : MonoBehaviour {
	protected int points;
	protected int hp=8;
	protected int killerid=1;
	public static Transform[] players;
	protected float damageTimer,fallSpeed=-1;
	protected SpriteRenderer _renderer;
	public bool stopMovement;

	protected bool damageEffect,killed;
	protected delegate void Del();

	public virtual void SetSprites(EnemyInfo ei){}
	void Start(){
		_renderer = GetComponent<SpriteRenderer>();
	}
	public void SetHP(int i)
	{
		hp=i;
	}
	protected static Transform GetPlayer(Vector3 v){
		if(players[1]==null || !players[1].gameObject.activeSelf)
			return players[0];
		if(!players[0].gameObject.activeSelf)return players[1];
		if(Vector3.Distance(players[0].position,v)<Vector3.Distance(players[1].position,v))return players[0];
		return players[1];
	}
	protected static Transform GetPlayer(){
		if(players[1]==null)return players[0];
		if(players[0].gameObject.activeSelf && players[1].gameObject.activeSelf)
			return players[Random.Range(0,2)];
		if(!players[0].gameObject.activeSelf)return players[1];
		return players[0];
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
		if(col.gameObject.name.Contains("enemybullet")
		|| col.gameObject.name.Contains("enemy")
		|| col.gameObject.name.Contains("drone")
		|| col.gameObject.name.Contains("Player") && name.Contains("gr")
		|| (transform.position.y > Scaler.sizeY && !name.Contains("Boss"))
		|| hp<=0) return;
		int i=1;
		Bullet bull=col.gameObject.GetComponent<Bullet>();
		if(bull){
			i=bull.damage;
			int.TryParse(bull.owner[7].ToString(),out killerid);
		}
		ParticleManager.Emit(2,col.collider.transform.position,1);
		hp-=i;
		if(hp<=0)Die();
		// killerid=1;
		if(!damageEffect || damageTimer <= 0)
		{
			damageTimer = 1;
		}
	}
	public void Kill(int i){
		killerid=i;
		hp=0;
		killed=true;
		Die();
	}
	protected virtual void Die()
	{
		Destroy(gameObject);
		if(hp<=0)
		{
			if(killerid>0){
				InGame_HUD.special[killerid-1] += 0.01f;
				EnemySpawner.points[killerid-1]+=points;
			}
			if(!killed){
				SoundManager.PlayEffects(15, 0.8f, 1.2f);
				ParticleManager.Emit(1, transform.position,1);
			}
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
