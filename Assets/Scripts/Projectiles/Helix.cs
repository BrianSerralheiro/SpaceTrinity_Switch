using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour
{
    new SpriteRenderer renderer;
    Vector3 dir;
    public float time;
    void Start()
    {
        renderer=GetComponent<SpriteRenderer>();
        dir=Random.insideUnitCircle;
    }
    void Update()
    {
        if(transform.parent){
            transform.Rotate(0,0,Time.deltaTime*360);
            renderer.enabled=Time.time%0.1f<0.05f;
        }else{
            transform.Translate(dir*Time.deltaTime*10,Space.World);
            transform.Rotate(0,0,Time.deltaTime*360);
            renderer.enabled=true;
            if(time<Time.time)Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        ParticleManager.Emit(0,transform.position,1);
    }
}
