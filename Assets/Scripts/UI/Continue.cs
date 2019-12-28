using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour 
{
	public delegate void Del(bool b);
	public Del Active; 
	[SerializeField]
	private Text continueLog;
	[SerializeField]
	private Button button;
	public Ship ship;
	private void Update()
	{
		button.interactable = false;
		if(!Ship.paused)
		{
			gameObject.SetActive(false);
			Active(false);
		}
		if(ship.input.GetKeyDown("shoot"))buyContinue();
	}
	private void OnEnable()
	{
		continueLog.text="Continues Left: "+Ship.continues[ship.input.id];
	}
	public void WatchAd()
	{
		SoundManager.PlayEffects(0);
		Ship.continues[ship.input.id]--;
	}
	public bool HasContinue()
	{
		return Ship.continues[ship.input.id]>0;
	}
	public void buyContinue()
	{
		if(Cash.totalCash>=0){
			Cash.Save();
			SoundManager.PlayEffects(0);
			Ship.continues[ship.input.id]--;
			gameObject.SetActive(false);
			Active(false);
			ship.Revive();
		}
		else
		{
			SoundManager.PlayEffects(11);
		}
	}

}
