using UnityEngine;
[CreateAssetMenu(fileName ="Enemy",menuName ="Shooter Info")]
public class PathEnemy : EnemyInfo
{
    public BulletPath bulletPath;
    public int shotCount,cicles;
    public float shotDelay,reloadTime;
}
