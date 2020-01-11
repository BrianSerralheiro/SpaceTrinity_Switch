using UnityEngine;

public class PathBullet : Bullet
{
    public BulletPath path;
    public bool mirror;
    private Vector3 position;
    protected override void Start()
    {
		renderer=GetComponent<SpriteRenderer>();
        position=transform.position;
        transform.up=path.Directiom(mirror);
    }
    void Update()
    {
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		if(bulletTime<=0)renderer.sprite=sprites[spriteID+(blink?0:1)];
        transform.up=path.Directiom(mirror);
        transform.position=position+BulletPath.Next(ref path,mirror);
        if(path.Finished())Destroy(gameObject);
    }
}
