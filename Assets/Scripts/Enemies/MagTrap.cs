using UnityEngine;

public class MagTrap : EnemyBase
{
    Transform[] parts=new Transform[3];
    float timer;
    static Transform field;
    Transform magField;

    public override void SetSprites(EnemyInfo ei)
    {
        SetHP(40,ei.lifeproportion);
        points=40;
        for (int i = 0; i < 3; i++)
        {
            GameObject go=new GameObject("part");
            go.transform.rotation=Quaternion.Euler(0,0,120*i);
            go.transform.parent=transform;
            go.transform.Translate(0,0.3f,0.1f);
            parts[i]=go.transform;
            go.AddComponent<SpriteRenderer>().sprite=ei.sprites[1];
            
        }
        if(!field)field=ei.particles[0].transform;
        fallSpeed=-3;
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        transform.Rotate(0,0,fallSpeed*10*Time.deltaTime);
        if(timer>0){
            timer-=Time.deltaTime;
            magField.localScale=Vector3.MoveTowards(magField.localScale,Vector3.one,Time.deltaTime);
            fallSpeed=-1;
            foreach (Transform t in parts)
            {
                t.Translate(0,1*Time.deltaTime/2,0);
            }
        }
        if(timer==0){
            if(Vector3.Distance(GetPlayer(transform.position).position,transform.position)<6){
                timer=1;
                magField=Instantiate<Transform>(field,transform);
                magField.gameObject.SetActive(true);
                magField.name="enemybullet";
            }
        }
        SlowFall();
    }
}
