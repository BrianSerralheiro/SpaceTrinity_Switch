using UnityEngine;

public class Train : MonoBehaviour
{
    public BulletPath path;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 vector=Vector3.Cross(-transform.up,path.Direction(false));
        // //transform.Rotate(vector*90*Time.deltaTime);
        // transform.up=-path.Direction(false);
        // BulletPath.Next(ref path,false);
        // transform.Translate(0,-Time.deltaTime*path.speed,0);
        // if(path.Finished())path.Restart();
        transform.Translate(0,Time.deltaTime*5,0);
        /*
            if(transform.position.y>Scaler.sizeY/2 && transform.position.x<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),180*Time.deltaTime);
            if(transform.position.x>Scaler.sizeX/2-2.5f && transform.position.y>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,180),180*Time.deltaTime);
            if(transform.position.y<-Scaler.sizeY/2 && transform.position.x>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,90),180*Time.deltaTime);
            if(transform.position.x<-Scaler.sizeX/2+2.5f && transform.position.y<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,0),180*Time.deltaTime);
        */
        if(transform.position.x>Scaler.sizeX/2-2 && transform.position.x>=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,91),360*Time.deltaTime);
        if(transform.position.x<-Scaler.sizeX/2+2 && transform.position.x<0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),360*Time.deltaTime);

    }
}
