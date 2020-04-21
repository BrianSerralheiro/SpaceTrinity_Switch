using UnityEngine;
[System.Serializable]
public struct BulletPath
{
    public int nodeID;
    public float speed;
    private Vector3 point;
    public Vector3[] nodes;
    private static float rad=90f*Mathf.Deg2Rad;
    public BulletPath Set(float s,Vector3[] n){
        speed=s;
        nodes=new Vector3[n.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i]=n[i];
        }
        return this;
    }
    public void Next(){
        nodeID++;
    }
    public static BulletPath Upscale(BulletPath path){
        path=new BulletPath().Set(path.speed,path.nodes);
        for (int i = 0; i < path.nodes.Length; i++)
        {
            path.nodes[i].x*=Scaler.sizeX/2;
            path.nodes[i].y*=Scaler.sizeY;
        }
        return path;
    }
    public static Vector3 Next(ref BulletPath path,bool b){
        Vector3 p=path.point;
        path.point=Calculate(path.point,path.GetNode(),path.speed*Time.deltaTime);
        Vector3 v=path.point;
        if(path.point==path.GetNode()){
            path.nodeID++;
            if(!path.Finished()){
               float f=Vector3.Distance(p,path.point);
               f=path.speed*Time.deltaTime-f;
               return Recalculate(ref path,b,f);
            }
        }
        if(b)v.x*=-1;
        return v;
    }
    private static Vector3 Recalculate(ref BulletPath path,bool b,float s){
        Vector3 p=path.point;
        path.point=Calculate(path.point,path.GetNode(),s);
        Vector3 v=path.point;
        if(path.point==path.GetNode()){
            path.nodeID++;
            if(!path.Finished()){
               float f=Vector3.Distance(p,path.point);
               f=s-f;
               return Recalculate(ref path,b,f);
            }
        }
        if(b)v.x*=-1;
        return v;
    }
    public Vector3 Direction(bool b){
        Vector3 v;
        if(nodeID==0)v=nodes[0];
        else v=nodes[nodeID]-nodes[nodeID-1];
        if(b)v.x*=-1;
        return v.normalized;
    }
    public Vector3 GetNodeL(bool b){
        Vector3 v;
        if(nodes.Length<2)v= nodes[0];
        else v= nodes[nodes.Length-1]-nodes[nodes.Length-2];
        if(b)v.x*=-1;
        return v;
    }
    public Vector3 GetNode0(bool b){
        Vector3 v=nodes[0];
        if(b)v.x*=-1;
        return v;
    }
    public Vector3 GetNode(){
        return nodes[nodeID];
    }
    private Vector2 PrevNode(){
        if(nodeID==0)return Vector3.zero;
        return nodes[nodeID-1];
    }
    public bool Finished(){
        return nodeID>=nodes.Length;
    }
    public void Restart(){
        nodeID=0;
        point=Vector3.zero;
    }
    public static Vector3 Calculate(Vector3 pos,Vector3 node,float speed){
        return Vector3.MoveTowards(pos,node,speed);
    }
}
