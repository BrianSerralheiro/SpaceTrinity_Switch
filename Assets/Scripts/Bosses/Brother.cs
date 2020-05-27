using UnityEngine;
public class Brother : EnemyBase
{
    Transform arml,armr,elbow,rocket,target;
    TrailRenderer slash;
    LineRenderer laser;
    BoxCollider2D col;
    float angleR,angleL,time,armSpeed=30,jet=1;
    int shotID,jetID;
    Del update;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=1400;
        name+="Boss";
        EnemySpawner.boss=true;
        damageEffect=true;
        shotID=ei.bulletsID[0];
        jetID=ei.particleID[0];
        GameObject go=new GameObject("Head");
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        go.transform.parent=transform;
        go.transform.localPosition=new Vector3(0,2.3f,-0.01f);
        go=new GameObject("armLbig");
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[2];
        go.AddComponent<PolygonCollider2D>();
        arml=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=new Vector3(-0.8f,1.5f,0.01f);
        go=new GameObject("slash");
        slash=go.AddComponent<TrailRenderer>();
        slash.enabled=false;
        slash.time=0.3f;
        slash.startWidth=0.7f;
        slash.endWidth=0.1f;
        slash.material=new Material(Shader.Find("Sprites/Default"));
        go.transform.parent=arml;
        go.transform.localPosition=new Vector3(-3.5f,-1f,0.01f);
        go=new GameObject("armRbig");
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
        armr=go.transform;
        go.transform.parent=transform;
        go.transform.localPosition=new Vector3(0.8f,1.5f,0.01f);
        go=new GameObject("enemylaser");
        laser=go.AddComponent<LineRenderer>();
        laser.material=slash.material;
        laser.startColor=Color.red;
        laser.SetPosition(0,Vector3.zero);
        laser.SetPosition(1,new Vector3(0,-Scaler.sizeX));
        laser.useWorldSpace=false;
        laser.widthMultiplier=0.1f;
        col=go.AddComponent<BoxCollider2D>();
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[4];
        laser.enabled=false;
        col.enabled=false;
        elbow=go.transform;
        go.transform.parent=armr;
        go.transform.localPosition=new Vector3(2.5f,1,-0.01f);
        go=new GameObject("rocketbig");
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[5];
        go.AddComponent<PolygonCollider2D>();
        rocket=go.transform;
        go.transform.parent=armr;
        go.transform.localPosition=new Vector3(2.5f,1,0.01f);
        update=Intro;
    }
    void Intro(){
        Jet(2);
        transform.Translate(0,-Time.deltaTime*2,0);
        if(arml.rotation==Quaternion.Euler(0,0,angleL)){
            if(angleL==15)angleL=-15;
            else angleL=15;
        }
        if(armr.rotation==Quaternion.Euler(0,0,angleR)){
            if(angleR==15)angleR=-15;
            else angleR=15;
        }
        if(transform.position.y<Scaler.sizeY/2)update=Wait;
    }
    void Wait(){
        if(!target)target=GetPlayer();
        Jet(1);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,Time.deltaTime*7);
        if(Mathf.Abs(target.position.x-transform.position.x)>5)transform.Translate((transform.position.x>target.position.x?-2:2)*Time.deltaTime,0,0);
        if(arml.rotation==Quaternion.Euler(0,0,angleL)){
            armSpeed=30;
            if(angleL==15)angleL=-15;
            else angleL=15;
        }
        if(armr.rotation==Quaternion.Euler(0,0,angleR)){
            if(angleR==5)angleR=-5;
            else angleR=5;
        }
        if(transform.position.y<Scaler.sizeY/2){
            transform.Translate(0,Time.deltaTime*3,0);
            if(transform.position.y>Scaler.sizeY/2)time=Time.time+2;
        }
        else if(time<Time.time){
            switch(Random.Range(0,3)){
                case 0:
                    update=Fall;
                    angleL=-15;
                    angleR=0;
                    slash.enabled=true;
                    break;
                case 1:
                    update=Laser;
                    angleR=0;
                    break;
                case 2:
                    update+=Shot;
                    update+=Shot;
                    update+=Shot;
                    time=Time.time+1;
                    break;
            }
        }
    }
    void Fall(){
        SlowFall();
        Jet(0);
        fallSpeed=Mathf.MoveTowards(fallSpeed,-6,Time.deltaTime*6);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,35),Time.deltaTime*7);
        if(transform.position.y<-Scaler.sizeY/4){
            update=Slash;
            angleL=90;
            angleR=-15;
            armSpeed=180;
        }
    }
    void Slash(){
        fallSpeed=Mathf.MoveTowards(fallSpeed,0,Time.deltaTime*3);
        SlowFall();
        jet=5;
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,5),Time.deltaTime*10);
        if(arml.rotation==Quaternion.Euler(0,0,angleL)){
            update=Wait;
            slash.enabled=false;
            angleL=15;
        }
    }
    void Laser(){
        if(arml.rotation==Quaternion.Euler(0,0,angleL)){
            if(angleL==15)angleL=-15;
            else angleL=15;
        }
        if(!laser.enabled){
            elbow.rotation=Quaternion.RotateTowards(elbow.rotation,Quaternion.Euler(0,0,45),30*Time.deltaTime);
            if(elbow.rotation==Quaternion.Euler(0,0,45))laser.enabled=col.enabled=true;
        }
        else{
            elbow.rotation=Quaternion.RotateTowards(elbow.rotation,Quaternion.Euler(0,0,-15),90*Time.deltaTime);
            if(elbow.rotation==Quaternion.Euler(0,0,-15)){
                laser.enabled=col.enabled=false;
                update=Wait;
                time=Time.time+2;
            }
        }
    }
    void Shot(){
        if(time<Time.time+1f){
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotID];
            go.AddComponent<CircleCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.spriteID=shotID;
            bu.bulletSpeed=8;
            bu.Timer(5);
            bu.owner=name;
            go.transform.position=transform.GetChild(0).position;
            go.transform.up=target.position-go.transform.position;
            update-=Shot;
            time=Time.time+2;
        }
    }
	protected new void Update(){
		if(Ship.paused)return;
		base.Update();
        update?.Invoke();
        ParticleManager.Emit(jetID,rocket.position-rocket.up*3.7f+rocket.right*1.1f,rocket.up,(int)jet);
        arml.rotation=Quaternion.RotateTowards(arml.rotation,Quaternion.Euler(0,0,angleL),armSpeed*Time.deltaTime);
        armr.rotation=Quaternion.RotateTowards(armr.rotation,Quaternion.Euler(0,0,angleR),10*Time.deltaTime);
    }
    private new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name.Contains("enemylaser") || col.otherCollider.name.Contains("arm") || col.otherCollider.name.Contains("rocket"))return;
		if(update!=Intro)base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(3,col.collider.transform.position,1);
	}
    void Jet(int i){
        jet=Mathf.MoveTowards(jet,i,Time.deltaTime*2);
    }
    
	protected override void Die()
	{
		update=Dying;
		EnemySpawner.points[killerid]+=500;
        rocket.parent=null;
        time=Time.time+4;
        armSpeed=10;
        laser.enabled=false;
		foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
	}
	void Dying(){
        Jet(10);
        rocket.Rotate(0,0,25*Time.deltaTime);
        transform.Rotate(0,0,-15*Time.deltaTime);
        transform.Translate(0, fallSpeed*Time.deltaTime,0);
        rocket.Translate(0,-fallSpeed*Time.deltaTime,0);
        fallSpeed=Mathf.MoveTowards(fallSpeed,-4,Time.deltaTime*2);
        if(time<Time.time){
            Destroy(gameObject);
            Destroy(rocket.gameObject);
            EnemySpawner.boss=false;
        }
    }
}
