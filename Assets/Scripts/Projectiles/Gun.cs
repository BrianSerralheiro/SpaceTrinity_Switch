using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	[SerializeField]
	protected int level;
	[SerializeField]
	protected int spriteID;
	[SerializeField]
	private int damage=1;
	[SerializeField]
	private bool pierce;

	[SerializeField]
	private int particleID;

	public bool minusPower;

	public virtual void Shoot()
	{
		if(!gameObject.activeSelf)return;
		ParticleManager.Emit(17,transform.position,1);
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.bullets[spriteID+(Bullet.blink ? 0 : 1)];
		go.AddComponent<BoxCollider2D>();
		Bullet bull= go.AddComponent<Bullet>();
		bull.owner=transform.parent.name;
		bull.damage=damage;
		bull.pierce=pierce;
		bull.particleID=particleID;
		bull.spriteID=spriteID;
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
