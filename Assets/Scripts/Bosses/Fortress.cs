using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : EnemyBase
{
    Del update;
    float treadspeed=-1,turretTimer,turretShot,sequence,cannonTime,rotationSpeed,aim;
    int turretID,shotId,shotBig;
    bool flag;
    MiniTurret[] turrets=new MiniTurret[4];
    Transform cannon,target;
    Transform[] tracks=new Transform[4];
    void Start()
    {
        
    }
	public override void Position(int i)
	{
		transform.position=new Vector3(0,Scaler.sizeY+5,0);
	}
	public override void SetSprites(EnemyInfo ei){
        hp=2000;
		BossWarning.Show();
		name+="Boss";
		SoundManager.Play(2);
		damageEffect = true;
        EnemySpawner.boss=true;
        update=Intro;
        GameObject g=new GameObject("Cannon");
        _renderer=g.AddComponent<SpriteRenderer>();
        _renderer.sprite=ei.sprites[1];
        cannon=g.transform;
        cannon.parent=transform;
        cannon.Translate(0,2,-0.1f);
        for (int j = 0; j < 2; j++)
        {
            
            g=new GameObject("Tread"+j);
            g.AddComponent<SpriteMask>().sprite=ei.sprites[2];
            Transform t=g.transform;
            t.parent=transform;
            t.localPosition=new Vector3(-2.8f+j*5.6f,-4,0.1f);
            for (int i = 0; i < 2; i++)
            {
                g=new GameObject("treadpiece");
                SpriteRenderer rend=g.AddComponent<SpriteRenderer>();
                rend.sprite=ei.sprites[2];
                rend.maskInteraction=SpriteMaskInteraction.VisibleInsideMask;
                tracks[j*2+i]=g.transform;
                g.transform.parent=t;
                g.transform.localPosition=new Vector3(0,0.9f*i,0);
            }
        }
        shotId=ei.bulletsID[0];
        shotBig=ei.bulletsID[2];
        for (int i = 0; i < 4; i++)
        {
            g=new GameObject("turret"+i);
            turrets[i]=g.AddComponent<MiniTurret>();
            turrets[i].shotId=shotId;
            g.AddComponent<SpriteRenderer>().sprite=ei.sprites[3];
            g.transform.parent=transform;
            g.transform.localPosition=new Vector3(-3.2f+6.4f*(i%2),2.1f-2.7f*(i/2),-0.05f);
            g.transform.localScale=Vector3.one*1.5f;
        }
    }
    void Dying(){
        transform.Translate(0,-2*Time.deltaTime,0);
		if(transform.position.y<-Scaler.sizeY/2)Loader.Scene("SelectionTest");
		if(Time.time%1f<0.1f)ParticleManager.Emit(1,transform.position+Random.onUnitSphere*transform.localScale.sqrMagnitude,1);
    }
    protected override void Die()
	{
		update=Dying;
		Locks.Boss(3,true);
		EnemySpawner.points[killerid]+=1000;
        tracks[0].Rotate(0,0,35);
        tracks[1].Rotate(0,0,35);
        tracks[2].Rotate(0,0,-35);
        tracks[3].Rotate(0,0,-35);
        foreach (MiniTurret turret in turrets)
        {
            turret.enabled=false;
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled=false;
		}
	}
    void Intro(){
        UpdateTrack();
        target=GetPlayer();
        if(transform.position.y>Scaler.sizeY/2)transform.Translate(0,-Time.deltaTime*2,0);
        else{
            treadspeed=3;
            update=TurretUpdate;
            update+=UpdateCannon;
            update+=UpdateTrack;
        }
    }
    void UpdateCannon(){
        // Vector3 v=target.position-cannon.position;
        // v.z=0;
        // cannon.Rotate(Vector3.Cross(-cannon.up,v)*20*Time.deltaTime);
        float delta=Vector2.SignedAngle(-cannon.up,target.position-cannon.position);
        rotationSpeed=Mathf.MoveTowards(rotationSpeed,delta>0?15:-15,Time.deltaTime*15);
        aim+=rotationSpeed*Time.deltaTime;
        cannon.rotation=Quaternion.Euler(0,0,aim);
        if(cannonTime<Time.time && Mathf.Abs(delta)<10){
            Shot();
        }
    }
    void TurretUpdate(){
        if(turretTimer<Time.time){
            switch(turretID){
                case 0:
                    if(!turrets[0].enabled){
                        turretID++;
                        break;
                    }
                    if(turretShot<Time.time){
                        if(sequence==0){
                            turrets[0].Prepare(-45,2,45,0.01f);
                            turretShot=Time.time+3;
                            sequence=1;
                        }else{
                            turrets[0].Prepare(45,-2,45,0.01f);
                            sequence=0;
                            turretTimer=Time.time+10;
                            turretID=Random.Range(0,4);
                        }
                    }
                    break;
                case 1:
                    if(!turrets[1].enabled){
                        turretID++;
                        break;
                    }
                    if(turretShot<Time.time){
                        turrets[1].Prepare(180,17,60,0.01f);
                        turretTimer=Time.time+10;
                        turretID=Random.Range(0,4);
                    }
                    break;
                case 2:
                    if(!turrets[2].enabled){
                        turretID++;
                        break;
                    }
                    if(turretShot<Time.time){
                        turrets[2].Prepare(-45,0.9f,100,0.01f);
                        turretTimer=Time.time+15;
                        turretID=Random.Range(0,4);
                    }
                    break;
                case 3:
                    if(!turrets[3].enabled){
                        turretID=0;
                        break;
                    }
                    if(turretShot<Time.time){
                        switch (sequence)
                        {
                            case 0:
                                turrets[3].Prepare(-10,2,10,0.01f);
                                sequence++;
                                turretShot=Time.time+1;
                            break;
                            case 1:
                                turrets[3].Prepare(15,-2,15,0.01f);
                                sequence++;
                                turretShot=Time.time+1;
                            break;
                            case 2:
                                turrets[3].Prepare(-20,2,20,0.01f);
                                sequence=0;
                                turretTimer=Time.time+10;
                                turretID=Random.Range(0,4);
                            break;

                        }
                    }
                    break;
            }
        }
    }
    void BaseUpdate(){
        if(turretTimer<Time.time+9 && turretTimer>Time.time+1 && cannonTime<Time.time){
            if(flag){
                for (int i = 0; i < 12; i++)
                {
                    Shot(i*30f);
                }
            }
            else ShotBig();
            cannonTime=+Time.time+2;
            flag=!flag;
        }
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        update?.Invoke();
    }
    void UpdateTrack(){
        foreach(Transform t in tracks)
        {
            t.Translate(0,treadspeed*Time.deltaTime,0);
            if(t.localPosition.y>0.9f)t.Translate(0,-0.9f*2,0);
            if(t.localPosition.y<-0.9f)t.Translate(0,0.9f*2,0);
        }
    }
    private new void OnCollisionEnter2D(Collision2D col)
	{
		if(update!=Intro && update!=Dying) base.OnCollisionEnter2D(col);
		else
			ParticleManager.Emit(16,col.collider.transform.position,1);
        if(hp<1500 && cannon){
            ParticleManager.Emit(0,cannon.position,1);
            Destroy(cannon.gameObject);
            _renderer=GetComponent<SpriteRenderer>();
            update-=UpdateCannon;
            update+=BaseUpdate;
        }
	}
    void Shot(float f){
        GameObject game=new GameObject("enemybullet");
        game.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        game.AddComponent<BoxCollider2D>();
        Bullet bu=game.AddComponent<Bullet>();
        bu.spriteID=shotId;
        bu.bulleSpeed=12;
        bu.owner=name;
        game.transform.position=transform.position-Vector3.forward/10;
        game.transform.rotation=Quaternion.Euler(0,0,f);
    }
    void Shot(){
        cannonTime=+Time.time+2;
        for (int i = 0; i < 2; i++)
        {
            GameObject game=new GameObject("enemybullet");
            game.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotBig];
            game.AddComponent<BoxCollider2D>();
            Bullet bu=game.AddComponent<Bullet>();
            bu.spriteID=shotBig;
            bu.bulleSpeed=10;
            bu.owner=name;
            game.transform.position=cannon.position-cannon.up*5+(flag?cannon.right:-cannon.right)-Vector3.forward/10;
            flag=!flag;
            game.transform.up=-cannon.up;
            game.transform.localScale=Vector3.one*2;
        }
        target=GetPlayer();
    }
    void ShotBig(){
        GameObject game=new GameObject("enemybullet");
        game.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotBig];
        game.AddComponent<BoxCollider2D>();
        Bullet bu=game.AddComponent<Bullet>();
        bu.spriteID=shotBig;
        bu.bulleSpeed=8;
        bu.owner=name;
        game.transform.position=transform.position-Vector3.up*4-Vector3.forward/10;
        game.transform.localScale=Vector3.one*5;
        game.transform.up=Vector3.down;
    }
}
