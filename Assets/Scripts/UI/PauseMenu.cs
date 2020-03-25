using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
	static int player;
	[SerializeField]
	GameObject buttom;
	static PauseMenu menu;
	void Start()
	{
		menu=this;
		gameObject.SetActive(false);
	}
	public static void Pause(int i){
		menu.OnPause(true);
		player=i;
	}
	public void OnPause(bool p)
	{
		Ship.paused = p;
		gameObject.SetActive(p);
		buttom.SetActive(!p);
	}
	void Update()
	{
		if(PlayerInput.GetKeyShotDown(player))OnPause(false);
		if(PlayerInput.GetKeySpecialDown(player))Exit();
	}
	public void Exit()
	{	
		Ship.paused = false;
		Loader.Scene("MenuSelection");
	}
}
