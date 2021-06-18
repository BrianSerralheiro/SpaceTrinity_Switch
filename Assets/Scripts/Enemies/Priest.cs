using UnityEngine;

public class Priest : EnemyBase
{
    LineRenderer line;
    Vector3[] positions=new Vector3[20];
	new private Core light;
    float time,timer,posX;
    Transform[] wings=new Transform[4];
    new BoxCollider2D collider;
    public static Material material;
    Del upate;
    public override void SetSprites(EnemyInfo ei)
	{
        SetHP(120,ei.lifeproportion);
        points=90;
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
        if(!material)material=new Material(Shader.Find("Sprites/Default"));
        g=new GameObject("enemylaser");
        g.transform.parent=transform;
        collider=g.AddComponent<BoxCollider2D>();
        collider.enabled=false;
        line=g.AddComponent<LineRenderer>();
        line.positionCount=positions.Length;
        line.enabled=false;
        line.material=material;
        Gradient gradient=new Gradient();
		gradient.SetKeys(new GradientColorKey[]{new GradientColorKey(new Color(1,1,0.8f),0)},new GradientAlphaKey[]{new GradientAlphaKey(1,0.5f),new GradientAlphaKey(0,1)});
		line.colorGradient=gradient;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].y=i*Scaler.sizeY/9-Scaler.sizeY-1;
            positions[i].z=-0.2f;
        }
        g=new GameObject("light");
		Texture2D t=new Texture2D(1,1);
		t.SetPixels(new Color[]{Color.white});
		t.Apply(false);
		light=g.AddComponent<Core>().Set(Sprite.Create(t,new Rect(0,0,1,1),new Vector2(0.5f,0.5f)),new Color(1f,1f,1f,0f));
        light.white = new Color(1,1,1,0.3f);
		// light.white=new Color(0f,0f,0f,1f);
		g.transform.localScale=new Vector3(5000,5000);
		g.transform.position=new Vector3(0,0,-0.1f);
        upate=SlowFall;
        upate+=Divine;
        upate+=LightOn;
        time=Time.time+5;
        timer=2;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        upate?.Invoke();
        if(timer<2){
            for (int i = 0; i < 4; i++)
                wings[i].rotation=Quaternion.RotateTowards(wings[i].rotation,Quaternion.Euler(0,0,-45*(i%2==0?-1:1)),90*Time.deltaTime);
        }
        else{
            for (int i = 0; i < 4; i++)
                wings[i].rotation=Quaternion.Euler(0,0,Mathf.PingPong(Time.time*(30+i/2*90),45)*(i%2==0?-1:1));
        }
    }
    void Divine()
    {
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
                    timer=2;
                    upate-=Divine;
                    upate+=LightOff;
                }
            }
        }
    }
    void LightOn(){
        if(timer<1.8)light.Add(Time.deltaTime*5);
        if(light.Value()>=1){
            // timer=Time.time+3;
            upate-=LightOn;
        }
    }
    void LightOff(){
        light.Min(Time.deltaTime);
        if(light.Value()<=0)upate-=LightOff;
    }
    new void OnCollisionEnter2D(Collision2D col)
	{
		if(col.otherCollider.name=="enemylaser") return;
		base.OnCollisionEnter2D(col);
	}
    protected override void Die()
	{
        light.Auto(0.5f);
        base.Die();
    }
    void Show(){
        line.enabled=true;
        line.widthMultiplier=0;
        posX=GetPlayer().position.x;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].x=posX;
        }
        line.SetPositions(positions);
    }
}
