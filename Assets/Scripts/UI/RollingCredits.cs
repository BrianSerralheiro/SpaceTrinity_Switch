using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollingCredits : MonoBehaviour 
{

	[SerializeField]
	private RectTransform rollingCredits;

	// Use this for initialization
	void Start () 
	{
		rollingCredits = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!EnemySpawner.boss)
		{
			rollingCredits.Translate(0, Time.deltaTime * Screen.height / 7, 0);			
		}
		if(rollingCredits.position.y > Screen.height + 8000)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}
