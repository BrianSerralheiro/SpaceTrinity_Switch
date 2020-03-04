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
	[HideInInspector]
	public float reviveTimer;
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
#region EQUIPS
	void Equip(int i){
		onShot=onDamage=onRevive=onUse=onUpdate=null;
		equipTime=0;
		_renderer.color=Color.white;
		speed=normalSpeed;
		switch (i)
		{
			case 1:
				onUse=WebShot;
				break;
			case 2:
				onShot=SquidShot;
				break;
			case 3:
				onRevive=AddPlate;
				onDamage=RemovePlate;
				break;
			case 4:
				onUse=Camouflage;
				break;
			case 5:
				onUse=SpeedUp;
				break;
			case 6:
				onUse=Bat;
				break;
		}

	}
	delegate void EquipAction(Ship s);
	EquipAction onShot,onDamage,onRevive,onUse,onUpdate;
	float equipTime;
	public static int webSprite;
	static void WebShot(Ship s){
		if(s.equipTime>Time.time)return;
		GameObject web=new GameObject("playerbullet");
		web.AddComponent<WebShot>().Set(webSprite,s.transform.position+Vector3.up*5,10,3,4,1,s.name);
		web.transform.position=s.transform.position;
		//cooldown 120 seconds
		s.equipTime=Time.time+120;
	}
	public static int squidSprite;
	static void SquidShot(Ship s){
		if(s.equipTime>Time.time)return;
		//cooldown 1 second
		s.equipTime = Time.time+1;
		GameObject go=new GameObject("playerbullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[squidSprite];
		go.AddComponent<BoxCollider2D>();
		CircleCollider2D col=go.AddComponent<CircleCollider2D>();
        col.isTrigger=true;
        col.radius=5;
		Homing bull= go.AddComponent<Homing>();
		bull.owner=s.name;
		bull.damage=2;
		// bull.particleID=particleID;
		bull.spriteID=squidSprite;
		bull.bulleSpeed=6;
		go.transform.position=s.transform.position;
	}
	static void AddPlate(Ship s){
		//add plate
		s.equipTime=0;
	}
	static void RemovePlate(Ship s){
		if(s.equipTime==0){
			//remove plate and give hp
			s.equipTime=1;
			s.hp++;
		}
	}
	static void Camouflage(Ship s){
		if(s.equipTime>Time.time)return;
		//start invisibility
		s.GetComponent<BoxCollider2D>().enabled=false;
		s._renderer.color=new Color(0,1,1,0.5f);
		s.immuneTime=0;
		s.damageTimer=0;
		s.onUpdate=Reveal;
		//invisibility duration;
		s.equipTime=Time.time+1;
	}
	static void Reveal(Ship s){
		if(s.equipTime<Time.time){
			s.GetComponent<BoxCollider2D>().enabled=true;
			s._renderer.color=Color.white;
			s.immuneTime=0.5f;
			//invisibility cooldown 60 seconds
			s.equipTime=Time.time+60;
			s.onUpdate=null;
		}
	}
	float normalSpeed;
	static void SpeedUp(Ship s){
		if(s.equipTime>Time.time)return;
		s.normalSpeed=s.speed;
		//power up duration
		s.equipTime=Time.time+5;
		//speed up bonus
		s.speed+=6;
		s.onUpdate=SpeedDown;
	}
	static void SpeedDown(Ship s){
		if(s.equipTime<Time.time){
			s.speed=s.normalSpeed;
			s.onUpdate=null;
			//speed up cooldown
			s.equipTime=Time.time+60;
		}
	}
	public static EnemyInfo batInfo;
	static void Bat(Ship s){
		if(s.equipTime>Time.time)return;
		GameObject go=new GameObject(s.name);
		BatAlly bat=go.AddComponent<BatAlly>();
		bat.SetSprites(batInfo);
		bat.target=s.transform.position+Vector3.up;
		bat.player=s.transform;
		Bullet bu=go.AddComponent<Bullet>();
		//damage bat deals each second
		bu.damage=5;
		bu.enabled=false;
		bu.owner=s.name;
		bu.pierce=true;
		//bat health / total times hit
		bat.hp=10;
		go.transform.position=s.transform.position;
		//bat spawn cooldown
		s.equipTime=Time.time+15;
	}
#endregion
	void Start()
	{
		normalSpeed=speed;
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
		onRevive?.Invoke(this);
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
		EnemyBase.players[input.id]=transform;
	}
	void OnCollisionStay2D(Collision2D col){
		OnCollisionEnter2D(col);
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(immuneTime > 0) return;
		if(col.gameObject.name.Contains("Player")
		|| col.gameObject.name.Contains("playerbullet")
		|| col.gameObject.name=="drone"
		||col.otherCollider.name=="laser") return;
		if(shielded)
		{
			shielded=false;
			immuneTime=0.5f;
			return;
		}
		onDamage?.Invoke(this);
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
			}
			else
			{
				InGame_HUD.Revive(this);
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
		onRevive?.Invoke(this);
		InGame_HUD.shipHealth[input.id] =1;
		reviveTimer=1;
		transform.position=new Vector3(0,-Scaler.sizeY-2,-0.1f);
		//paused=false;
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
		if(reviveTimer>0){
			reviveTimer-=Time.deltaTime;
			transform.Translate(0,4*Time.deltaTime,0);
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
		onUpdate?.Invoke(this);
		if(!special.Finished())
		{
			special.Update();
		}
#region CHEATS
		if(Input.GetKeyDown(KeyCode.F10))Equip(0);
		if(Input.GetKeyDown(KeyCode.F1))Equip(1);
		if(Input.GetKeyDown(KeyCode.F2))Equip(2);
		if(Input.GetKeyDown(KeyCode.F3))Equip(3);
		if(Input.GetKeyDown(KeyCode.F4))Equip(4);
		if(Input.GetKeyDown(KeyCode.F5))Equip(5);
		if(Input.GetKeyDown(KeyCode.F6))Equip(6);
		if(Input.GetKeyDown(KeyCode.Alpha1))OnLevel(1);
		if(Input.GetKeyDown(KeyCode.Alpha2))OnLevel(2);
		if(Input.GetKey(KeyCode.Alpha3))OnLevel(3);
		if(Input.GetKeyDown(KeyCode.Space) && special.Finished())Special();
#endregion
		if(PlayerInput.GetKeySpecialDown(input.id) && InGame_HUD.special[input.id]>=special.cost && special.Finished())Special();
		if(PlayerInput.GetKeyEquipDown(input.id))onUse?.Invoke(this);
		if(shielded)shield.Add(Time.deltaTime);
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
		Vector3 v =input.GetAxis();
		_renderer.sprite = v.x >= 0.99f  ? skin.right :  v.x <= -0.99f ? skin.left : skin.iddle;
		if(v.sqrMagnitude>0)v/=Mathf.Abs(v.normalized.x)+Mathf.Abs(v.normalized.y);
		v *= Time.deltaTime * speed;
		if(transform.position.x+v.x>Scaler.sizeX/2)v.x=Scaler.sizeX/2-transform.position.x;
		if(transform.position.x+v.x<-Scaler.sizeX/2)v.x=-Scaler.sizeX/2-transform.position.x;
		if(transform.position.y+v.y>Scaler.sizeY)v.y=Scaler.sizeY-transform.position.y;
		if(transform.position.y+v.y<-Scaler.sizeY)v.y=-Scaler.sizeY-transform.position.y;
		transform.Translate(v);
		if(special.AllowShot() && PlayerInput.GetKeyShot(input.id))
		{
			onShot?.Invoke(this);
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
