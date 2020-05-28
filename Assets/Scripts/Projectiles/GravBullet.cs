using UnityEngine;

public class GravBullet : Bullet
{
    private static int count=6;
    private int size;
    public int areaDamage;
    public static GameObject impact;
    // void Update()
    // {
    //     if(Ship.paused) return;
    //     renderer.sprite=Bullet.sprites[spriteID+(int)((2-timer)*4%2)];
    //     timer-=Time.deltaTime;
	// 	ParticleManager.Emit(particleID,transform.position,size+1);
	// 	transform.Translate(0,Time.deltaTime*bulletSpeed,0);
	// 	if(timer<=0)Destroy(gameObject);
    // }
    void Update()
	{
		if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		transform.Translate(0,Time.deltaTime* bulletSpeed,0);
		timer-=Time.deltaTime;
		if(timer<=0)Destroy(gameObject);
	}
    public void Size(int i){
        size=i;
        transform.localScale=Vector3.one*i;
    }
	new void OnCollisionEnter2D(Collision2D col)
	{
        if(col.collider.name.Contains("layer") || col.collider.name.Contains("laser"))return;
        Instantiate(impact,transform.position,Quaternion.identity).transform.localScale=Vector3.one*size*2;
        GameObject go = new GameObject("playerbullet");
		go.AddComponent<CircleCollider2D>().radius=size*2;
		Bullet bull= go.AddComponent<Bullet>();
		bull.owner=owner;
		bull.damage=areaDamage;
		bull.pierce=true;
		bull.particleID=bull.impactID=-1;
		bull.bulletSpeed=0;
        bull.enabled=false;
        Destroy(go,0.1f);
		go.transform.position=transform.position;
        base.OnCollisionEnter2D(col);
    }
}
