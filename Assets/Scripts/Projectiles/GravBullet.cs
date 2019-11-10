using UnityEngine;

public class GravBullet : Bullet
{
    private static int count=6;
    private int size;
    private delegate void Del();
    Del update;
    void Update()
    {
        if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		update?.Invoke();
        timer-=Time.deltaTime;
		transform.Translate(0,Time.deltaTime*10,0);
		if(timer<=0) Destroy(gameObject);
    }
    public void Size(int i){
        size=i;
        Debug.Log(i);
        pierce=i>0;
        damage+=i*20;
        BoxCollider2D c=GetComponent<BoxCollider2D>();
        Vector2 v2=c.size;
        switch(i){
            case 0:
                update=UpdateSprite0;
                v2.x=2;
                break;
            case 1:
                update=UpdateSprite1;
                v2.x=3;
                break;
            default:
                update=UpdateSprite2;
                break;
        }
        c.size=v2;
    }
    void UpdateSprite0(){
        renderer.sprite=Bullet.sprites[spriteID+(int)(Time.time%2)];
    }void UpdateSprite1(){
       renderer.sprite=Bullet.sprites[spriteID+2+(int)(Time.time*16%count)];
    }void UpdateSprite2(){
       renderer.sprite=Bullet.sprites[spriteID+2+count+(int)(Time.time*16%count)];
    }
	new void OnCollisionEnter2D(Collision2D col)
	{
        base.OnCollisionEnter2D(col);
    }
}
