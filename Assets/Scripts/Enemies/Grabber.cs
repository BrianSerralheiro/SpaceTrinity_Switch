using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : EnemyBase {
	private Vector3 rotation = Vector3.zero;
	private Transform armL;
	private Transform armR;
	private Core core;
	private Vector3 vector = new Vector3();
	BulletPath path;
	static BulletPath[] paths;
	Vector3 position;
	public override void SetSprites(EnemyInfo ei)
	{
		hp=10;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
		points = 50;
		GameObject go = new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
		armR=go.transform;
		armL.parent=armR.parent=transform;
		armL.localPosition=new Vector3(0.6f,-0.6f,-0.1f);
		armR.localPosition=new Vector3(-0.6f,-0.6f,-0.1f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(ei.sprites[3],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,-0.18f);
		if(paths==null)paths=(ei as MultiPathEnemy).paths;
	}
	public override void Position(int i){
		base.Position(i);
		int p=i<10?i:(19-i);
		if(i==0)transform.position=new Vector3(-Scaler.sizeX/2-2,Scaler.sizeY/2,0.1f);
		if(i==19)transform.position=new Vector3(Scaler.sizeX/2+2,Scaler.sizeY/2,0.1f);
		position=transform.position;
		if(p<3)path=paths[p];
		else path=paths[3];
	}
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		if(!transform.parent){
			if(path.Finished()){
				Die();
			}
			else transform.position=position+BulletPath.Next(ref path,position.x>0);
			vector.Set(0,0,Mathf.PingPong(Time.time*100,45f));
			core.Set(0);
		}
		else
		{
			vector.Set(0,0,-30f);
			transform.parent.Translate(transform.localPosition*Time.deltaTime);
			if(transform.localPosition.y<0.2f)transform.parent=null;
			core.Set(1);
		}
		armL.localEulerAngles=vector;
		armR.localEulerAngles=-vector;
	}
	new private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.name.Contains("Ship")){
			transform.parent=col.transform;
			hp*=2;
		}else 
			base.OnCollisionEnter2D(col);
	}
}
