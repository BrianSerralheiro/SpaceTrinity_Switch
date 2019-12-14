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
	private int continues;
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
	public void Continues(int i)
	{
		continues=i;
	}
	private void OnEnable()
	{
		continueLog.text="Continues Left: "+continues;
	}
	public void WatchAd()
	{
		SoundManager.PlayEffects(0);
		continues--;
	}
	public bool HasContinue()
	{
		return continues>0;
	}
	public void buyContinue()
	{
		if(Cash.totalCash>=0){
			Cash.Save();
			SoundManager.PlayEffects(0);
			continues--;
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
