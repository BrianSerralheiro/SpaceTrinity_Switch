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
	public override void SetSprites(EnemyInfo ei){
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
        path=paths[0];
    }
	public override void Position(int i){
		base.Position(i);
        position=transform.position;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(path.Finished()){
            timer-=Time.deltaTime;
			transform.Rotate(Vector3.Cross(-transform.up,paths[1].GetNode0(position.x>0))*60*Time.deltaTime);
            if(cicles<=0)Die();
            Debug.Log(path.nodes.Length);
            if(timer<0)Shot();
        }
        else{
			transform.Rotate(Vector3.Cross(-transform.up,path.Directiom(position.x>0))*360*Time.deltaTime);
            transform.position=position+BulletPath.Next(ref path,position.x>0);
        }
    }
    void Shot(){
        GameObject go=new GameObject("enemybullet");
        go.transform.position=transform.position-transform.up;
        PathBullet bu=go.AddComponent<PathBullet>();
        bu.owner="enemy";
        bu.bulleSpeed=8;
        bu.spriteID=shotId;
        bu.path=paths[1];
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        go.AddComponent<BoxCollider2D>();
        go.transform.up=-transform.up;
        if(counter--==0){
            timer=reload;
            counter=shots;
            if(--cicles<=0){
                position=transform.position;
                path=paths[2];
                Debug.Log(path.nodes.Length);
            }
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
