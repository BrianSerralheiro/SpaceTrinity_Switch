using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour 
{
	private int id,playerID;
	private bool set;
	private SpriteRenderer _renderer;
	private Core core;
	private Vector3 dir;
	public static int spriteId;
	void Start () 
	{
		dir =  new Vector3(Random.value,-2 - Random.value, 0);
	}
	public void Set(int i,int j)
	{
		if(set)return;
		set=true;
		id = i;
		playerID=j;
		EnemySpawner.AddPost(gameObject);
		_renderer= GetComponent<SpriteRenderer>();
		_renderer.color=Color.black;
		i=(id==2?2+(j==1?Ship.player2:Ship.player1):id);
		_renderer.sprite = Bullet.sprites[spriteId+i];
		gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
		core=gameObject.AddComponent<Core>().Set(Color.black,new Color(0.2f,0.2f,0.2f));
	}
	void Update () 
	{
		if(Ship.paused) return;
		if(transform.position.y>Scaler.sizeY-4)dir.y=-Mathf.Abs(dir.y);
		if(transform.position.x>Scaler.sizeX/2f-2)dir.x=-Mathf.Abs(dir.x);
		if(transform.position.x<-Scaler.sizeX/2f+2)dir.x=Mathf.Abs(dir.x);
		transform.Translate(dir * Time.deltaTime * 3);
		if(transform.position.y<-Scaler.sizeY)dir.Set(Random.value,0.5f + Random.value,0);
		core.Set((Mathf.Cos(Time.time*5)+1)/2f);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Ship s = other.GetComponent<Ship>();
		if (s != null && (id<2 || s.input.id==playerID))
		{
			SoundManager.PlayEffects(21);
			ParticleManager.Emit(5,s.transform,1);
			if(id == 0)
			{
				s.Shield();
			}
			else if(id == 1)
			{
				InGame_HUD.special[s.input.id]=1;
			}
			else if(id == 2)
			{
				s.OnLevel();
			}
			Destroy(gameObject);
		}
	}
}
