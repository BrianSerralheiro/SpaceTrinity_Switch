using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlanter : MonoBehaviour
{
    [SerializeField]
    GameObject[] trees;
    [SerializeField]
    Vector2Int cellCount;
    [SerializeField]
    Vector2 gridArea;
    [SerializeField][Range(0.1f,1)]
    float maxOffset,minScale=1,maxScale=1;
    [SerializeField][Range(0,360)]
    float minAngle,maxAngle;
    Vector2 offset;
    void Start()
    {
        offset=new Vector2(gridArea.x/cellCount.x,gridArea.y/cellCount.y)*maxOffset;
        Vector2 size=new Vector2(gridArea.x/cellCount.x,gridArea.y/cellCount.y);
        Vector3 center=new Vector3(gridArea.x/2f,0,gridArea.y/2f);
        for (int i = 0; i < cellCount.x; i++){
            for (int j = 0; j < cellCount.y; j++){
                Transform t=Instantiate(trees[Random.Range(0,trees.Length)],transform).transform;
                t.localPosition=new Vector3(size.x*i+offset.x*Random.value,0,size.y*j+offset.y*Random.value)-center;
                t.Rotate(0,Random.Range(minAngle,maxAngle),0,Space.Self);
                t.localScale=Vector3.one*Random.Range(minScale,maxScale);
            }
        }
        Destroy(this);
    }
}
