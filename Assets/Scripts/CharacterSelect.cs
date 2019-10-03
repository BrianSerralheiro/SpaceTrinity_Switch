using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
	[SerializeField]
	Graphic[] chars;
	[SerializeField]
	GameObject worldMenu;
	[SerializeField]
	Graphic selector;
	int selectionID;
	int skinID;
	int movement;
	void Start()
	{

	}
	private void OnEnable()
	{
		movement=1;
		transform.position=new Vector3(-Screen.width/2,Screen.height/2);
		selectionID=Ship.playerID;
		if(chars[selectionID])
		{
			selector.rectTransform.position=chars[selectionID].rectTransform.position;
			selector.rectTransform.anchoredPosition=chars[selectionID].rectTransform.anchoredPosition;
			selector.rectTransform.anchorMin=chars[selectionID].rectTransform.anchorMin;
			selector.rectTransform.anchorMax=chars[selectionID].rectTransform.anchorMax;
		}
	}
	void Update()
	{
		if(movement==-1)
		{
			transform.Translate(-Screen.width*Time.deltaTime,0,0);
			if(transform.position.x<-Screen.width/2)
			{
				gameObject.SetActive(false);
			}
			return;
		}
		if(movement==1)
		{
			transform.Translate(Screen.width*Time.deltaTime,0,0);
			if(transform.position.x>Screen.width/2){
				movement=0;
				transform.position=new Vector3(Screen.width/2,Screen.height/2);
			}
			return;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))skinID++;
		if(Input.GetKeyDown(KeyCode.DownArrow))skinID--;
		if(Input.GetKeyDown(KeyCode.RightArrow))selectionID++;
		if(Input.GetKeyDown(KeyCode.LeftArrow))selectionID--;
		if(selectionID>=chars.Length) selectionID=0;
		if(skinID>2) skinID=-1;
		if(selectionID<0) selectionID=chars.Length-1;
		if(skinID<-1) skinID=2;

		if(chars[selectionID])
		{
			selector.rectTransform.position=chars[selectionID].rectTransform.position;
			selector.rectTransform.anchoredPosition=chars[selectionID].rectTransform.anchoredPosition;
			selector.rectTransform.anchorMin=chars[selectionID].rectTransform.anchorMin;
			selector.rectTransform.anchorMax=chars[selectionID].rectTransform.anchorMax;
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			movement=-1;
			Ship.playerID=selectionID;
			Ship.skinID=skinID;
			worldMenu.SetActive(true);
		}
	}
}
