using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ADs;

public class storeManager : MonoBehaviour 
{

	[SerializeField]
	private Button like;
	[SerializeField]
	private Button share;
	[SerializeField]
	private Button invite;
	[SerializeField]
	private Button ad;
	private UnityWebRequest webRequest;
	private bool watched;
	void Update()
	{
		if(webRequest!=null  && webRequest.GetResponseHeader("date")!=null)
		{
			if(watched)
			{
				PlayerPrefs.SetString("adday",webRequest.GetResponseHeader("date").Substring(0,10));
				watched = false;
			}
			ad.interactable= adsManager.LoadedVideo() && PlayerPrefs.GetString("adday")!=webRequest.GetResponseHeader("date").Substring(0,10);
			webRequest = UnityWebRequest.Get("https://www.worldtimeserver.com");
			webRequest.Send();
			//Enable();
			//webRequest=null;
		}
		
	}
	void OnEnable()
	{
		//if(Locks.IsPremium())Warning.Open("You are  premuim user, you already have everything in the store!");
		//else {
		webRequest = UnityWebRequest.Get("https://www.worldtimeserver.com");
		webRequest.Send();
		Enable();
		//}
	} 
	void Enable ()
	{
		like.interactable=!PlayerPrefs.HasKey("like");
		share.interactable=!PlayerPrefs.HasKey("share");
		invite.interactable=!PlayerPrefs.HasKey("invite");
	}
	

	public void inviteFriends()
	{
		PlayerPrefs.SetInt("invite",0);
		//precisa mudar o link
		Application.OpenURL("https://www.facebook.com/sharer/sharer?u=https://youtu.be/iHUDvD6dMAE");
		Enable();
		Cash.totalCash += 20;
		Cash.Save();
		Warning.Open("You got 20 Stars!");
	}

	public void shareApp()
	{
		PlayerPrefs.SetInt("share",0);
		Application.OpenURL("https://www.facebook.com/sharer/sharer?u=www.facebook.com/OverclockEntretenimento");
		Enable();
		Cash.totalCash += 20;
		Cash.Save();
		Warning.Open("You got 20 Stars!");
	}

	public void playAD()
	{
		watched = true;
		webRequest = UnityWebRequest.Get("https://www.worldtimeserver.com");
		webRequest.Send();
		adsManager.ShowAd(false);
		Warning.Open("You got 20 Stars!");
	}

	public void facebookPage()
	{
		PlayerPrefs.SetInt("like",0);
		Application.OpenURL("https://www.facebook.com/OverclockEntretenimento/");
		Enable();
		Cash.totalCash += 20;
		Cash.Save();
		Warning.Open("You got 20 Stars!");
	}
}
