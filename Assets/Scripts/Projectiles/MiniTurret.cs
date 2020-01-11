using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTurret : MonoBehaviour
{
    float min,max,timer,shooterTimer;
    int shotId;
    void Perepare(Sprite s,float mi,float ma,float t,int id)
    {
        gameObject.AddComponent<SpriteRenderer>().sprite=s;
        gameObject.AddComponent<BoxCollider2D>();
        min=mi;
        max=ma;
        shooterTimer=t;
        shotId=id;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.name.Contains("Player")){
            Destroy(gameObject);
        }
    }
    void Update()
    {
        shooterTimer-=Time.deltaTime;
        if(shooterTimer<=0)Shot();
    }
    void Shot(){
        transform.rotation=Quaternion.Euler(0,0,Random.Range(min,max));
        timer=shooterTimer;
        GameObject go=new GameObject("enemybullet");
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulleSpeed=5;
        bu.spriteID=shotId;
        go.AddComponent<CircleCollider2D>();
        go.transform.up=-transform.up;
    }
}
