using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ADs;

public class IntroScene : MonoBehaviour 
{
	[SerializeField]
	private RawImage IntroCharacters;
	[SerializeField]
	private Text tapStart;
	[SerializeField]
	private Text tapStartOutline;
	[SerializeField]
	private AudioSource audioHandler;

	private Color introColor;
	private AsyncOperation loading;

	void Start () 
	{
		adsManager.Initialize();
	}
	
	void Update () 
	{
		if(loading != null)
		{
			tapStart.text = Mathf.Round((loading.progress + 0.1f) * 1000) / 10f + "%";
			tapStartOutline.text = tapStart.text;
			audioHandler.volume = 1f - loading.progress;
		}
		  introColor = tapStart.color;
		  introColor.a = Mathf.Abs(Mathf.Cos(Time.time * 2));
		  tapStart.color = introColor;
		  introColor = tapStartOutline.color;
		  introColor.a = Mathf.Abs(Mathf.Cos(Time.time * 2));
		  tapStartOutline.color = introColor;
	}

	public void OnTap()
	{
		if(loading == null)
		{
			loading = SceneManager.LoadSceneAsync("MainMenu");
		} 
	}
}
