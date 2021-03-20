using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace LanguagePack{
public static class Language
{
    public static Dictionary<string,string> menu=new Dictionary<string, string>(),dialog=new Dictionary<string, string>();
    public static int id{
        set{
            internalid=value;
            if(internalid>1)internalid=0;
            if(internalid<0)internalid=1;
            LoadMenu();
            UITranslatorRT.Update();
            }
        get{return internalid;}
    }
    static  int internalid=1;
    static string menupath="Assets/LanguagePack/menu",dialogpath="Assets/LanguagePack/";
    public static void LoadMenu(){
        menu.Clear();
        StreamReader reader;
        try
        {
            reader=new StreamReader(menupath+id+".json");
        }
        catch (System.Exception)
        {
            return;
        }
        string s=reader.ReadLine();
        while(s!=null)
        {
            int i=s.IndexOf("=");
            if(i>=0)menu.Add(s.Substring(0,i),s.Substring(i+1,s.Length-i-1));
            s=reader.ReadLine();
        }
        reader.Close();
    }
    public static string GetMenu(string s){
        string r;
        if(menu.TryGetValue(s,out r))s=r;
        return s;
    }
    public static void LoadDialog(string name){
        dialog.Clear();
        StreamReader reader=new StreamReader(dialogpath+name+id+".json");
        string s=reader.ReadLine();
        while(s!=null)
        {
            int i=s.IndexOf("=");
            // Debug.Log(s);
            dialog.Add(s.Substring(0,i),s.Substring(i+1,s.Length-i-1));
            s=reader.ReadLine();
        }
        reader.Close();
    }
    public static string GetDialog(string key){
        string s;
        if(dialog.TryGetValue(key,out s))return s;
        Debug.LogError("No dialog found to key "+key);
        return key;
    }
    public static void Save(){
        PlayerPrefs.SetInt("language",internalid);
    }
    public static void Load(){
        internalid=PlayerPrefs.GetInt("language");
    }
}
}
