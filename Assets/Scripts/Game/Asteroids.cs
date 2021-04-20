using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public delegate void Del();
    public Del update;
    [SerializeField]
    float delayTime,speed,dieOffset;
    [SerializeField]
    GameObject controlledObject;
    float time;
    void Start()
    {
        time=Time.time+delayTime;
        controlledObject.SetActive(false);
        update=()=>{
            if(time<Time.time){
                controlledObject.SetActive(true);
                update=()=>{
                    controlledObject.transform.Translate(0,speed*Time.deltaTime,0);
                    if(controlledObject.transform.position.y<-Scaler.sizeY-dieOffset)Destroy(gameObject);
                };
            }
        };
    }
    void OnDestroy()
    {
        Destroy(controlledObject);
    }

    void Update()
    {
        update();
    }
}
