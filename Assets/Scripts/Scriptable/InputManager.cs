﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputManager : ScriptableObject
{
    public PlayerInput[] players;
    public void Load()
    {
        PlayerInput.players=players;
    }
}
[System.Serializable]
public struct PlayerInput{
    public static bool recentConect,recentDisconect;
    public int id;
    public KeyCode shoot,special,equip;
    public string vertical,horizontal,name;
    public static PlayerInput[] players;
    public bool got,conected;
    public Vector3 GetAxis(){
        return new Vector3(Input.GetAxis(horizontal),Input.GetAxis(vertical),0);
    }
    public PlayerInput Select(){
        got=true;
        return this;
    }
    public bool GetKeyDown(string s){
        switch(s){
            case "shoot":
                return Input.GetKeyDown(shoot);
            case "special":
                return Input.GetKeyDown(special);
            case "equip":
                return Input.GetKeyDown(equip);
            default:
                return false;
        }
    }
    public static KeyCode GetKeyEquip(int i){
        if(i<players.Length)return players[i].equip;
        return KeyCode.None;
    }
    public static KeyCode GetKeySpecial(int i){
        if(i<players.Length)return players[i].special;
        return KeyCode.None;
    }
    public static KeyCode GetKeyShot(int i){
        if(i<players.Length)return players[i].shoot;
        return KeyCode.None;
    }
    public static void WaitInput(){
        recentConect=false;
        recentDisconect=false;
        for(int i=0;i<players.Length;i++){
            if(players[i].conected){
                if(Input.GetKeyDown(players[i].equip))DisConnected(i);
            }
            else{
                if(Input.GetKeyDown(players[i].special))Connect(i);
            }
        }
    }
    public static void DisConnected(int i){
        players[i].conected=false;
        recentDisconect=true;
    }
    public static void Connect(int i){
        players[i].conected=true;
        recentConect=true;
    }
    public static bool Conected(int i){
        return players[i].conected;
    }
    public static void Unload(){
        players[0].got=false;
    }
    public static PlayerInput Get(){
        if(players[0].got)return players[1].Select();
        return players[0].Select();
    }
}
