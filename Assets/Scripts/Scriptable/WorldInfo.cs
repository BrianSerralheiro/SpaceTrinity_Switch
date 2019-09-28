using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="World",menuName ="Wolrd Info")]
public class WorldInfo : ScriptableObject
{
	public string wave;
    public EnemyInfo[] enemies;
	public Texture[] bgs;
	public AudioClip Song;
	public EnemyInfo Boss;
	public AudioClip BossSong;

}
