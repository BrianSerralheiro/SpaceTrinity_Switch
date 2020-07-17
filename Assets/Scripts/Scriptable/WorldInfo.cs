
using UnityEngine;
[CreateAssetMenu(fileName ="World",menuName ="Wolrd Info")]
public class WorldInfo : ScriptableObject
{
	public string wave;
    public EnemyInfo[] enemies;
	public ParticleSystem explosion;
	public BGProp[] props;
	public EnemyInfo subBoss;
	public bool loopWorld=true;
	public float scroll=60;
	[Range(0,1)]
	public float lightIntensity=1;
	public Color lightColor=Color.white;
	public EnemyInfo Boss;
	public Sprite bossFace;
	public EnemyInfo drone;
	public DialogInfo  begining,end;
	[HideInInspector]
	public string songName;
	[HideInInspector]
	public string bossSong;

}
