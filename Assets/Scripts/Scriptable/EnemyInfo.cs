using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Enemy Info")]
public class EnemyInfo : ScriptableObject
{
	public Sprite[] sprites;
	System.Type script;
	//public EnemyInfo copy;
	public System.Type GetScript()
	{
		if(script==null)script=System.Type.GetType(name);
		return script;
	}
	/*void OnValidate()
	{
		if(copy!=null)copy.sprites=sprites;
	}*/
}
