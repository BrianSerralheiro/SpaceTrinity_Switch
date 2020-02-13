using UnityEngine;
using System.Collections.Generic;

public class Archangel : EnemyBase
{
    Transform halo,target;
    Transform[] wings=new Transform[2];
    Vector3 point=Vector3.up*Scaler.sizeY;
    List<Transform> rings=new List<Transform>();
    Sprite face;
    Core chin;
    float timer;
    int shotId;
    Del update,check;
    public override void SetSprites(EnemyInfo ei)
	{
		BossWarning.Show();
		SoundManager.Play(2);
		name+="Boss";
		damageEffect = true;
		EnemySpawner.boss=true;
		hp=1800;
		if(PlayerInput.Conected(1))hp=(int)(hp*ei.lifeproportion);
        GameObject go=new GameObject("halo");
        SpriteRenderer sr=go.AddComponent<SpriteRenderer>();
        sr.sprite=ei.sprites[1];
        halo=go.transform;
        halo.parent=transform;
        halo.Translate(0,0,1);
        GetComponent<SpriteRenderer>().color=Color.blue;
        gameObject.AddComponent<Core>().Set(Color.white,Color.clear);
        face=ei.sprites[2];
        for(int i = 0; i < 2; i++)
        {
            go=new GameObject("wing"+i);
            sr=go.AddComponent<SpriteRenderer>();
            sr.sprite=ei.sprites[3];
            sr.flipX=i==1;
            go.AddComponent<Core>().Set(Color.white,Color.clear);
            go.transform.parent=transform;
            go.transform.Translate(0,-3,0.5f);
            go.transform.Rotate(0,0,35*(i==0?1:-1));
            wings[i]=go.transform;
        }
        go=new GameObject("chin");
        chin=go.AddComponent<Core>().Set(ei.sprites[4],Color.clear);
        chin.white=new Color(0,1,1);
        go.transform.parent=transform;
        go.transform.Translate(0,-3.37f,0.01f);
        shotId=ei.bulletsID[0];
        update=Intro;
        check=Check;
    }
    void Intro(){
        GetComponent<Core>().Set(1);
        timer+=Time.deltaTime;
        transform.Translate(0,-Time.deltaTime*2,0);
        halo.Rotate(0,0,Mathf.Sin(timer)*180*Time.deltaTime);
        if(transform.position.y<0){
            update=Chase;
            timer=Time.time+1;
            target=GetPlayer();
        }
    }
    void Check(){
        if(hp<1500){
            damageTimer=0;
            GetComponent<Core>().Set(0);
            GetComponent<Collider2D>().enabled=false;
            timer=0;
            _renderer.sprite=face;
            _renderer.color=Color.clear;
            update=Revealing;
            check=null;
        }
    }
    void Revealing(){
        transform.position=Vector3.MoveTowards(transform.position,Vector3.up*Scaler.sizeY/2,Time.deltaTime*2);
        if(transform.position==Vector3.up*Scaler.sizeY/2)
            timer+=Time.deltaTime;
        halo.Rotate(0,0,Mathf.Sin(Time.time)*180*Time.deltaTime);
        if(timer>1){
            foreach (Core c in GetComponentsInChildren<Core>())
                c.Add(Time.deltaTime/4);
            chin.Add(Time.deltaTime/4);
        }
        if(timer>4){
            halo.rotation=Quaternion.RotateTowards(halo.rotation,Quaternion.identity,180*Time.deltaTime);
            for(int i = 0; i < 2; i++)
                wings[i].rotation=Quaternion.RotateTowards(wings[i].rotation,Quaternion.identity,40*Time.deltaTime);
        }
        if(timer>5){
            BoxCollider2D box=GetComponent<BoxCollider2D>();
            box.enabled=true;
            box.size=new Vector2(2,4);
            box.offset=new Vector2(0,-3);
            update=Creating;
            chin.Set(Color.white,chin.white);
        }
    }
    void Shot(){
        for (int i = 0; i < 12; i++)
        {
            GameObject go=new GameObject("enemybullet");
            go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
            go.AddComponent<BoxCollider2D>();
            Bullet bu=go.AddComponent<Bullet>();
            bu.owner=name;
            bu.spriteID=shotId;
            bu.bulleSpeed=10;
            bu.Timer(4);
            go.transform.position=halo.position-Vector3.forward/10;
            go.transform.rotation=Quaternion.Euler(0,0,i*30);
        }
    }
    void Chase(){
        transform.position=Vector3.MoveTowards(transform.position,target.position,Time.deltaTime*4);
        halo.rotation=Quaternion.RotateTowards(halo.rotation,Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.down,target.position-transform.position)),180*Time.deltaTime);
        if(timer<=Time.time){
            timer=Time.time+2;
            update=Shooting;
        }
    }
    private new void OnCollisionEnter2D(Collision2D col)
	{
        if(!col.otherCollider.enabled)return;
        if(hp<800 && update.GetInvocationList().Length<2){
            update+=Halo;
            halo.parent=null;
            halo.gameObject.AddComponent<CircleCollider2D>().radius=1.2f;
        }
		if(update!=Intro && update!=Dying)base.OnCollisionEnter2D(col);
		else ParticleManager.Emit(16,col.collider.transform.position,1);
        check?.Invoke();
	}
    void Shooting(){
        halo.Rotate(0,0,30*Time.deltaTime);
        if(timer<Time.time){
            Shot();
            timer=Time.time+2;
            update=Chase;
        }
    }
    void Creating(){
        if(halo.parent)
            halo.Rotate(0,0,Mathf.Sin(Time.time)*360*Time.deltaTime);
        if(timer<=Time.time)
            Create(new Vector3(-Scaler.sizeX/4+Scaler.sizeX/4*rings.Count,transform.position.y));
    }
    void Sending(){
        if(halo.parent)
            halo.Rotate(0,0,Mathf.Sin(Time.time)*270*Time.deltaTime);
        if(!target){
            if(rings.Count==0){
                update-=Sending;
                update+=Aim;
                timer=Time.time+2;
            }
            else{
                target=rings[Random.Range(0,rings.Count)];
                timer=0;
                rings.Remove(target);
            }
        }
        else{
            timer+=Time.deltaTime*10;
            target.Translate(0,-Time.deltaTime*timer,0);
            if(target.position.y<-Scaler.sizeY-2)Destroy(target.gameObject);
        }
    }
    void Halo(){
        halo.Rotate(0,0,Mathf.Sin(Time.time)*180*Time.deltaTime);
        if(halo.position==point){
            Shot();
            point.Set(Random.Range(-Scaler.sizeX/2+2,Scaler.sizeX/2-2),Random.Range(Scaler.sizeY/4,Scaler.sizeY-1),0);
        }
        halo.position=Vector3.MoveTowards(halo.position,point,Time.deltaTime);
    }
    void Aim(){
        if(halo.parent)halo.rotation=Quaternion.RotateTowards(halo.rotation,Quaternion.identity,30*Time.deltaTime);
        chin.Min(Time.deltaTime*2);
        Vector3 vector=GetPlayer(transform.position).position;
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.down,vector-transform.position)),190*Time.deltaTime);
        for(int i = 0; i < 2; i++)
                wings[i].localRotation=Quaternion.RotateTowards(wings[i].localRotation,Quaternion.identity,40*Time.deltaTime);
        if(timer<Time.time){
            update-=Aim;
            update+=Dive;
            timer=0;
        }
    }
    void Dive(){
        if(halo.parent)
            halo.Rotate(0,0,Mathf.Sin(Time.time)*360*Time.deltaTime);
        timer+=Time.deltaTime;
        chin.Add(Time.deltaTime*2);
        transform.Translate(0,-timer*6*Time.deltaTime,0);
        wings[0].localRotation=Quaternion.RotateTowards(wings[0].localRotation,Quaternion.Euler(0,0,35),70*Time.deltaTime);
        wings[1].localRotation=Quaternion.RotateTowards(wings[1].localRotation,Quaternion.Euler(0,0,-35),70*Time.deltaTime);
        if(timer>2){
            update-=Dive;
            if(Random.value<0.7f){
                timer=Time.time+2;
                update+=Aim;
            }
            else
                update+=Resting;
        }
    }
    void Resting(){
        if(halo.parent)halo.rotation=Quaternion.RotateTowards(halo.rotation,Quaternion.identity,60*Time.deltaTime);
        transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.identity,90*Time.deltaTime);
        transform.position=Vector3.MoveTowards(transform.position,Vector3.up*Scaler.sizeY/2,Time.deltaTime*3);
        chin.Min(Time.deltaTime*2);
        for(int i = 0; i < 2; i++)
                wings[i].localRotation=Quaternion.RotateTowards(wings[i].localRotation,Quaternion.identity,40*Time.deltaTime);
        if(transform.position==Vector3.up*Scaler.sizeY/2){
            update-=Resting;
            update+=Creating;
        }
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
    }
    public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+4,0);
	}
    void Create(Vector3 v){
        target=null;
        GameObject go=new GameObject("enemy");
        go.transform.position=v;
        rings.Add(go.transform);
        CircleCollider2D col=go.AddComponent<CircleCollider2D>();
        col.radius=0.5f;
        col.offset=new Vector2(4,0);
        col=go.AddComponent<CircleCollider2D>();
        col.radius=0.5f;
        col.offset=new Vector2(-4,0);
        LineRenderer ring=go.AddComponent<LineRenderer>();
        ring.loop=true;
        ring.positionCount=30;
        float rad=2*Mathf.PI/ring.positionCount;
        // ring.widthMultiplier=0.1f;
        ring.useWorldSpace=false;
        for (int i = 0; i < ring.positionCount; i++)
        {
            ring.SetPosition(i,new Vector3(Mathf.Cos(i*rad)*4,Mathf.Sin(i*rad)*2,Mathf.Sin(i*rad)));
        }
        timer=Time.time+0.2f;
        if(rings.Count>=3){
            update-=Creating;
            update+=Sending;
        }
    }
    protected override void Die()
	{
        update=Dying;
        foreach (Transform t in rings)
        {
            Destroy(t.gameObject);
        }
        if(target)Destroy(target.gameObject);
        GetComponent<Collider2D>().enabled=false;
        timer=Time.time+3;
    }
    void Dying(){
        halo.Translate(0,-Time.deltaTime,0,Space.World);
		if(Time.time%1f<0.1f)ParticleManager.Emit(0,transform.position+Random.onUnitSphere,1);
        if(timer<Time.time)Loader.Scene("SelectionTest");
    }
}
