using UnityEngine;

public class Cherubin : EnemyBase
{
    Del update;
    float time;
    int shots,shotID;
    Transform[] wings=new Transform[2];
    GameObject[] faces=new GameObject[3];
    public override void SetSprites(EnemyInfo ei)
	{
        hp=40;
        points=60;
        update=In;
        shotID=ei.bulletsID[0];
        fallSpeed=-4;
        GameObject g;
        for (int i = 0; i < 3; i++)
        {
            g=new GameObject("face"+i);
            g.AddComponent<SpriteRenderer>().sprite=ei.sprites[1+i];
            faces[i]=g;
            g.transform.parent=transform;
            g.SetActive(false);
        }
        for (int i = 0; i < 2; i++)
        {
            g=new GameObject("wing"+i);
            SpriteRenderer sp=g.AddComponent<SpriteRenderer>();
            sp.sprite=ei.sprites[4];
            sp.flipX=i%2==1;
            wings[i]=g.transform;
            wings[i].parent=transform;
            wings[i].localPosition=new Vector3(0.3f-i%2*0.6f,0.7f,0.1f);
        }
        faces[0].transform.localPosition=new Vector3(-0.37f,-0.05f,-0.1f);
        faces[1].transform.localPosition=new Vector3(0.01f,0.34f,-0.1f);
        faces[2].transform.localPosition=new Vector3(0.395f,-0.06f,-0.1f);
    }
    void In(){
        SlowFall();
        if(transform.position.y<Scaler.sizeY/4)update=Wait;
    }
    void Wait(){
        if(time-1<Time.time){
            for (int i = 0; i < 3; i++)
            {
                faces[i].SetActive(false);
            }
        }
        if(time<Time.time){
            shots++;
            switch (Random.Range(0,3))
            {
                case 0:
                    update=Shot1;
                    faces[0].SetActive(true);
                    break;
                case 1: 
                    update=Shot2;
                    faces[1].SetActive(true);
                    break;
                case 2: 
                    update=Shot3;
                    faces[2].SetActive(true);
                    break;
            }
        }
    }
    void Shot1(){
        for (int i = 0; i < 4; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.bulleSpeed=12;
            bu.spriteID=shotID;
            go.transform.position=transform.position+Vector3.left/2-Vector3.forward/10+Vector3.down*i;
            go.transform.up=Vector3.down;
        }
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }
    void Shot2(){
        for (int i = 0; i < 12; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.spriteID=shotID;
            bu.bulleSpeed=6;
            bu.Timer(4);
            go.transform.position=transform.position-Vector3.forward/10;
            go.transform.rotation=Quaternion.Euler(0,0,i*30);
        }
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }
    void Shot3(){
        GameObject go=new GameObject("enemybullet");
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
        go.AddComponent<BoxCollider2D>();
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner=name;
        bu.spriteID=shotID;
        bu.bulleSpeed=12;
        go.transform.position=transform.position+Vector3.right/2-Vector3.forward/10;
        Vector3 vector=GetPlayer().position-go.transform.position;
        vector.z=0;
        go.transform.up=vector;
        time=Time.time+2;
        if(shots>4)update=SlowFall;
        else update=Wait;
    }

    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
        for (int i = 0; i < 2; i++)
            wings[i].rotation=Quaternion.Euler(0,0,Mathf.PingPong(Time.time*(60+i%2*90),45)*(i%2==0?-1:1));
    }
}
