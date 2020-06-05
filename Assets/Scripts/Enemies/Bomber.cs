using UnityEngine;

public class Bomber : EnemyBase {
	private float timer=4;
	private Vector3 local=new Vector3(-7f,-0.2f);
	private static Sprite bomb;
	static int explosionID,spawnID;
	static GameObject trail;
	public override void SetSprites(EnemyInfo ei)
	{
		points = 120;
		name+="big";
		hp=140;
		explosionID=ei.particleID[0];
		spawnID=ei.particleID[1];
		if(!trail)trail=ei.particles[2].gameObject;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		bomb=ei.sprites[1];
	}

	public override void Position(int i)
	{
		base.Position(i);
		transform.Rotate(0,0,-90f);
	}
	new void Update () {
		if(Ship.paused) return;
		base.Update();
		transform.Translate(Time.deltaTime,0,0);
		timer-=Time.deltaTime;
		if(timer<0)Bomb();
		if(transform.position.x<-Scaler.sizeX/2f-4.2F || transform.position.x>Scaler.sizeX/2f+4.2F || transform.position.y<-Scaler.sizeY-8.4F) Die();

	}
	void Bomb()
	{
		timer=2;
		GameObject go = new GameObject("enemy");
		Instantiate(trail,go.transform);
		go.AddComponent<SpriteRenderer>().sprite=bomb;
		go.transform.parent=transform;
		go.transform.localPosition=local;
		go.transform.parent=null;
		ParticleManager.Emit(spawnID,go.transform.position,1);
		go.AddComponent<Bomb>().Set(5,explosionID);
	}

	protected override void Die()
	{
		killed=true;
		base.Die();
		if(hp<=0)ParticleManager.Emit(1,transform.position-transform.right*3,3,1);
	}
}