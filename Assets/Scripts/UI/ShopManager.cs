using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IAP;
public class ShopManager : MonoBehaviour 
{

	[SerializeField]
	private IAPManager manager;
	[SerializeField]
	private PopUp pop;
	[SerializeField]
	private PopUp ppop;
	[SerializeField]
	private int[] skinPrices;
	[SerializeField]
	private string[] skinNames;
	[SerializeField]
	private int[] charPrices;
	[SerializeField]
	private string[] charNames;
	private int price;
	private int id;
	private bool cha;
	public void BuySkin(int i)
	{
		if(skinPrices[i]<=Cash.totalCash)
		{
			price=skinPrices[i];
			cha=false;
			id=i;
			pop.Open("Buy skin "+skinNames[i]+" for "+price+" stars?",Confirm);
		}
		else
		{
			Warning.Open("You need "+skinPrices[i]+" stars to buy this skin!");
		}
	}
	public void BuyChar(int i)
	{
		if(charPrices[i]<=Cash.totalCash)
		{
			price=charPrices[i];
			cha=true;
			id=i;
			pop.Open("Buy pilot "+charNames[i]+" for "+price+" stars?",Confirm);
		}
		else
		{
			Warning.Open("You need "+charPrices[i]+" stars to buy this pilot!");
		}
	}
	public void Premium()
	{
		manager.InitializePurchasing();
		ppop.Open("Buy the Premium Pack: Unlock all characters, 15+ skins, remove all ads, play with 4 continues!" ,IAPManager.Premium);
	}
	public void Confirm()
	{
		Cash.totalCash-=price;
		Cash.Save();
		if(cha)Locks.Char(id,true);
		else Locks.Skin(id,true);
		gameObject.BroadcastMessage("OnEnable");
	}
}
