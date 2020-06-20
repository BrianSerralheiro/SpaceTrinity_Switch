using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BGProp
{
    public GameObject prefab;
    public float position,variant,distance;
    public bool manual;
    public Vector3 Position(float f){
        return new Vector3(position+Random.Range(-variant,variant),Scaler.sizeY+5+f,1);
    }
}
