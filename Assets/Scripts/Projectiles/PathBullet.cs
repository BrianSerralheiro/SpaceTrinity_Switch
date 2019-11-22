using UnityEngine;

public class PathBullet : Bullet
{
    public BulletPath path;

    void Update()
    {
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		if(bulletTime<=0)renderer.sprite=sprites[spriteID+(blink?0:1)];
        transform.Translate(BulletPath.Next(ref path,transform.position.x>0),Space.World);
        if(path.Finished())Destroy(gameObject);
    }
}
