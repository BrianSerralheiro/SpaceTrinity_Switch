using UnityEngine;

public class SkinSwitch : MonoBehaviour {
	[SerializeField]
	private GameObject[] skins=new GameObject[4];
	[SerializeField]
	private int charID;
	private int skinID;
	public static int[] selectedChar={0,5};
	public static int[] selectedSkin;
	void Update()
	{
		if(selectedChar[0]==charID || selectedChar[1]==charID){
			if(skinID!=selectedSkin[charID])Set();
		}
	}
	void Set() {
		skinID=selectedSkin[charID];
		for(int i = 0; i<skins.Length; i++)
		{
			skins[i].SetActive(i==skinID);
		}
		PlayerPrefs.SetInt("char"+charID,skinID+1);
	}
}
