using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyBase {
	private int id;
	private Vector3 dir=Vector3.right;

	private bool dropped;

	private bool _right;
	public override void SetSprites(EnemyInfo ei)
	{
		GetComponent<SpriteRenderer>().sprite=ei.sprites[!PlayerInput.Conected(1) || Random.value>0.5?Ship.player1:Ship.player2];
		ItemDrop.spriteId=ei.bulletsID[0];
		hp=1;
		name="drone";
	}
	
	// Update is called once per frame
	new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0, 2* -Time.deltaTime,0);
		else transform.Translate(dir*(_right?-1:1)* 3 * Time.deltaTime);
		if(transform.position.x<-Scaler.sizeX/2f-1 || transform.position.x>Scaler.sizeX/2f+1)Die();
	}
	public override void Position(int i)
	{
		_right = i > 9;
		base.Position(i);
		id = i%3;
	}
	protected override void Die()
	{
		Destroy(gameObject);
		if(hp<=0 && !dropped)
		{
			dropped = true;
			EnemySpawner.points[killerid-1]+=points;
			GameObject go = new GameObject("ItemDrop");
			go.AddComponent<SpriteRenderer>();
			ItemDrop item= go.AddComponent<ItemDrop>();
			item.Set(id,0);
			go.transform.position = transform.position;
			if(id==2 && PlayerInput.Conected(1)){
				go = new GameObject("ItemDrop");
				go.AddComponent<SpriteRenderer>();
				item= go.AddComponent<ItemDrop>();
				item.Set(id,1);
				go.transform.position = transform.position;
			}
		}
	}
}
