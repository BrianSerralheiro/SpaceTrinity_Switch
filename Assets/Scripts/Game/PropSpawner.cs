using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    List<BGProp> props=new List<BGProp>();
    List<Transform> objects=new List<Transform>();
    float distance=-24,speed,maxDistance;
    Queue<float> distances=new Queue<float>();
    static PropSpawner instance;
    int prev=-1;
    void Start()
    {
        instance=this;
        foreach (BGProp prop in EnemySpawner.world.props)
        {
            if(!prop.manual)props.Add(prop);
        }
        if(EnemySpawner.world){
            Instantiate(props[0].prefab);
            enabled=false;
        }
        speed=EnemySpawner.world.scroll;
        if(props.Count==0)enabled=false;
    }
    Transform Spawn(int i){
        prev=i;
        Transform t=Instantiate(props[i].prefab).transform;
        if(objects.Count==0)maxDistance=props[i].distance;
        else distances.Enqueue(props[i].distance);
        objects.Add(t);
        t.position=props[i].Position(distance);
        distance+=props[i].distance;
        return t;
    }
    public static Transform ManualSpawn(int i){
        if(!instance){
            Debug.LogError("Can't spawn prop \""+i+"\"");
            return null;
        }
        return instance.Spawn(i);
    }
    void Update()
    {
        if(distance<=0){
            int i=0;
            do{
                i=Random.Range(0,props.Count);
            }while(i==prev);
            Spawn(i);
        }
        distance-=Time.deltaTime*speed;
        foreach (Transform t in objects)
        {
            t.Translate(0,-speed*Time.deltaTime,0,Space.World);
        }
        if(objects.Count>0 && objects[0].position.y<-maxDistance-10){
            Destroy(objects[0].gameObject);
            objects.RemoveAt(0);
            maxDistance=distances.Dequeue();
        }
    }
}
