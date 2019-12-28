using UnityEngine;

public class Ship : MonoBehaviour {
	[SerializeField]
	private ParticleSystem trail;
	[SerializeField]
	private Color[] colors;
	[SerializeField]
	private Gun[] guns;
	[SerializeField]
	Transform burst;

	[SerializeField]
	private int maxhp = 1;
	private int hp;
	[SerializeField]
	private string[] falas;
	[SerializeField]
	private float[] sizes;
	[SerializeField]
	private Sprite[] charPics;
	private Vector3 offset = Vector3.up;
	[SerializeField]
	private float speed=5f;
	public static bool paused;
	[SerializeField]
	private Core shield;
	[SerializeField]
	private Material specialMat;
	[SerializeField]
	private SpecialInfo special;
	public static int[] continues=new int[2];
	private bool shielded;

	private float damageTimer;
	private float clickTime=-1;

	private SpriteRenderer _renderer;

	private int Level = 1;
	

	[SerializeField]
	private int id;
	public static int player1,player2=5;
	public static int[] skinID={-1,-1,-1,-1,-1,-1};

	public float immuneTime;
	public PlayerInput input;
	public static Skin[] skins=new Skin[2];

	[SerializeField]
	private Skin skin;

	void Start()
	{
		input=PlayerInput.Get();
		continues[input.id]=PlayerInput.Conected(1)?2:4;
		InGame_HUD.shipHealth[input.id] = 1;
		InGame_HUD.special[input.id] = 0;
		name=input.name;
		foreach (Gun gun in guns)
		{
			gun.Load(id);
		}
		hp=maxhp;
		_renderer = GetComponent<SpriteRenderer>();
		// specialMat.mainTexture=specials[0];
		DialogBox.Texts(falas,sizes);
		DialogBox.Chars(charPics);
		falas=null;
		charPics=null;
		sizes=null;
		if(skinID[id]!=-1 && Locks.Skin(id*3+skinID[id]))
		{
			skin=skins[input.id];
			ParticleSystem.MainModule main = trail.main;
			main.startColor=colors[skinID[id]];
			// specialMat.mainTexture=specials[skinID[id]+1];
			specialMat=null;
			// specials=null;
			colors=null;
		}
		EnemyBase.player=transform;
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(immuneTime > 0) return;
		if(col!=null && col.gameObject.name=="playerbullet") return;
		if(col!=null && col.gameObject.name=="drone") return;
		if(col!=null && col.otherCollider.name=="laser") return;
		if(shielded)
		{
			shielded=false;
			immuneTime=0.5f;
			return;
		}
		damageTimer=1;
		immuneTime=1f;
		if(--hp<=0)
		{
			damageTimer = 0;
			if(Level>1)
			{
				OnLevel(--Level);
			}
			SoundManager.PlayEffects(10, 1, 0);
			gameObject.SetActive(false);
			ParticleManager.Emit(0, transform.position, 1);
			if(continues[input.id]-- <= 0)
			{				
				GameOverController.Open(this);
				paused = true;
			}
			else
			{
				transform.position = Vector3.zero;
				Revive();
			}
		}
		else
		{
			SoundManager.PlayEffects(9, 1, 0);
		}
		InGame_HUD.shipHealth[input.id] = (float)hp / (float)maxhp;

	}
	public void Revive()
	{
		gameObject.SetActive(true);
		immuneTime = 3;
		hp=maxhp;
		InGame_HUD.shipHealth[input.id] =1;
		paused=false;
	}
	public void Shield()
	{
		// if(hp==maxhp)
			shielded=true;
		// else hp++;
		// InGame_HUD.shipHealth[input.id]=(float)hp/(float)maxhp;
	}
	public int ID(){
		return id;
	}
	void Update()
	{
		if(paused)
		{
			return;
		}
		
		if(immuneTime > 0)
			{
			immuneTime -= Time.deltaTime;
			Color c = _renderer.color;
			if(immuneTime <= 0)
			{
				c.a = 1;
			}
			else
			{
				c.a = Mathf.Abs(Mathf.Cos(immuneTime * 20));
			}
			_renderer.color = c;
			
		}
		if(!special.Finished())
		{
			special.Update();
		}
		if(Input.GetKeyDown(KeyCode.Alpha1))OnLevel(1);
		if(Input.GetKeyDown(KeyCode.Alpha2))OnLevel(2);
		if(Input.GetKey(KeyCode.Alpha3))OnLevel(3);
		if(Input.GetKeyDown(KeyCode.Space) && special.Finished())Special();
		if(Input.GetKeyDown(input.special) && InGame_HUD.special[input.id]>=special.cost && special.Finished())Special();
		if(shielded) shield.Add(Time.deltaTime);
		else shield.Min(Time.deltaTime);
		if(Bullet.bulletTime<=0)
		{
			Bullet.bulletTime=0.1f;
			Bullet.blink=!Bullet.blink;
		}
		Bullet.bulletTime-=Time.deltaTime;
		if(damageTimer > 0)
		{
			damageTimer -= Time.deltaTime;
			_renderer.color = Color.Lerp(Color.white,Color.red,damageTimer);
		}
		Vector3 v =input.GetAxis() * speed;
		_renderer.sprite = v.x >= speed * 0.99f  ? skin.right :  v.x <= -speed * 0.99f ? skin.left : skin.iddle;
		v *= Time.deltaTime;
		if(transform.position.x+v.x>Scaler.sizeX/2)v.x=Scaler.sizeX/2-transform.position.x;
		if(transform.position.x+v.x<-Scaler.sizeX/2)v.x=-Scaler.sizeX/2-transform.position.x;
		if(transform.position.y+v.y>Scaler.sizeY)v.y=Scaler.sizeY-transform.position.y;
		if(transform.position.y+v.y<-Scaler.sizeY)v.y=-Scaler.sizeY-transform.position.y;
		transform.Translate(v);
		if(special.AllowShot() && Input.GetKey(input.shoot))
		{
			if(id!=2)SoundManager.PlayEffects(2 + id,0.1f,0.5f);
			foreach(Gun gun in guns)
			{
				if(gun.enabled)
				{
					gun.Shoot();
				}
			}
		}
	}
	void OnLevel(int i)
	{
		foreach(Gun gun in guns)
		{
			gun.Level(i);
		}
	}

	public void OnLevel()
	{
		if(Level<3)
		{
			OnLevel(++Level);
		}
	}


	public void Special()
	{
		InGame_HUD.special[input.id]-=special.cost;
		SoundManager.PlayEffects(6 + id, 1f, 2f);
		immuneTime=special.imuneTime;
		special.Start(transform);
	}
}
