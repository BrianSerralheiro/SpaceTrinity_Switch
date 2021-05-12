using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed : MonoBehaviour
{
    [SerializeField]
    Vector3 point;

    // Update is called once per frame
    void Update()
    {
        transform.position=point;
    }
}
