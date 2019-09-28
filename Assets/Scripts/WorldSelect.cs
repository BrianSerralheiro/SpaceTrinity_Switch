using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldSelect : MonoBehaviour
{
	[SerializeField]
	Graphic[] options;
	[SerializeField]
	WorldInfo[] worlds;
	[SerializeField]
	int rowCount=3;
	[SerializeField]
	Graphic selector;
	int selectionID;
	void Start()
    {
        if(rowCount<2)rowCount=2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))selectionID-=rowCount;
        if(Input.GetKeyDown(KeyCode.DownArrow))selectionID+=rowCount;
        if(Input.GetKeyDown(KeyCode.RightArrow))selectionID++;
        if(Input.GetKeyDown(KeyCode.LeftArrow))selectionID--;
		if(selectionID>=options.Length)selectionID-=options.Length;
		if(selectionID<0)selectionID+=options.Length;

		if(options[selectionID])
		{
			selector.rectTransform.position=options[selectionID].rectTransform.position;
			selector.rectTransform.anchoredPosition=options[selectionID].rectTransform.anchoredPosition;
			selector.rectTransform.anchorMin=options[selectionID].rectTransform.anchorMin;
			selector.rectTransform.anchorMax=options[selectionID].rectTransform.anchorMax;
		}
		if(Input.GetKeyDown(KeyCode.KeypadEnter)){
			EnemySpawner.world=worlds[selectionID];
			if(EnemySpawner.world)SceneManager.LoadSceneAsync("cen");
			enabled=false;
		}

	}
}
