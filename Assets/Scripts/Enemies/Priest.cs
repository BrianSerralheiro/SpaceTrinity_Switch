using UnityEngine;

public class Priest : EnemyBase
{
    LineRenderer line;
    Vector3[] positions=new Vector3[20];
    float time,timer,posX;
    Transform[] wings=new Transform[4];
    new BoxCollider2D collider;
    Del upate;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=80;
        points=60;
        GameObject g;
        for (int i = 0; i < 4; i++)
        {
            g=new GameObject("wing"+i);
            SpriteRenderer sp=g.AddComponent<SpriteRenderer>();
            sp.sprite=ei.sprites[1+i/2];
            sp.flipX=i%2==1;
            wings[i]=g.transform;
            wings[i].parent=transform;
            wings[i].localPosition=new Vector3((0.6f+i/2*0.15f)*(i%2==1?-1:1),0.4f-i/2*0.2f,0.1f);
        }
        g=new GameObject("enemylaser");
        g.transform.parent=transform;
        collider=g.AddComponent<BoxCollider2D>();
        collider.enabled=false;
        line=g.AddComponent<LineRenderer>();
        line.positionCount=positions.Length;
        line.enabled=false;
        timer=Time.time+5;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].y=i*Scaler.sizeY/9-Scaler.sizeY-1;
            positions[i].z=-0.2f;
        }
        timer=Time.time+3;
        upate=SlowFall;
        upate+=Divine;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        upate?.Invoke();
        if(timer>0){
            for (int i = 0; i < 4; i++)
                wings[i].rotation=Quaternion.Euler(0,0,30*(i%2==0?-1:1));
        }
        else{
            for (int i = 0; i < 4; i++)
                wings[i].rotation=Quaternion.Euler(0,0,Mathf.PingPong(Time.time*(30+i/2*90),45)*(i%2==0?-1:1));
        }
    }
    void Divine(){
        if(!line.enabled && time<Time.time){
            Show();
        }
        else{
            if(line.enabled){
                timer-=Time.deltaTime;
                if(timer<1)collider.enabled=!collider.enabled;        
                float f=4.5f/line.positionCount;
                float f1=40f;
                float t=Time.time*10;
                float t1=Time.time*40;
                line.widthMultiplier=2-timer;
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i].x=posX+(Mathf.Sin(t+f*i)*0.6f+Mathf.Cos(t1+f1*i)*0.4f)*(2-timer);
                }
                line.SetPositions(positions);
                collider.size=line.bounds.size;
                collider.offset=line.bounds.center-transform.position;
                if(timer<0){
                    line.enabled=false;
                    collider.enabled=false;
                    time=Time.time+3;
                    if(transform.position.y<0)upate-=Divine;
                }
            }
        }
    }
    new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="enemylaser") return;
		base.OnCollisionEnter2D(col);
	}
    void Show(){
        line.enabled=true;
        line.widthMultiplier=0;
        posX=GetPlayer().position.x;
        timer=2;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].x=posX;
        }
        line.SetPositions(positions);
    }
}
