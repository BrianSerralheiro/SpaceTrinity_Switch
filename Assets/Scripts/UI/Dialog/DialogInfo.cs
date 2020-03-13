using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="Dialog",menuName="DialogInfo")]
public class DialogInfo:ScriptableObject{
    public Dialog[] dialogs;
    public int id;
    public Dialog Next(){
        for (int i = id; i < dialogs.Length; i++)
        {
            if(dialogs[i].Show()){
                id=i+1;
                return dialogs[i].GetDialog();
            }
        }
        return new  Dialog();
    }
    public Dialog GetDialog(int i){
        if(dialogs[i].Show())return dialogs[i].GetDialog();
        return new  Dialog();
    }
}
[System.Serializable]
public struct Dialog{
    public DialogCondition[] conditions;
    public Speech[] entries;
    public Vector2 vector;
    int id;
    bool CanShow(){
        foreach (DialogCondition condition in conditions)
        {
            if(!condition.Met())return false;
        }
        return true;
    }
    public Speech GetSpeech(){
        if(id<entries.Length)return  entries[id++];
        return new Speech();
    }
    public bool HasSpeech(){
        return entries!=null && id<entries.Length;
    }
    public bool Show(){
        return  CanShow();
    }
    public bool IsEmpty(){
        return entries==null || entries.Length==0;
    }
    public Dialog GetDialog(){
        return this;
    }

}
[System.Serializable]
public struct Speech 
{
    [TextArea(3,3)]
    public string text;
    public Character[] characters;
}
[System.Serializable]
public struct Character{
    public Sprite picture;
    public bool lit;
    [Range(-1.2f,1.2f)]
    public float proportion;
    [Range(0,1)]
    public float position;
}
[System.Serializable]
public struct DialogCondition{
    public enum Type{
        none,ship,boss
    }
    public Type conditionType;
    public int id;
    public bool Met(){
        switch (conditionType)
        {
            case Type.ship:
                return Ship.player1==id || Ship.player2==id;
            case Type.boss:
                return false;//BOSS DERROTADO
            default:
                return true;
        }
    }
}

