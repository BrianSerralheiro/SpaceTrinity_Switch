using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSwitch : MonoBehaviour {
	[SerializeField]
	private GameObject[] skins=new GameObject[4];
	private int id=-1;
	[SerializeField]
	private int charID;
	public void Next()
	{
		id++;
		if(id>2)id=-1;
		SoundManager.PlayEffects(0);
		Set();
	}
	public void Prev()
	{
		id--;
		if(id<-1) id=2;
		SoundManager.PlayEffects(0);
		Set();
	}
	void OnEnable()
	{
		id=PlayerPrefs.GetInt("char"+charID)-1;
		Set();
	}
	void Set() {
		Ship.skinID=id;
		for(int i = 0; i<skins.Length; i++)
		{
			skins[i].SetActive(i==id+1);
		}
		PlayerPrefs.SetInt("char"+charID,id+1);
	}
}
