using UnityEngine;

public class Priest : EnemyBase
{
    LineRenderer line;
    Vector3[] positions=new Vector3[20];
    float time,timer,posX;
    new BoxCollider2D collider;
    Del upate;
    public override void SetSprites(EnemyInfo ei)
	{
        hp=80;
        points=60;
        GameObject g=new GameObject("enemylaser");
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
        upate+=SlowFall;
        upate+=Divine;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        upate?.Invoke();
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
    void Show(){
        line.enabled=true;
        line.widthMultiplier=0;
        posX=GetPlayer().position.x;
        float f=Time.time;
        timer=2;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].x=posX;
        }
        line.SetPositions(positions);
    }
}
