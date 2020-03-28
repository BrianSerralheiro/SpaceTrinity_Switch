using UnityEngine;

public class PlaneMKII : EnemyBase
{
    Transform[] helix=new Transform[3];
    bool charge;
    bool right;
    static Sprite bomb;
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
        if(!bomb)bomb=ei.sprites[2];
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
        if(transform.position.y<Scaler.sizeY/2+2){
            transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,right?90:-90),60*Time.deltaTime);
            if(!charge)Bomb();
        }
        transform.Translate(0,-Time.deltaTime*3,0);
        foreach (Transform t in helix)
        {
            t.Rotate(0,0,90*Time.deltaTime,Space.Self);
        }
        if(transform.position.x<-Scaler.sizeX-2 || transform.position.x>Scaler.sizeX+2)Die();
    }
    void Bomb()
	{
		charge=true;
		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=bomb;
		go.transform.position=transform.position;
		go.AddComponent<Bomb>().SetSprites(null);
	}
}
