using UnityEngine;

public class Gun : MonoBehaviour {
	[SerializeField]
	protected int level;
	[SerializeField]
	protected Sprite[] shots;
	protected int shotId;
	[SerializeField]
	protected int damage=1;
	[SerializeField]
	protected bool pierce;

	[SerializeField]
	protected int particleID;

	public bool minusPower;
	void Awake()
	{
		shotId=Bullet.Register(shots[(Ship.skinID+1)*2+0]);
		Bullet.Register(shots[(Ship.skinID+1)*2+1]);
		shots=null;
	}
	public virtual void Shoot()
	{
		if(!gameObject.activeSelf)return;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId+(Bullet.blink ? 0 : 1)];
		go.AddComponent<BoxCollider2D>();
		Bullet bull= go.AddComponent<Bullet>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=pierce;
		bull.particleID=particleID;
		bull.spriteID=shotId;
		go.transform.position=transform.position;
		go.transform.rotation=transform.rotation;
	}
	public virtual void Level(int i)
	{
		if(minusPower && i==1)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(level<=i);
		}
	}
}
