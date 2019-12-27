using UnityEngine;

public class LaserGun : Gun {
	[SerializeField]
	private int damageByLevel,ticks;
	private float second,half;
	[SerializeField]
	private Texture[] lasers;
	[SerializeField]
	private Material lasermaterial;
	private float timer;
	private BoxCollider2D col;
	[SerializeField]
	private AudioSource source;
	private LineRenderer line;
	
	public override void Load(int i){}
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
		col=gameObject.AddComponent<BoxCollider2D>();
		col.offset=new Vector2(0,10);
		col.size=new Vector2(1,20);
	}
	public override void Shoot()
	{
		col.enabled=Time.time%second>half;
		line.enabled=true;
		float f=4.5f/line.positionCount;
		float f1=40f;
		float t=Time.time*10;
		float t1=Time.time*40;
		for(int i = 1; i<line.positionCount-1; i++)
		{
			Vector3 v=line.GetPosition(i);
			v.x=Mathf.Sin(t+f*i)*level*0.6f+Mathf.Cos(t1+f1*i)*0.4f;
			line.SetPosition(i,v);
		}
		timer=0.1f;
	}
	public override void Level(int i)
	{
		if(i<4){
			line.widthMultiplier=i;
			Vector2 v= col.size;
			v.x=i;
			col.size=v;
			level=i;
			transform.parent.GetComponent<Bullet>().damage=damage+damageByLevel*(i-1);
		}
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
