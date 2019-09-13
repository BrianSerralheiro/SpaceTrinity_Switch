using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLock : MonoBehaviour
{
	[SerializeField]
	private bool isChar;
	[SerializeField]
	private int charId;
	[SerializeField]
	private int skinId;
	void OnEnable () {
		Button b=GetComponent<Button>();
		if(b)
		{
			if(isChar) b.interactable=!Locks.Char(charId);
			else b.interactable=charId>=0? Locks.Char(charId) && !Locks.Skin(skinId):!Locks.Skin(skinId);
		}
		else Debug.LogError("ButtonLock needs an image or a button to work");
	}
}
