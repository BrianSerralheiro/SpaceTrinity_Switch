
using UnityEngine;
[CreateAssetMenu(fileName ="World",menuName ="Wolrd Info")]
public class WorldInfo : ScriptableObject
{
	public string wave;
    public EnemyInfo[] enemies;
	public Texture[] bgs;
	public EnemyInfo subBoss;
	public bool loopWorld=true;
	public float scroll=60;
	public EnemyInfo Boss;
	public EnemyInfo drone;
	[HideInInspector]
	public string songName;
	[HideInInspector]
	public string bossSong;

}
