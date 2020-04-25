using UnityEngine;

public class GravBullet : Bullet
{
    private static int count=6;
    private int size;
    private delegate void Del();
    public int sizeDamage;
    Del update;
    void Update()
    {
        if(Ship.paused) return;
        update?.Invoke();
        timer-=Time.deltaTime;
		transform.Translate(0,Time.deltaTime*bulletSpeed,0);
		if(timer<=0)Destroy(gameObject);
    }
    public void Size(int i){
        size=i;
        pierce=i>0;
        damage+=i*sizeDamage;
        BoxCollider2D c=GetComponent<BoxCollider2D>();
        Vector2 v2=c.size;
        switch(i){
            case 0:
                update=UpdateSprite0;
                v2.x=1;
                break;
            case 1:
                update=UpdateSprite1;
                v2.x=1;
                break;
            default:
                update=UpdateSprite2;
                v2.x=3;
                break;
        }
        c.size=v2;
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
        base.OnCollisionEnter2D(col);
    }
}
