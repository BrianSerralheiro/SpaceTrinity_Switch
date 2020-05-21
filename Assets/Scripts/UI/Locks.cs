using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Locks {
	
	private static string skins="000000000000000000000";
	private static string bosses="0000000";

	public static void Load()
	{
		if(PlayerPrefs.HasKey("skins"))skins=PlayerPrefs.GetString("skins");
		if(PlayerPrefs.HasKey("bosses"))bosses=PlayerPrefs.GetString("bosses");
	}
	public static void Save()
	{
		PlayerPrefs.SetString("skins",skins);
		PlayerPrefs.SetString("bosses",bosses);
	}
	public static bool Skin(int i)
	{
		return skins[i]!='0';
	}
	public static void Skin(int i,bool b)
	{
		skins=skins.Substring(0,i)+"1"+skins.Substring(i+1,skins.Length-1-i);
		Save();
	}
	public static bool Boss(int i)
	{
		if(i<0)return false;
		return bosses[i]!='0';
	}
	public static int Boss(){
		int i=0;
		foreach (char c in bosses)
		{
			if(c=='1')i++;
		}
		return i;
	}
	public static void Boss(int i,bool b)
	{
		bosses=bosses.Substring(0,i)+"1"+bosses.Substring(i+1,bosses.Length-1-i);
		Save();
	}
	public static void UnlockAll()
	{
		for(int i = 0; i<skins.Length; i++)
		{
			if(i<bosses.Length)Boss(i,true);
			Skin(i,true);
		}
	}
}
