using UnityEngine;

public class Ironfly : EnemyBase
{
    float time,variant=0.5f,angle;
    SpriteRenderer[] wings=new SpriteRenderer[4];
    public override void SetSprites(EnemyInfo ei)
	{
        hp=30;
        points=50;
        for (int i = 0; i < wings.Length; i++)
        {
            GameObject go=new GameObject("wing"+i);
            go.transform.parent=transform;
            go.transform.localPosition=new Vector3(-0.1f+i%2*0.2f,0,-0.1f);
            go.transform.Rotate(0,0,-15+(i+i/2)%2*30);
            wings[i]=go.AddComponent<SpriteRenderer>();
            wings[i].sprite=ei.sprites[1];
            wings[i].flipX=i%2==1;
        }
    }
    new void Update () 
	{
		if(Ship.paused) return;
		base.Update();
        if(time<Time.time-variant){
            time=Time.time+variant;
            transform.rotation=Quaternion.Euler(0,0,Random.Range(-15f,15f));
        }
        if(time>Time.time)transform.Translate(0,-Time.deltaTime*12,0);
        bool b=Time.time%0.4f<0.1f;
        foreach (SpriteRenderer rend in wings)
        {
            rend.enabled=b;
        }
        if(transform.position.y<Scaler.sizeY-3)Die();
    }
}
