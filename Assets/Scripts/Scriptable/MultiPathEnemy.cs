using UnityEngine;
[CreateAssetMenu(fileName ="MultiPath",menuName ="Multi Path")]
public class MultiPathEnemy : EnemyInfo
{
    public BulletPath[] paths;
    public int shotCount,cicles;
    public float shotDelay,reloadTime;
}
