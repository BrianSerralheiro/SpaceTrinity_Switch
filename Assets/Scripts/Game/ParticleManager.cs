using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {
	[SerializeField]
	private ParticleSystem[] systems;
	private static ParticleSystem[] sys;
	private static Vector3 up=Vector3.up;
	private static Vector3 mod=Vector3.back*0.1f;
	private static float timer,marker,fraction;
	private static int id;
	void Start () {
		sys=systems;
	}
	private void Update()
	{
		if(timer>0)
		{
			timer-=Time.deltaTime;
			if(timer <=marker){
				sys[id].Emit(1);
				marker-=fraction;
			}
		}
	}
	public static void Emit(int i,Vector3 p,int c,float t)
	{
		sys[i].transform.up=up;
		sys[i].transform.position=p+mod;
		id=i;
		timer=marker=t;
		fraction=t/(float)c;
	}
	public static void Emit(int i,Transform t,int c)
	{
		sys[i].transform.forward=-t.up;
		sys[i].transform.position=t.position+mod;
		sys[i].transform.rotation=t.rotation;
		sys[i].Emit(c);
	}
	public static void Emit(int i,Vector3 p,int c)
	{
		sys[i].transform.up=up;
		sys[i].transform.position=p+mod;
		sys[i].Emit(c);
	}
}
