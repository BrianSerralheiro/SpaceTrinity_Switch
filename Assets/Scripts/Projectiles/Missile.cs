using UnityEngine;

public class Missile : EnemyBase {
	public bool release;
	public float time;
	public override void SetSprites(EnemyInfo ei)
	{
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.name.Contains("Player"))
		{
			Destroy(gameObject);
		}
		else 
		base.OnCollisionEnter2D(col);
	}
	new void Update()
	{
		if(Ship.paused) return;
		base.Update();
		if(release){
			transform.Translate(0,Time.deltaTime*8,0);
			if(transform.position.x<-Scaler.sizeX/2-2 || transform.position.x>Scaler.sizeX/2+2 || transform.position.y<-Scaler.sizeY-2 || transform.position.y>Scaler.sizeY+2)Destroy(gameObject);
		}
	}
	void OnDestroy()
	{
		ParticleManager.Emit(0,transform.position,1);
	}
}
