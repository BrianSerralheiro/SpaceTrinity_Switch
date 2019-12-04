using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLoader : MonoBehaviour
{
    [SerializeField]
    private InputManager manager;
    void Start()
    {
        manager.Load();
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
