using UnityEngine;

public class Plane : EnemyBase
{
    Transform[] helix=new Transform[3];
    int bullets=6,shotId;
    float time;
    bool right;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=30;
        points=60;
        for (int i = 0; i < 3; i++)
        {
            GameObject go =new GameObject("helix");
            go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
            go.transform.parent=transform;
            go.transform.localPosition=new Vector3(-0.8f+i*0.8f,-0.2f+i%2*-0.6f,-0.1f);
            helix[i]=go.transform;
        }
        shotId=ei.bulletsID[0];
    }
	public override void Position(int i)
	{
		base.Position(i);
        right=i>9;
    }
    new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
        if(time<Time.time && bullets>0 && transform.position.y<Scaler.sizeY/2+2)Shot();
        if(bullets<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,right?90:-90),60*Time.deltaTime);
        transform.Translate(0,-Time.deltaTime*3,0);
        foreach (Transform t in helix)
        {
            t.Rotate(0,0,90*Time.deltaTime,Space.Self);
        }
        if(transform.position.x<-Scaler.sizeX-2 || transform.position.x>Scaler.sizeX+2)Die();
    }
    void Shot()
    {
        time=Time.time+0.1f;
        GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
		go.AddComponent<BoxCollider2D>();
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shotId;
        b.bulletSpeed = 8;
		go.transform.position=transform.position+new Vector3(-0.5f+bullets%2,-1,0);
		Vector3 v = GetPlayer(transform.position).position - transform.position;
        v.z = 0;
        go.transform.up=v;
        bullets--;
    }
}
