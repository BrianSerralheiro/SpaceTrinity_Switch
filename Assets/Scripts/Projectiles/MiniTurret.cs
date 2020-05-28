using UnityEngine;

public class MiniTurret : MonoBehaviour
{
    float angle,angledelta,timer,shooterTimer;
    int shots,hp;
    public int shotId,trailID,impactID;
    public float variant=1;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.name.Contains("Player") && hp--<=0){
            enabled=false;
        }
    }
    void OnEnable()
    {
        hp=5;
    }
    void OnDisable()
    {
        shots=0;
    }
    void Update()
    {
        if(!Ship.paused && shots>0 && timer<Time.time)Shot();
    }
    public void Prepare(float initialAngle,float anglepershot,float shotcount,float delay){
        angle=initialAngle;
        angledelta=anglepershot;
        shots=(int)shotcount;
        shooterTimer=delay*variant;
        timer=0;
    }
    void Shot(){
        timer=Time.time+shooterTimer;
        transform.rotation=Quaternion.Euler(0,0,angle);
        angle+=angledelta;
        shots--;
        GameObject go=new GameObject("enemybullet");
        go.AddComponent<SpriteRenderer>().sprite=Bullet.sprites[shotId];
        Bullet bu=go.AddComponent<Bullet>();
        bu.owner="enemy";
        bu.bulletSpeed=5;
        bu.Timer(5);
        bu.spriteID=shotId;
        bu.particleID=trailID;
        bu.impactID=impactID;
        go.AddComponent<BoxCollider2D>();
        go.transform.position=transform.position-transform.up;
        go.transform.up=-transform.up;
    }
}
