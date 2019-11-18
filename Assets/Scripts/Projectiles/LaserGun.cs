using UnityEngine;

public class LaserGun : Gun {
	[SerializeField]
	private Texture[] lasers;
	[SerializeField]
	private Material lasermaterial;
	private float timer;
	private BoxCollider2D col;
	[SerializeField]
	private AudioSource source;
	private LineRenderer line;
	
	protected override void Awake(){}
	void Start () {
		if(Ship.skinID!=-1 && Locks.Skin(6+Ship.skinID))lasermaterial.mainTexture=lasers[Ship.skinID+1];
		else lasermaterial.mainTexture=lasers[0];
		lasers=null;
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
				v.x=Mathf.Sin(t+f*i)*level*0.6f+Mathf.Cos(t1+f1*i)*0.4f;
				line.SetPosition(i,v);
			}
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
		}
	}
	void Update()
	{
		if(timer>0)
			timer-=Time.deltaTime;
		if(timer<0){
			line.enabled=false;
			col.enabled=false;
		}

	}
}
