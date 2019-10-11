using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
	public void OnPause(bool p)
	{
		Ship.paused = p;
	}

	public void Exit()
	{	
		Ship.paused = false;
		Loader.Scene("SelectionTest");
	}
}
