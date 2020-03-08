using UnityEngine;
using System.Collections.Generic;

public class Train : MonoBehaviour
{
    public BulletPath path;
    [SerializeField]
    Queue<Vector3> pos=new Queue<Vector3>();
    public Vector3[] vec;
    float queueTime;
    public Transform[] cars;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i]=Instantiate(transform.GetChild(0));
            cars[i].Translate(0,-i-1,0);
            pos.Enqueue(Vector3.up*-i);

        }
        vec=pos.ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,Time.deltaTime*5,0);
        Vector3 vector=Vector3.Cross(transform.up,path.GetNode()-transform.position);
        transform.Rotate(vector*90*Time.deltaTime);
        if(Vector3.Distance(transform.position,path.GetNode())<0.5f){
            path.Next();
            if(path.Finished())path.Restart();
            // queueTime=Time.time+1;
            // pos.Enqueue(transform.position);
            // pos.Dequeue();
        }
        if(queueTime<Time.time){
            queueTime=Time.time+0.2f;
            pos.Enqueue(transform.position);
            pos.Dequeue();
            vec=pos.ToArray();
        }
        // vector=Vector3.Cross(-cars[0].up,transform.position-cars[0].position);
        // cars[0].Rotate(vector*60*Time.deltaTime);
        for (int i = 0; i<cars.Length; i++)
        {
            cars[i].position=Vector3.MoveTowards(cars[i].position,vec[cars.Length-1-i],5*Time.deltaTime);
            if(i==0)vector=Vector3.Cross(cars[i].up,transform.position-cars[i].position);
            else vector=Vector3.Cross(cars[i].up,cars[i-1].position-cars[i].position);
            cars[i].Rotate(vector*720*Time.deltaTime);
        }
        // Vector3 vector=Vector3.Cross(transform.up,path.Direction(false));
        // transform.Rotate(vector*180*Time.deltaTime);
        // // transform.up=-path.Direction(false);
        // transform.position=BulletPath.Next(ref path,false);
        // // transform.Translate(0,-Time.deltaTime*path.speed,0);
        // if(path.Finished())path.Restart();
        //transform.Translate(0,Time.deltaTime*5,0);
        /*
            if(transform.position.y>Scaler.sizeY/2 && transform.position.x<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),180*Time.deltaTime);
            if(transform.position.x>Scaler.sizeX/2-2.5f && transform.position.y>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,180),180*Time.deltaTime);
            if(transform.position.y<-Scaler.sizeY/2 && transform.position.x>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,90),180*Time.deltaTime);
            if(transform.position.x<-Scaler.sizeX/2+2.5f && transform.position.y<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,0),180*Time.deltaTime);
        */
        // if(transform.position.x>Scaler.sizeX/2-2 && transform.position.x>=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,91),360*Time.deltaTime);
        // if(transform.position.x<-Scaler.sizeX/2+2 && transform.position.x<0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),360*Time.deltaTime);

    }
}
