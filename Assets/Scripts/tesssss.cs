using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LanguagePack;

public class tesssss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Language.LoadMenu();
        Debug.Log(Language.GetMenu("Play"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
