﻿using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour 
{

	[SerializeField]
	private PopUp pop;
	[SerializeField]
	private MenuSelect menu;
	[SerializeField]
	private int[] skinPrices;
	[SerializeField]
	private string[] skinNames;
	[SerializeField]
	private int[] charPrices;
	[SerializeField]
	private string[] charNames;
	private int price;
	public static int buyID;
	public void BuySkin()
	{
		if(skinPrices[buyID]<=Cash.totalCash)
		{
			price=skinPrices[buyID];
			pop.Open("Buy skin "+skinNames[buyID]+" for "+price+" stars?",Confirm,menu);
		}
		else
		{
			Warning.Open("You need "+skinPrices[buyID]+" stars to buy this skin!");
		}
	}
	/*reusar caso preciso
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
	*/
	public void Premium()
	{
		pop.Open("Buy the Premium Pack: Unlock all characters, 15+ skins, remove all ads, play with 4 continues!" ,Confirm,menu);
	}
	public void Confirm()
	{
		//Cash.totalCash-=price;
		//Cash.Save();
		Locks.Skin(buyID,true);
		gameObject.BroadcastMessage("OnEnable");
	}
}
