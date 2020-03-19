using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Enemy Info")]
public class EnemyInfo : ScriptableObject
{
	public Sprite[] sprites;
	public float lifeproportion=1;
	public Sprite[] bullets;
	public ParticleSystem[] particles;
	System.Type script;
	[HideInInspector]
	public int[] bulletsID,particleID;
	public EnemyInfo copy;
	public System.Type GetScript()
	{
		if(script==null)script=System.Type.GetType(name);
		return script;
	}
	public void Register(){
		if(bullets==null || bullets.Length==0)return;
		bulletsID=new int[bullets.Length];
		for(int i=0;i<bullets.Length;i++){
			bulletsID[i]=Bullet.Register(bullets[i]);
		}
	}
	public void Particles(){
		if(particles==null || particles.Length==0)return;
		particleID=new int[particles.Length];
		for(int i = 0; i < particles.Length; i++)
		{
			particleID[i]=ParticleManager.Register(particles[i]);
		}
	}
	void OnValidate()
	{
		if(copy!=null){
			copy.sprites=sprites;
			copy.bullets=bullets;
			copy.particles=particles;
			copy.name=name;
		}
	} 
}
