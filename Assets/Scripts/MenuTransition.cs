﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MenuTransition
{
    public KeyCode key;
    public MenuSelect menu;
    public Vector3 flow;
    public bool GetKeyDown(){
        return Input.GetKeyDown(key);
    }
    public void Open(){
        menu?.Open(new Vector3((flow.x+0.5f)*Screen.width,(flow.y+0.5f)*Screen.height));
    }
    public void Close(MenuSelect m){
        m.Close(new Vector3((-flow.x+0.5f)*Screen.width,(-flow.y+0.5f)*Screen.height));
    }
}
