using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : EnemyBase {
	private Vector3 rotation = Vector3.zero;
	private Transform armL;
	private Transform armR;
	private Core core;
	private Vector3 vector = new Vector3();
	new public void Start()
	{
		base.Start();
		hp=40;
		points = 120;
		GameObject go = new GameObject("armL");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.grabber[1];
		armL=go.transform;
		go=new GameObject("armR");
		go.AddComponent<SpriteRenderer>().sprite=SpriteBase.I.grabber[2];
		armR=go.transform;
		armL.parent=armR.parent=transform;
		armL.localPosition=new Vector3(0.3f,-0.3f,-0.1f);
		armR.localPosition=new Vector3(-0.3f,-0.3f,-0.1f);
		go=new GameObject("core");
		core=go.AddComponent<Core>().Set(SpriteBase.I.grabber[3],new Color(0.5f,0.1f,0.05f));
		core.transform.parent=transform;
		core.transform.localPosition=new Vector3(0,-0.09f);
	}
	
	new void Update(){
		if(Ship.paused) return;
		base.Update();
		rotation.z=Mathf.Atan2(transform.position.x-player.position.x,player.position.y-transform.position.y)*Mathf.Rad2Deg+180;
		transform.eulerAngles=rotation;
		if(!transform.parent){
			transform.Translate(0,-2*Time.deltaTime,0);
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
