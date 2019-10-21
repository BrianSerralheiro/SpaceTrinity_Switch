using UnityEngine;

public class SkinSwitch : MonoBehaviour {
	[SerializeField]
	private GameObject[] skins=new GameObject[4];
	[SerializeField]
	private int charID;
	private int skinID;
	public static int selectedChar,selectedSkin;
	void Update()
	{
		if(selectedChar==charID){
			if(skinID!=selectedSkin)Set();
		}
	}/*
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
	}*/
	void Set() {
		skinID=selectedSkin;
		for(int i = 0; i<skins.Length; i++)
		{
			skins[i].SetActive(i==skinID);
		}
		PlayerPrefs.SetInt("char"+charID,skinID+1);
	}
}
