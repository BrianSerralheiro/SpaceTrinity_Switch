
using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Enemy Info")]
public class EnemyInfo : ScriptableObject
{
	public Sprite[] sprites;
	System.Type script;
	public System.Type GetScript()
	{
		if(script==null)script=System.Type.GetType(name);
		return script;
	}
}
