using UnityEngine;
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
		  introColor = tapStart.color;
		  introColor.a = Mathf.Abs(Mathf.Cos(Time.time * 2));
		  tapStart.color = introColor;
		  introColor = tapStartOutline.color;
		  introColor.a = Mathf.Abs(Mathf.Cos(Time.time * 2));
		  tapStartOutline.color = introColor;
	}

	public void OnTap()
	{
		Loader.Scene("SelectionTest");
	}
}
