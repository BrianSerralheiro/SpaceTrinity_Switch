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
	GameObject charMenu;
	[SerializeField]
	Graphic selector;
	int selectionID;
	int movement;
	void Start()
    {
        if(rowCount<2)rowCount=2;
		Locks.Load();
    }
	private void OnEnable()
	{
		movement=-1;
	}
	void Update()
    {
		if(movement==-1)
		{
			transform.Translate(-Screen.width*Time.deltaTime,0,0);
			if(transform.position.x<Screen.width/2){
				movement=0;
				transform.position=new Vector3(Screen.width/2,Screen.height/2);
			}
			return;
		}
		if(movement==1)
		{
			transform.Translate(Screen.width*Time.deltaTime,0,0);
			if(transform.position.x>Screen.width*1.5f){
				gameObject.SetActive(false);
				transform.position=new Vector3(Screen.width*1.5f,Screen.height/2);
			}
			return;
		}
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
		if(Input.GetKeyDown(KeyCode.Space)){
			EnemySpawner.world=worlds[selectionID];
			if(EnemySpawner.world)SceneManager.LoadSceneAsync("cen");
			enabled=false;
		}
		if(Input.GetKeyDown(KeyCode.Q))
		{
			movement=1;
			charMenu.SetActive(true);
		}
	}
}
