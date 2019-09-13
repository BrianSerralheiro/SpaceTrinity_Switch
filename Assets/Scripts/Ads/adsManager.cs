	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Advertisements;

	namespace ADs{
	public static class adsManager 
	{
		private static string appID="3278506";

		private static bool revive;

		private static ShowOptions options = new ShowOptions{resultCallback = HandleReward};

		public static bool LoadedVideo()
		{
			return !Locks.IsPremium() && Advertisement.IsReady();
		}
		public static void ShowAd(bool d)
		{
			if(LoadedVideo())
			{
				Advertisement.Show(options);
				revive=d;
			}
		}
		public static void HandleReward(ShowResult result)
		{
			switch(result)
			{
				case ShowResult.Finished:
				if(revive)
				{
				EnemyBase.player.GetComponent<Ship>().Revive();
				}
				else 
				{
				Cash.totalCash += 20;
				Cash.Save();
				Warning.Open("Received 20 stars!");
				}
				break;
				case ShowResult.Failed:
				Warning.Open("Ad failed to load.");
				break;
			}
		}

		public static void Initialize()
		{
			Advertisement.Initialize(appID);
		}

		} }