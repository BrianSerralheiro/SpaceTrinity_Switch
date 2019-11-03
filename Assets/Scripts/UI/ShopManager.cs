﻿using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour 
{

	[SerializeField]
	private PopUp pop;
	[SerializeField]
	private MenuSelect menu;
	[SerializeField]
	private Text sellerDialog;
	[SerializeField]
	private Scrollbar slider;
	float timer=2;
	[SerializeField]
	private RectTransform selector;
	[SerializeField]
	private int[] skinPrices;
	[SerializeField]
	private string[] skinNames;
	[SerializeField]
	private int[] charPrices;
	[SerializeField]
	private string[] charNames;
	[SerializeField]
	private Image[] slots;
	[SerializeField]
	private Sprite[] chibis;
	private int price;
	public static int buyID;
	void Update()
	{
		if(timer>0){
			timer-=Time.deltaTime;
			if(timer<=0){
				sellerDialog.text="Chose the pattern you want to buy.";
				int i=buyID/6;
				if(slots[i])slots[i].sprite=chibis[i*2];
			}
		}
		if(slider)slider.value=selector.anchorMin.y/0.81f;
	}
	void OnEnable()
	{
		sellerDialog.text="Welcome!";
		for (int i = 0; i < slots.Length; i++)
		{
			if(slots[i])slots[i].sprite=chibis[i/6*2];
		}
		timer=2;
	}
	public void BuySkin()
	{
		if(skinPrices[buyID]<=Cash.totalCash)
		{
			price=skinPrices[buyID];
			pop.Open("Buy skin "+skinNames[buyID]+" for "+price+" stars?",Confirm,Cancel,menu);
			sellerDialog.text="Buy this patten?";
		}
		else
		{
			Warning.Open("You need "+skinPrices[buyID]+" stars to buy this skin!");
			sellerDialog.text="You can't buy this patten!";
		}
		timer=0;
	}
	void Cancel(){
		timer=Time.deltaTime;
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
	
	public void Premium()
	{
		pop.Open("Buy the Premium Pack: Unlock all characters, 15+ skins, remove all ads, play with 4 continues!" ,Confirm,menu);
	}*/
	public void Confirm()
	{
		//Cash.totalCash-=price;
		//Cash.Save();
		Locks.Skin(buyID,true);
		gameObject.BroadcastMessage("OnEnable");
		sellerDialog.text="Thank you for your buy.";
		int i=buyID/6;
		if(slots[i])slots[i].sprite=chibis[i*2+1];
		timer=2;
	}
}
