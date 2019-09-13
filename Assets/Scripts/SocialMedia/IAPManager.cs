using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace IAP
{
	public class IAPManager :MonoBehaviour, IStoreListener
	{
		[SerializeField]
		private GameObject shop;
		private delegate void Dele(int i);
		private static Dele _premium;
		private static IStoreController storeController;
		private static IExtensionProvider storeExtensionProvider;
		
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
			storeController=null;
			storeExtensionProvider=null;
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

			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			for(int i=0;i<5;i++)
			{
				builder.AddProduct(products[i],i==4? ProductType.NonConsumable : ProductType.Consumable);
			}
			UnityPurchasing.Initialize(this,builder);
		}


		private bool IsInitialized()
		{
			return storeController != null && storeExtensionProvider != null;
		}
		
		public void BuyProduct(int id)
		{
			if(IsInitialized() && id>=0 && id<5)
			{
				Product product = storeController.products.WithID(products[id]);

				if(product != null && product.availableToPurchase)
				{
					storeController.InitiatePurchase(product);
				}
				else
				{
					Warning.Open("BuyProductID: FAIL. product not purchased, either is not found or is not available for purchase");
				}
			}
			else
			{
				Warning.Open("BuyProductID FAIL. Not initialized.");
			}
		}

		//  
		// --- IStoreListener
		//

		public void OnInitialized(IStoreController controller,IExtensionProvider extensions)
		{	
			storeController = controller;
			storeExtensionProvider = extensions;
		}


		public void OnInitializeFailed(InitializationFailureReason error)
		{
			Warning.Open("Initialization failed, Reason:" + error);
		}


		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			for(int i=0;i<5;i++){
				if(String.Equals(args.purchasedProduct.definition.id,products[i],StringComparison.Ordinal))
				{
					Warning.Open("Purchase Succsefull! Thank you for supporting us.");
					if(shop) shop.BroadcastMessage("OnEnale");
					if(i==4)
						Locks.UnlockAll();
					else 
						Cash.totalCash += values[i];
					Cash.Save();
					return PurchaseProcessingResult.Complete;
				}
			}
			Warning.Open(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'",args.purchasedProduct.definition.id));
			return PurchaseProcessingResult.Complete;
		}


		public void OnPurchaseFailed(Product product,PurchaseFailureReason failureReason)
		{
			Warning.Open(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId,failureReason));
		}
	}
}