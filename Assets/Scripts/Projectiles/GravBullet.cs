using UnityEngine;

public class GravBullet : Bullet
{
    private static int count=6;
    public static int impactID;
    private int size;
    private delegate void Del();
    public int sizeDamage;
    Del update;
    void Update()
    {
        if(Ship.paused) return;
        renderer.sprite=Bullet.sprites[spriteID+(int)((2-timer)*4%2)];
        timer-=Time.deltaTime;
		ParticleManager.Emit(particleID,transform.position,size+1);
		transform.Translate(0,Time.deltaTime*bulletSpeed,0);
		if(timer<=0)Destroy(gameObject);
    }
    public void Size(int i){
        size=i;
        pierce=i>0;
        damage+=i*sizeDamage;
        transform.localScale=Vector3.one*(i+1)*0.5f;
    }
    void UpdateSprite0(){
        renderer.sprite=Bullet.sprites[spriteID+(int)((2-timer)*4%2)];
    }void UpdateSprite1(){
       renderer.sprite=Bullet.sprites[spriteID+2+(int)((2-timer)*16%count)];
    }void UpdateSprite2(){
       renderer.sprite=Bullet.sprites[spriteID+2+count+(int)((2-timer)*16%count)];
    }
	new void OnCollisionEnter2D(Collision2D col)
	{
        if(col.collider.name.Contains("player"))return;
        base.OnCollisionEnter2D(col);
        if(size==0)ParticleManager.Emit(impactID,col.contacts[0].point,1);
    }
}
