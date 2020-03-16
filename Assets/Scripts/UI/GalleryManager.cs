using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    [SerializeField]
    RectTransform[] objects,options;
    RectTransform rect;
    public static int id=-1;
    void Start()
    {
        rect=GetComponent<RectTransform>();
    }

    void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(i==id){
                objects[i].anchorMin=Vector2.MoveTowards(objects[i].anchorMin,rect.anchorMin,Time.deltaTime*2);
                objects[i].anchorMax=Vector2.MoveTowards(objects[i].anchorMax,rect.anchorMax,Time.deltaTime*2);
            }else {
                objects[i].anchorMin=Vector2.MoveTowards(objects[i].anchorMin,options[i].anchorMin,Time.deltaTime*5);
                objects[i].anchorMax=Vector2.MoveTowards(objects[i].anchorMax,options[i].anchorMax,Time.deltaTime*5);
            }
        }
    }
}
