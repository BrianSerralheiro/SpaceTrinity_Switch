using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ADs;

public class MainMenu : MonoBehaviour 
{

	[SerializeField]
	private Text text;
	[SerializeField]
	private GameObject eventSystem;

	private float Timer;
	private AsyncOperation loading;
	
	void Start () 
	{Locks.UnlockAll();
		//PlayerPrefs.DeleteAll();
	}
	
	void Update () 
	{
		if(loading!=null)
		{
			text.text=Mathf.Round((loading.progress+0.1f)*100)+"%";
		}
	}

	public void StartButton()
	{
		eventSystem.SetActive(false);
		loading = SceneManager.LoadSceneAsync("cen");
		SoundManager.PlayEffects(1);
	}


}
