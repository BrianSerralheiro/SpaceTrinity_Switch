using UnityEngine;
using System.Collections.Generic;

public class LaserGun : Gun {
	[SerializeField]
	private int damageByLevel,ticks;
	private float second,half;
	[SerializeField]
	private Texture[] lasers;
	[SerializeField]
	private Material lasermaterial;
	private float timer;
	PolygonCollider2D col;
	[SerializeField]
	private AudioSource source;
	private LineRenderer line;
	
	public override void Load(int i){
		particleID=ParticleManager.Register(shotParticle);
	}
	void Start () {
		if(Ship.skinID[2]!=-1 && Locks.Skin(6+Ship.skinID[2]))lasermaterial.mainTexture=lasers[Ship.skinID[2]+1];
		else lasermaterial.mainTexture=lasers[0];
		lasers=null;
		second=1f/ticks;
		half=second/2f;
		line=GetComponent<LineRenderer>();
		Bullet b=transform.parent.GetComponent<Bullet>();
		b.owner=transform.parent.name;
		b.damage=damage;
		col=gameObject.AddComponent<PolygonCollider2D>();
	}
	public override void Shoot()
	{
		col.enabled=Time.time%second>half;
		line.enabled=true;
		float f=4.5f/line.positionCount;
		float f1=40f;
		float t=Time.time*10;
		float t1=Time.time*40;
		List<Vector2> list=new List<Vector2>(),left=new List<Vector2>();
		list.Add(line.GetPosition(0));
		for(int i = 1; i<line.positionCount-1; i++)
		{
			Vector3 v=line.GetPosition(i);
			v.x=Mathf.Sin(t+f*i)*level*0.6f+Mathf.Cos(t1+f1*i)*0.4f;
			line.SetPosition(i,v);
			left.Add(new Vector2(v.x-0.5f*level,v.y));
			list.Add(new Vector2(v.x+0.5f*level,v.y));
			if(Random.value<0.01f)ParticleManager.Emit(particleID,v+transform.position,1);
		}
		for (int i = 0; i < left.Count; i++)
		{
			list.Add(left[left.Count-i-1]);
		}
		col.SetPath(0,list);

		timer=0.1f;
	}
	public override void Level(int i)
	{
		if(i<4){
			line.widthMultiplier=i;
			level=i;
			transform.parent.GetComponent<Bullet>().damage=damage+damageByLevel*(i-1);
		}
	}
	void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.otherCollider==col)ParticleManager.Emit(particleID,collision.contacts[0].point,1);
	}
	new void Update()
	{
		if(timer>0)
			timer-=Time.deltaTime;
		if(timer<0){
			line.enabled=false;
			col.enabled=false;
		}

	}
}
