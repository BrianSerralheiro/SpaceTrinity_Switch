using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    List<BGProp> props=new List<BGProp>();
    List<Transform> objects=new List<Transform>();
    float distance,speed;
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
            objects.Add(t);
            t.position=props[i].Position(distance);
            distance+=props[i].distance;
        }
        distance-=Time.deltaTime*speed;
        foreach (Transform t in objects)
        {
            t.Translate(0,-speed*Time.deltaTime,0,Space.World);
        }
        foreach (Transform t in objects)
        {
            if(t.position.y<-Scaler.sizeY-15){
                objects.Remove(t);
                Destroy(t.gameObject);
                break;
            }
        }
    }
}
