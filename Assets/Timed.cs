using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed : MonoBehaviour
{
    [SerializeField]
    float timer;
    float time;
    void OnEnable()
    {
        time=Time.time+timer;
        GetComponent<Collider2D>().enabled=true;
    }
    void Update()
    {
        if(time<Time.time){
            gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled=false;
        }

    }
}
