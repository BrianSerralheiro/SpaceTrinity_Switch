using UnityEngine;
[System.Serializable]
public struct BulletPath
{
    public int nodeID;
    public float speed;
    private Vector3 point;
    public Vector3[] nodes;
    public float[] curves;
    private static float rad=90f*Mathf.Deg2Rad;
    public static Vector3 Next(ref BulletPath path,bool b){
        float c=path.GetCurve();
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
        }else if(c!=0)v+=Calculate(path.point,path.PrevNode(),path.GetNode(),c);
        if(b)v.x*=-1;
        return v;
    }
    private static Vector3 Recalculate(ref BulletPath path,bool b,float s){
        float c=path.GetCurve();
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
        }else if(c!=0)v+=Calculate(path.point,path.PrevNode(),path.GetNode(),c);
        if(b)v.x*=-1;
        return v;
    }
    public Vector3 Directiom(bool b){
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
    public Vector3 GetNode0(){
        return nodes[0];
    }
    private Vector3 GetNode(){
        return nodes[nodeID];
    }
    private float GetCurve(){
        return curves[nodeID];
    }
    private Vector2 PrevNode(){
        if(nodeID==0)return Vector3.zero;
        return nodes[nodeID-1];
    }
    public bool Finished(){
        return nodeID>=nodes.Length;
    }
    public static Vector3 Calculate(Vector3 pos,Vector3 prev,Vector3 node,float c){
        float h=Vector3.Distance(prev,node);
        float d=Vector3.Distance(pos,prev);
        float p=d/h;
        p=Mathf.Sin(p*rad);
        Vector3 v=node-prev;
        v=new Vector3(-v.y,v.x,v.z)*p*c;
        return v;
    }
    public static Vector3 Calculate(Vector3 pos,Vector3 node,float speed){
        return Vector3.MoveTowards(pos,node,speed);
    }
}
