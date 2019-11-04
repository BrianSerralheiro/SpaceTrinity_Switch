using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour 
{
	private int id;
	private bool set;
	private SpriteRenderer _renderer;
	private Sprite[] sprite=new Sprite[2];
	private Vector3 dir;

	void Start () 
	{
		dir =  new Vector3(Random.value,-2 - Random.value, 0);
		Set(Random.Range(0 , 2));
	}
	public void Set(int i)
	{
		if(set)return;
		set=true;
		id = i;
		_renderer= GetComponent<SpriteRenderer>();
		_renderer.sprite = SpriteBase.I.item[id];
		sprite[0] = SpriteBase.I.item[(id==2?id+Ship.playerID:id)*2];
		sprite[1] = SpriteBase.I.item[(id==2 ? id+Ship.playerID : id)*2+1];
		gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
	}
	void Update () 
	{
		if(Ship.paused) return;
		if(transform.position.y>Scaler.sizeY-4)dir.y=-Mathf.Abs(dir.y);
		if(transform.position.x>Scaler.sizeX/2f-2)dir.x=-Mathf.Abs(dir.x);
		if(transform.position.x<-Scaler.sizeX/2f+2)dir.x=Mathf.Abs(dir.x);
		transform.Translate(dir * Time.deltaTime * 3);
		if(transform.position.y<-Scaler.sizeY)dir.Set(Random.value,0.5f + Random.value,0);
		_renderer.sprite=Bullet.blink ? sprite[0]:sprite[1];
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Ship s = other.GetComponent<Ship>();
		if (s != null)
		{
			SoundManager.PlayEffects(21);
			if(id == 0)
			{
				s.Shield();
			}
			else if(id == 1)
			{
				InGame_HUD.special=1;
			}
			else if(id == 2)
			{
				s.OnLevel();
			}
			Destroy(gameObject);
		}
	}
}
