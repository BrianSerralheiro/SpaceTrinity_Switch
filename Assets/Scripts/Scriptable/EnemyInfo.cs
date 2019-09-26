using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Enemy Info")]
public class EnemyInfo : ScriptableObject
{
	public Sprite[] sprites;
	public System.Type GetScript()
	{
		return System.Type.GetType(name);
	}
}
