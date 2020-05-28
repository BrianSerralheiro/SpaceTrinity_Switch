using UnityEngine;

public class Jet : EnemyBase
{
    MiniTurret[] turrets=new MiniTurret[2];
    float shootTime,moveTime;
    Vector3 pos=new Vector3(Scaler.sizeX/2,Scaler.sizeY/2);
    Quaternion rot=Quaternion.Euler(0,0,-45);
    int patternId;
    Transform target;
    bool face;
    Del update;
    float[,] patterns={{90,-30,4,0.02f,2},
                        {0,0,5,0.5f,4},
                        {0,18,10,0.1f,1.2f},
                        {180,-18,10,0.1f,3},
                        {15,-6,5,0.05f,0.25f},
                        {-15,6,5,0.05f,0.25f},
                        {15,-6,5,0.05f,0.25f},
                        {-15,6,5,0.05f,0.25f},
                        {15,-6,5,0.05f,0.25f},
                        {-15,6,5,0.05f,2f},
                        {45,-1,40,0.01f,3f},
    };
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
	public override void SetSprites(EnemyInfo ei){
        hp=1000;
        EnemySpawner.boss=true;
        name+="Boss";
        GameObject g=new GameObject("tail");
        g.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        g.transform.parent=transform;
        g.transform.localPosition=new Vector3(0,-2.4f,-0.1f);
        for (int i = 0; i < turrets.Length; i++)
        {
            g=new GameObject("enemy");
            g.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
            turrets[i]=g.AddComponent<MiniTurret>();
            turrets[i].shotId=ei.bulletsID[0];
            turrets[i].trailID=ei.particleID[0];
            turrets[i].impactID=ei.particleID[1];
            turrets[i].transform.parent=transform;
        }
        turrets[0].transform.localPosition=new Vector3(1.2f,-0.8f,-0.1f);
        turrets[1].transform.localPosition=new Vector3(-1.2f,-0.8f,-0.1f);
        update=Intro;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
    }
    void Intro(){
        if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*4,0);
        else {
            update=ZigZag;
            update+=TurretUpdate;
        }
    }
    void ZigZag(){
        transform.position=Vector3.MoveTowards(transform.position,pos,Time.deltaTime*2);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,rot,Time.deltaTime*30);
        if(transform.position==pos){
            pos.x*=-1;
            rot=Quaternion.Euler(0,0,(pos.x>0?-1:1)*45);
        }
    }
    void Face(){
        pos.x=target.position.x;
        pos.y*=0.99f;
        transform.position=Vector3.MoveTowards(transform.position,pos,Time.deltaTime*2);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,rot,Time.deltaTime*360);
    }
    void Dying(){
        transform.localScale=Vector3.MoveTowards(transform.localScale,Vector3.one/2,Time.deltaTime/2);
        if(transform.localScale==Vector3.one/2){
            EnemySpawner.boss=false;
            Destroy(gameObject);
        }
		if(Time.time%1f<0.1f)ParticleManager.Emit(1,transform.position+Random.onUnitSphere*transform.localScale.sqrMagnitude,1);
    }
    protected override void Die()
	{
		update=Dying;
		EnemySpawner.points[killerid]+=1000;
        turrets[0].enabled=turrets[1].enabled=false;
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
	}
    void TurretUpdate(){
        if(shootTime<Time.time){
            target=GetPlayer();
            shootTime=Time.time+patterns[patternId,4];
            turrets[0].Prepare(patterns[patternId,0],patterns[patternId,1],patterns[patternId,2],patterns[patternId,3]);
            turrets[1].Prepare(-patterns[patternId,0],-patterns[patternId,1],patterns[patternId,2],patterns[patternId,3]);
            patternId++;
            if(patternId>=patterns.GetLength(0))patternId=0;
        }
    }
    private new void OnCollisionEnter2D(Collision2D col)
	{
		if(update!=Intro && update!=Dying) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(16,col.collider.transform.position,1);
        if(hp<200 && !face){
            face=true;
            update=Face;
            update+=TurretUpdate;
            turrets[0].variant=turrets[1].variant=0.5f;
            rot=Quaternion.Euler(0,0,180);
        }
	}
}
