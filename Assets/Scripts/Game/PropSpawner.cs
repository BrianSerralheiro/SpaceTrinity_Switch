using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    List<BGProp> props=new List<BGProp>();
    List<Transform> objects=new List<Transform>();
    float distance=-24,speed,maxDistance;
    Queue<float> distances=new Queue<float>();
    int prev=-1;
    void Start()
    {
        foreach (BGProp prop in EnemySpawner.world.props)
        {
            if(!prop.manual)props.Add(prop);
        }
        speed=EnemySpawner.world.scroll;
        if(props.Count==0)enabled=false;
    }
    void Update()
    {
        if(distance<=0){
            int i=0;
            do{
                i=Random.Range(0,props.Count);
            }while(i==prev);
            prev=i;
            Transform t=Instantiate(props[i].prefab).transform;
            if(objects.Count==0)maxDistance=props[i].distance;
            else distances.Enqueue(props[i].distance);
            objects.Add(t);
            t.position=props[i].Position(distance);
            distance+=props[i].distance;
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
