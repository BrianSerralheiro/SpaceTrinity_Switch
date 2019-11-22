using UnityEngine;

public class Ship : MonoBehaviour {
	float shoottimer;
	[SerializeField]
	private ParticleSystem trail;
	[SerializeField]
	private Color[] colors;
	[SerializeField]
	private Gun[] guns;
	[SerializeField]
	Transform burst;
	[SerializeField]
	private float firerate=0.5f;
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
	[SerializeField]
	private Sprite[] skins;
	public static bool paused;
	[SerializeField]
	private Core shield;
	[SerializeField]
	private Material specialMat;
	[SerializeField]
	private Texture2D[] specials;
	private bool shielded;

	private float damageTimer;
	private float freezeTimer;
	private float clickTime=-1;

	private SpriteRenderer _renderer;

	private int Level = 1;
	

	[SerializeField]
	private int id;
	public static int playerID;
	public static int skinID=-1;

	public float immuneTime;

	[SerializeField]
	private Sprite[] sprites;

	void Start()
	{
		if(playerID != id)
		{
			gameObject.SetActive(false);
			return;
		}
		InGame_HUD.shipHealth = 1;
		InGame_HUD.special = 1;
		hp=maxhp;
		_renderer = GetComponent<SpriteRenderer>();
		specialMat.mainTexture=specials[0];
		DialogBox.Texts(falas,sizes);
		DialogBox.Chars(charPics);
		falas=null;
		charPics=null;
		sizes=null;
		if(skinID!=-1 && Locks.Skin(id*3+skinID))
		{
			_renderer.sprite=skins[skinID];
			ParticleSystem.MainModule main = trail.main;
			main.startColor=colors[skinID];
			specialMat.mainTexture=specials[skinID+1];
			specialMat=null;
			specials=null;
			skins=null;
			colors=null;
		}
		EnemyBase.player=transform;
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(immuneTime > 0) return;
		if(col!=null && col.gameObject.name=="playerbullet") return;
		if(col!=null && col.otherCollider.name=="laser") return;
		if(shielded)
		{
			shielded=false;
			return;
		}
		if(--hp<=0)
		{
			if(Level>1)
			{
				OnLevel(--Level);
			}
			SoundManager.PlayEffects(10, 1, 0);
			paused = true;
			gameObject.SetActive(false);
			GameOverController.Open(this);
		}
		else
		{
			SoundManager.PlayEffects(9, 1, 0);
		}
		InGame_HUD.shipHealth = (float)hp / (float)maxhp;
		damageTimer=1;
		immuneTime=1f;
	}
	public void Revive()
	{
		gameObject.SetActive(true);
		immuneTime = 3;
		hp=maxhp;
		InGame_HUD.shipHealth =1;
		paused=false;
	}
	public void Shield()
	{
		if(hp==maxhp)
			shielded=true;
		else hp++;
		InGame_HUD.shipHealth=(float)hp/(float)maxhp;
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
		if(freezeTimer > 0)
		{
			freezeTimer -= Time.deltaTime;
			if(freezeTimer<=0)
			{
				burst.gameObject.SetActive(true);
				_renderer.enabled=true;
				GameObject go = new GameObject("playerbullet");
				go.AddComponent<BoxCollider2D>().size=new Vector2(Scaler.sizeX,Scaler.sizeY);
				Bullet bu= go.AddComponent<Bullet>();
				bu.damage=200;
				bu.pierce=true;
				bu.owner=name;
				bu.enabled=false;
				Destroy(go,0.1f);
			}
			return;
		}
		if(Input.GetKeyDown(KeyCode.Alpha1))OnLevel(1);
		if(Input.GetKeyDown(KeyCode.Alpha2))OnLevel(2);
		if(Input.GetKeyDown(KeyCode.Alpha3))OnLevel(3);
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
		Vector3 v =new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0) * speed;
		_renderer.sprite = v.x >= speed * 0.99f  ? sprites[1] :  v.x <= -speed * 0.99f ? sprites[0] : sprites[2];
		v *= Time.deltaTime;
		if(transform.position.x+v.x>Scaler.sizeX/2)v.x=Scaler.sizeX/2-transform.position.x;
		if(transform.position.x+v.x<-Scaler.sizeX/2)v.x=-Scaler.sizeX/2-transform.position.x;
		if(transform.position.y+v.y>Scaler.sizeY)v.y=Scaler.sizeY-transform.position.y;
		if(transform.position.y+v.y<-Scaler.sizeY)v.y=-Scaler.sizeY-transform.position.y;
		transform.Translate(v);
		if(Input.GetKey(KeyCode.Space) && shoottimer<=0)
		{
			if(id!=2)SoundManager.PlayEffects(2 + id,0.1f,0.5f);
			shoottimer=firerate;
			foreach(Gun gun in guns)
			{
				gun.Shoot();
			}
		}
		if(shoottimer>0) shoottimer-=Time.deltaTime;
		
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
		SoundManager.PlayEffects(6 + id, 1f, 2f);
		switch(id)
		{
			case 0:
				ParticleManager.Emit(12,Vector3.up*-10,20,1f);
				immuneTime=4f;
				freezeTimer=3f;
				break;
			case 1:
				ParticleManager.Emit(13,Vector3.zero,200);
				immuneTime=4;
				freezeTimer=3;
				break;
			case 2:
				ParticleManager.Emit(14,Vector3.zero,5,1f);
				immuneTime=2;
				freezeTimer=1;
				break;
			case 3:
				ParticleManager.Emit(15,Vector3.zero,20,2f);
				immuneTime=3;
				freezeTimer=2;
				_renderer.enabled=false;
				burst.gameObject.SetActive(false);
				break;
		}
	}
}
