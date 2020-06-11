using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BGProp
{
    public GameObject prefab;
    public float position,variant,distance;
    public bool manual;
    public Vector3 Position(){
        return new Vector3(position+Random.Range(-variant,variant),Scaler.sizeY+2,1);
    }
}
