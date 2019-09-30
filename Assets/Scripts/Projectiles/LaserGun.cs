using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun {
	[SerializeField]
	private Sprite[] lasersprites;
	private Sprite[] lasersprite=new Sprite[4];
	private float timer;
	private BoxCollider2D col;
	[SerializeField]
	private AudioSource source;
	private LineRenderer line;
	

	void Start () {
		int f=0;
		if(Ship.skinID!=-1 && Locks.Skin(6+Ship.skinID))f=(Ship.skinID+1)*4;
		for(int i = 0; i<lasersprite.Length; i++)
		{
			lasersprite[i]=lasersprites[f+i];
		}
		lasersprites=null;
		line=GetComponent<LineRenderer>();
		col=gameObject.AddComponent<BoxCollider2D>();
		col.offset=new Vector2(0,10);
		col.size=new Vector2(1,20);
	}
	public override void Shoot()
	{
		col.enabled=!col.enabled;
		if(col.enabled){
			line.enabled=true;
			float f=4.5f/line.positionCount;
			float f1=40f;
			float t=Time.time*10;
			float t1=Time.time*40;
			for(int i = 1; i<line.positionCount-1; i++)
			{
				Vector3 v=line.GetPosition(i);
				v.x=Mathf.Sin(t+f*i)*0.6f+Mathf.Cos(t1+f1*i)*0.4f;
				line.SetPosition(i,v);
			}
		}
		timer+=Time.deltaTime*10;
		if(timer>1)timer=1;
		if(timer<0)timer=0;
	}
	public override void Level(int i)
	{
		if(i<4) line.widthMultiplier=i;
	}
	void Update()
	{
		if(timer>0)
			timer-=Time.deltaTime*6f;
		if(timer<0)line.enabled=false;

	}
}
