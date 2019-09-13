using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyBase {
	private int id;
	private Vector3 dir=Vector3.right;

	private bool dropped;

	private bool _right;
	new void Start () {
		base.Start();
		hp=40;
	}
	
	// Update is called once per frame
	new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
		if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime,0);
		else transform.Translate(dir*(_right?-1:1)*Time.deltaTime);
		if(transform.position.x<-Scaler.sizeX/2f-1 || transform.position.x>Scaler.sizeX/2f+1)Die();
	}
	public override void Position(int i)
	{
		_right = i > 3;
		base.Position(i%8);
		id = i%3;
	}
	protected override void Die()
	{
		Destroy(gameObject);
		if(hp<=0 && !dropped)
		{
			dropped = true;
			EnemySpawner.points+=points;
			GameObject go = new GameObject("ItemDrop");
			go.AddComponent<SpriteRenderer>();
			ItemDrop item= go.AddComponent<ItemDrop>();
			item.Set(id);
			go.transform.position = transform.position;
		}
	}
}
