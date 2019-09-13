using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
	public void OnPause(bool p)
	{
		Ship.paused = p;
	}

	public void Exit()
	{	
		Ship.paused = false;
		SceneManager.LoadScene("MainMenu");
	}
}
