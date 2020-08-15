using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    static bool reloaded;
    public enum Enum1
    {
        Rose,Luna,Bruna,Tomoyo,Chunhua,Zarina
    }
    public enum Enum2
    {
        none,Rose,Luna,Bruna,Tomoyo,Chunhua,Zarina
    }
    public Enum1 player1;
    public Enum2 player2;
    void Awake()
    {
        if(!reloaded)Loader.Scene("dialogtest");
        reloaded=true;
        Ship.player1=(int)player1;
        Ship.player2=(int)player2-1;
        enabled=false;
        Debug.Log(Ship.player1);
    }
}
