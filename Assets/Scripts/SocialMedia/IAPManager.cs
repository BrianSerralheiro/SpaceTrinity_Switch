using System;
using System.Collections.Generic;
using UnityEngine;

namespace IAP
{
	public class IAPManager :MonoBehaviour
	{
		[SerializeField]
		private GameObject shop;
		private delegate void Dele(int i);
		private static Dele _premium;
		
		private static string[] products={"starpack_1","starpack_2","starpack_3","starpack_4","premium_pack"};
		private static int[] values={100,230,400,1000};

		void Start()
		{
			if(!IsInitialized())
			{
				InitializePurchasing();
			}
		}
		public static void Denitialize()
		{
		}
		public static void Premium()
		{
			_premium(products.Length-1);
		}
		public void InitializePurchasing()
		{
			_premium=BuyProduct;
			if(IsInitialized())
			{
				return;
			}

			
		}


		private bool IsInitialized()
		{
			return false;
		}
		
		public void BuyProduct(int id)
		{
			
		}

	





	}
}