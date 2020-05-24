using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {
	// private static ParticleSystem[] sys;
	private static Vector3 up=Vector3.up;
	private static Vector3 mod=Vector3.back*0.1f;
	private static List<ParticleSystem> sys=new List<ParticleSystem>();
	static ParticleManager manager;
	static List<string> names=new List<string>();
	void Awake()
	{
		if(manager){
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		manager=this;
	}
	public static int Register(ParticleSystem s) 
	{
		if(!s)
		{
			return -1;
		}
		if(names.Contains(s.name))return names.IndexOf(s.name);
		names.Add(s.name);
		s=Instantiate<ParticleSystem>(s);
		s.name="particle"+sys.Count;
		sys.Add(s);
		s.transform.parent=manager.transform;
		return sys.Count-1;
	}
	public static void Clear(){
		foreach (ParticleSystem item in sys)
		{
			Destroy(item.gameObject);
		}
		sys.Clear();
		names.Clear();
	}
	public static void Emit(int i,Vector3 p,int c,float a)
	{
		sys[i].transform.up=up;
		for (int j = 0; j < c; j++)
		{		
			sys[i].transform.position=(Vector3)Random.insideUnitCircle*a+p+mod;
			sys[i].Emit(1);
		}
	}
	public static void Emit(int i,Transform t,int c)
	{
		sys[i].transform.forward=-t.up;
		sys[i].transform.position=t.position+mod;
		sys[i].transform.rotation=t.rotation;
		sys[i].Emit(c);
	}
	public static void Emit(int i,Vector3 p,Vector3 up,int c){
		sys[i].transform.up=up;
		Emit(i,p,c);
	}
	public static void Emit(int i,Vector3 p,int c)
	{
		if(i<0)
		{
			return;
		}
		if(sys==null)return;
		sys[i].transform.up=up;
		sys[i].transform.position=p+mod;
		sys[i].Emit(c);
	}
}
