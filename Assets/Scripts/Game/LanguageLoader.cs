using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
namespace LanguagePack{
public static class Language
{
    public static Dictionary<string,string> menu=new Dictionary<string, string>(),dialog=new Dictionary<string, string>();
    public static int id=1;
    static string menupath="Assets/LanguagePack/menu",dialogpath="Assets/LanguagePack/";
    public static void LoadMenu(){
        StreamReader reader=new StreamReader(menupath+id+".json");
        menu.Clear();
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
        //MUDAR DE LUGAR O LOAD
        if(menu.Count==0)LoadMenu();
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
}
}
