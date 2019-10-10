	using System;
	using System.Collections.Generic;
	using UnityEngine;

	namespace ADs{
	public static class adsManager 
	{
		private static string appID="3278506";

		private static bool revive;

		//private static ShowOptions options = new ShowOptions{resultCallback = HandleReward};

		public static bool LoadedVideo()
		{
			return false;
		}
		public static void ShowAd(bool d)
		{
			if(LoadedVideo())
			{
				//Advertisement.Show(options);
				revive=d;
			}
		}
		public static void HandleReward()
		{
			
		}

		public static void Initialize()
		{
		}

		} }