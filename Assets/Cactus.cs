using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    Collider2D[] colliders=new Collider2D[0];
    void Start()
    {
        Vector2 v=transform.position;
        colliders=Physics2D.OverlapCircleAll(v,3);
        foreach (Collider2D col in colliders)
        {
            if(col.name.Contains("gr")){
                Destroy(gameObject);
                break;
            }
        }
    }
    void Update()
    {
        Vector2 v=transform.position;
        colliders=Physics2D.OverlapCircleAll(v,.2f);
        foreach (Collider2D col in colliders)
        {
            if(col.name.Contains("bigr")){
                transform.Rotate(80,0,0);
                Destroy(this);
                break;
            }
        }
    }
}
