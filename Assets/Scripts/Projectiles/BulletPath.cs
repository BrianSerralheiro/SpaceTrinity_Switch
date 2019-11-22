using UnityEngine;
[System.Serializable]
public struct BulletPath
{
    public int nodeID;
    public float speed;
    private Vector3 point;
    public Vector3[] nodes;
    public static Vector3 Next(ref BulletPath path,bool b){
        Vector3 vector=Calculate(path.point,path.GetNode(),path.speed*Time.deltaTime);
        if(vector==path.GetNode())path.nodeID++;
        Vector3 v=vector-path.point;
        path.point=vector;
        if(b)v.x*=-1;
        return v;
    }
    private Vector3 GetNode(){
        return nodes[nodeID];
    }
    public bool Finished(){
        return nodeID>=nodes.Length;
    }
    public static Vector3 Calculate(Vector3 pos,Vector3 node,float speed){
        return Vector3.MoveTowards(pos,node,speed);
    }
}
