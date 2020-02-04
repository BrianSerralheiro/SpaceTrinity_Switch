using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : EnemyBase
{

    float timer,delay,reload;
    int shotId,counter,shots,cicles;
    static BulletPath[] paths;
    BulletPath path;
	Vector3 position;
    Helix helix;
    static BulletPath around;
	public override void SetSprites(EnemyInfo ei)
    {
        transform.localScale = Vector3.one * 0.8f;
        hp=100;
        GameObject go=new GameObject("Helix");
        go.transform.parent=transform;
        go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
        helix=go.AddComponent<Helix>();
        go.AddComponent<BoxCollider2D>();
        shotId=ei.bulletsID[0];
        MultiPathEnemy enemy=(MultiPathEnemy)ei;
        counter=shots=enemy.shotCount;
        delay=enemy.shotDelay;
        cicles=enemy.cicles;
        timer=reload=enemy.reloadTime;
        if(paths==null)paths=enemy.paths;
        if(around.nodes==null)around.Set(10,new Vector3[]{new Vector3(0,0),new Vector3(Scaler.sizeX,0),new Vector3(Scaler.sizeX,-Scaler.sizeY*2+2),new Vector3(-1,-Scaler.sizeY*2+2)});
    }
	public override void Position(int i){
		base.Position(i);
        if(i==0)transform.position=new Vector3(-Scaler.sizeX/2-1,Scaler.sizeY-1);
        if(i==19)transform.position=new Vector3(Scaler.sizeX/2+1,Scaler.sizeY-1);
        position=transform.position;
        i=i<10?i:(19-i);
        if(i==0)path=around;
        else path=paths[i];
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(path.Finished()){
            Die();
        }
        else{
            timer-=Time.deltaTime;
			transform.Rotate(Vector3.Cross(-transform.up,transform.position - GetPlayer(transform.position).position)*360*Time.deltaTime);
            if(timer<0)Shot();
            transform.position=position+BulletPath.Next(ref path,position.x>0);
        }
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-transform.up;
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulleSpeed=18;
        bu.spriteID=shotId;
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-transform.up;
        if(counter--==0){
            timer=reload;
            counter=shots;
        }else
            timer=delay;
    }
    
    protected override void Die(){
        if(hp<=0){
            helix.transform.parent=null;
            Bullet b=helix.gameObject.AddComponent<Bullet>();
            b.enabled=false;
            b.damage=40;
            b.owner="hel";
            helix.time=Time.time+1;
        }
        base.Die();
    }
}
