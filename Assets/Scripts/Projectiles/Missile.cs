using UnityEngine;

public class Missile : EnemyBase {
	public override void SetSprites(EnemyInfo ei)
	{
	}
	private new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.name.Contains("Ship"))
		{
			Destroy(gameObject);
			Debug.Log("BOOM");
		}
		else 
		base.OnCollisionEnter2D(col);
	}
}
