using UnityEngine;

public class Baron : EnemyBase
{
    Del update;
    Transform target;
    Transform[] helix=new Transform[2];
    Vector3 pos=new Vector3(0,Scaler.sizeY/2+2);
    int bullets,shotId,trailID,impactID;
    float dir,closest=10,rotation=90,time;
    public override void SetSprites(EnemyInfo ei)
	{
		name+="Boss";
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1800;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
        CircleCollider2D col=gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger=true;
        col.radius=5;
        for (int i = 0; i < 2; i++)
        {
            GameObject go =new GameObject("helix");
            go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
            go.transform.parent=transform;
            go.transform.localPosition=new Vector3(-1f+i*2f,0,-0.1f);
            helix[i]=go.transform;
        }
        shotId=ei.bulletsID[0];
        trailID=ei.particleID[0];
        impactID=ei.particleID[1];
        Instantiate(ei.particles[2],transform);
        update=Intro;
    }
    void Intro(){
        rotation=360;
        transform.Translate(0,-Time.deltaTime*18,0);
        if(transform.position.x>Scaler.sizeX/2+2){
            transform.position=new Vector3(Scaler.sizeX/2+2,Scaler.sizeY/2,0);
            transform.rotation=Quaternion.Euler(0,0,-90);
        }
        if(transform.position.x<-Scaler.sizeX/2-2){
            transform.position=new Vector3(0,Scaler.sizeY+2,0);
            transform.rotation=Quaternion.identity;
            update=Back;
        }
    }
    void Avoiding(){
        rotation=180;
        transform.Translate(dir*Time.deltaTime,0,0,Space.World);
        if(time<Time.time){
            switch (Random.Range(0,4))
            {
                case 0:
                    update=Bombing;
                    break;
                case  1:
                    update=Follow;
                    target=GetPlayer(transform.position);
                    break;
                case  2:
                    update=Aiming;
                    bullets=4;
                    target=GetPlayer();
                    break;

            }
        }
    }
    void Bombing(){
        rotation=270;
        transform.Translate(0,-Time.deltaTime*8,0);
        if(time<Time.time){
            GameObject g=new GameObject("enemy");
            g.AddComponent<CircleCollider2D>().radius=3;
            g.transform.position=_renderer.transform.position+Vector3.forward/5;
            ParticleManager.Emit(0,g.transform.position,1,2);
            time=Time.time+0.8f;
            Destroy(g,0.6f);
        }
        if(transform.position.y<-Scaler.sizeY-3){
            transform.position=Vector3.up*(Scaler.sizeY+2);
            time=Time.time+2;
            update=Back;
        }
    }
    void Back(){
        rotation=180;
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,90*Time.deltaTime);
        if(time<Time.time)transform.Translate(0,-Time.deltaTime,0);
        if(transform.position.y<Scaler.sizeY/2){
            update=Avoiding;
            time=Time.time+3;
        }
    }
    void Follow(){
        Vector3 v=target.position-transform.position;
        v.z=0;
        transform.Rotate(Vector3.Cross(-transform.up,v)*Time.deltaTime*30);
        transform.Translate(0,-Time.deltaTime*3,0);
        if(Vector3.Distance(target.position,transform.position)<8){
            bullets=50;
            update=Shoting;
        }
    }
    void Shoting(){
        rotation=180;
        if(bullets>0){
            if(time<Time.time)Shot();
        }
        else{
            transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,30*Time.deltaTime);
            transform.Translate(0,-Time.deltaTime*8,0);
            if(transform.position.y<-Scaler.sizeY-3){
                transform.position=Vector3.up*(Scaler.sizeY+2);
                time=Time.time+2;
                update=Back;
            }
        }
    }
    void Aiming(){
        if(transform.position==pos){
            Vector3 v=target.position-transform.position;
            v.z=0;
            transform.Rotate(Vector3.Cross(-transform.up,v)*Time.deltaTime*30);
            if(time<Time.time)Missile();
            if(bullets<=0)
                update=Back;
        }
        else{
            transform.position=Vector3.MoveTowards(transform.position,pos,Time.deltaTime*2);
        }
    }
    void Missile(){
        time=Time.time+2f;
        GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId+2];
		go.AddComponent<BoxCollider2D>();
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shotId+2;
        b.maxSpeed=1;
		go.transform.position=transform.position-transform.up;
		go.transform.up=-transform.up;
        target=GetPlayer();
        bullets--;
    }
    void Shot()
    {
        time=Time.time+0.05f;
        GameObject go = new GameObject("enemybullet");
		go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
		go.AddComponent<BoxCollider2D>();
		Bullet b= go.AddComponent<Bullet>();
		b.owner=name;
		b.spriteID=shotId;
		b.particleID=trailID;
		b.impactID=impactID;
		go.transform.position=transform.position+transform.right*(-0.5f+bullets%2)-transform.up;
		go.transform.up=-transform.up;
        bullets--;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
        foreach (Transform t in helix)
        {
            t.Rotate(0,0,rotation*Time.deltaTime,Space.Self);
        }
        dir=0;
        closest=10;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.name.Contains("playerbullet")){
            float d=Vector3.Distance(col.transform.position,transform.position);
            if(d<closest){
                closest=d;
                if(col.transform.position.x>transform.position.x)dir=-4;
                else dir=4;
            }
        }
    }
    public override void Position(int i)
	{
		transform.position=new Vector3(-Scaler.sizeX/2-1,Scaler.sizeY-2,0);
        transform.rotation=Quaternion.Euler(0,0,90);
	}
	void Dying(){
        transform.Translate(0,-Time.deltaTime*8,0);
        transform.Rotate(0,0,(Random.value-0.5f)*10);
        transform.localScale=Vector3.MoveTowards(transform.localScale,Vector3.one/5,Time.deltaTime);
        if(transform.localScale==Vector3.one/5){
            EnemySpawner.boss=false;
            Destroy(gameObject);
        }
		if(Time.time%1f<0.1f)ParticleManager.Emit(1,transform.position+Random.onUnitSphere*transform.localScale.sqrMagnitude,1);
    }
	protected override void Die()
	{
		update=Dying;
        rotation=360;
		EnemySpawner.points[killerid]+=500;
		foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
	}
    private new void OnCollisionEnter2D(Collision2D col)
	{
		if(update==Intro || (update==Back && transform.position.y>Scaler.sizeY))return;
        base.OnCollisionEnter2D(col);
	}
}
