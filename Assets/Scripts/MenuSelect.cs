using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuSelect : MonoBehaviour
{
	[SerializeField]
	Graphic[] options;
	[SerializeField]
	int rowCount=3;
	[SerializeField]
	Graphic selector;
	[SerializeField]
	Text displayText,displayName;
	[SerializeField]
	Image displayImage;
	[SerializeField]
	Sprite[] sprites;
	[SerializeField]
	KeyCode confirmKey;
	[SerializeField]
	private string analog,A,B,X,Y,L,R;
	[SerializeField]
	private Text analogDisplay,Adisplay,Bdisplay,Xdisplay,Ydisplay,Ldisplay,Rdisplay;
	public Menuoptions opt;
	[SerializeField]
	MenuTransition[] menus;
	int selectionID;
	int skinId;
	HashSet<Transform> slots=new HashSet<Transform>();
	Transform selected;
	Vector3 outVector;
	public delegate void Del();
	private Del update;
	private Del Check;
	private RectTransform rect;
	private Vector2 min,max;
	bool p1confirm;
	void Awake()
    {
        //if(rowCount<2)rowCount=2;
		//Locks.Load();
		if(update==null)update=UpdateInputWorld;
		Check=CheckSelection;
		if(opt.selection==Menuoptions.SelectionType.Character){
			selectionID=Ship.playerID;
			skinId=Ship.skinID+1;
			update=UpdateInputPilot;
			Check+=CheckSkins;
		}
		if(update==null)update=UpdateInputWorld;
    }
	void OnEnable()
	{
		OnValueChanged();
		if(displayName)displayName.text=name;
		analogDisplay.gameObject.SetActive(!string.IsNullOrEmpty(analog));
		analogDisplay.text=analog;
		Adisplay.gameObject.SetActive(!string.IsNullOrEmpty(A));
		Adisplay.text=A;
		Bdisplay.gameObject.SetActive(!string.IsNullOrEmpty(B));
		Bdisplay.text=B;
		Xdisplay.gameObject.SetActive(!string.IsNullOrEmpty(X));
		Xdisplay.text=X;
		Ydisplay.gameObject.SetActive(!string.IsNullOrEmpty(Y));
		Ydisplay.text=Y;
		Ldisplay.gameObject.SetActive(!string.IsNullOrEmpty(L));
		Ldisplay.text=L;
		Rdisplay.gameObject.SetActive(!string.IsNullOrEmpty(R));
		Rdisplay.text=R;
	}
	void MovingIn()
	{
		Vector3 vector=Vector3.zero;
		transform.position=Vector3.MoveTowards(transform.position,vector,Time.deltaTime*20);
		if((transform.position-vector).sqrMagnitude<0.1f){
			transform.position=vector;
			if(opt.selection==Menuoptions.SelectionType.Character)update=UpdateInputPilot;
			else update=UpdateInputWorld;
		}
	}
	void MovingOut()
	{
		transform.position=Vector3.MoveTowards(transform.position,outVector,Time.deltaTime*20);
		if((transform.position-outVector).sqrMagnitude<0.1f){
			gameObject.SetActive(false);
		}
	}
	void Shrinking()
	{
		rect.anchorMin=Vector2.MoveTowards(rect.anchorMin,min,Time.deltaTime);
		rect.anchorMax=Vector2.MoveTowards(rect.anchorMax,max,Time.deltaTime);
		if(rect.anchorMax==max && rect.anchorMin==min)gameObject.SetActive(false);
	}
	void Expanding()
	{
		rect.anchorMin=Vector2.MoveTowards(rect.anchorMin,Vector2.zero,Time.deltaTime);
		rect.anchorMax=Vector2.MoveTowards(rect.anchorMax,Vector2.one,Time.deltaTime);
		if(rect.anchorMax.x==1){
			if(opt.selection==Menuoptions.SelectionType.Character)update=UpdateInputPilot;
			else update=UpdateInputWorld;
		}
	}
	public void Open(Del d)
	{
		update=d;
	}
	void Update()
    {
		update?.Invoke();
	}
	public void GetInput()
	{
		update=UpdateInputWorld;
	}
	void lightsUP(int i, float f)
	{
		if (opt.selection == Menuoptions.SelectionType.Character && options[i])
		{
			int b = 0;
			foreach (Graphic graphic in options[i].GetComponentsInChildren<Graphic>())
			{
				if(b++ == 1)
				{
					continue;
				}
				Color c = graphic.color;
				c.a = f;
				graphic.color = c;
			}
		}
	}
	void OnValueChanged()
	{
		Check();
		if(options[selectionID])
		{
			selector.rectTransform.position=options[selectionID].rectTransform.position;
			selector.rectTransform.anchoredPosition=options[selectionID].rectTransform.anchoredPosition;
			selector.rectTransform.anchorMin=options[selectionID].rectTransform.anchorMin;
			selector.rectTransform.anchorMax=options[selectionID].rectTransform.anchorMax;
			selector.rectTransform.rotation=options[selectionID].rectTransform.rotation;
			selector.color=options[selectionID].color;
			if(displayText){
				displayText.text=options[selectionID].name;
				displayText.color=options[selectionID].color;
			}
			if(displayImage)displayImage.sprite=sprites[selectionID];
		}
		
	}
	void UpdateInputWorld()
	{
		int id=selectionID;
		if(Input.GetKeyDown(KeyCode.UpArrow))selectionID-=rowCount;
        if(Input.GetKeyDown(KeyCode.DownArrow))selectionID+=rowCount;
        if(Input.GetKeyDown(KeyCode.RightArrow))selectionID++;
        if(Input.GetKeyDown(KeyCode.LeftArrow))selectionID--;
		if(id!=selectionID)
		{
			OnValueChanged();
			lightsUP(id, 0.5f);
			lightsUP(selectionID, 1);
		}
		
		if(Input.GetKeyDown(confirmKey) && options[selectionID].raycastTarget){
			opt.Select(selectionID,skinId-1);
		}
		for(int i=0;i<menus.Length;i++){
			if(menus[i].GetKeyDown()){
				menus[i].Close(this);
				menus[i].Open();
				if(confirmKey==KeyCode.None)
					opt.Select(selectionID,skinId-1);
			}
		}
	}
	void UpdateInputPilot()
	{
		int id=selectionID;
		if(Input.GetKeyDown(KeyCode.UpArrow)&& p1confirm)skinId++;
        if(Input.GetKeyDown(KeyCode.DownArrow)&& p1confirm)skinId--;
        if(Input.GetKeyDown(KeyCode.RightArrow)&& !p1confirm)selectionID++;
        if(Input.GetKeyDown(KeyCode.LeftArrow)&& !p1confirm)selectionID--;
		if(id!=selectionID)
		{
			OnValueChanged();
			lightsUP(id, 0.5f);
			lightsUP(selectionID, 1);
		}
		
		if(Input.GetKeyDown(confirmKey) && options[selectionID].raycastTarget){
			Debug.Log(p1confirm);
			if(p1confirm){
				opt.Select(selectionID,skinId-1);
			}else {
				p1confirm=true;
				selected=options[selectionID].transform.GetChild(3);
				options[selectionID].transform.GetChild(2).gameObject.SetActive(true);
				slots.Remove(selected);
			}
		}
		foreach (Transform t in slots)
		{
			if(t.position.y>-4.5f)t.Translate(0,-Time.deltaTime*3,0);
			else {
				slots.Remove(t);
				break;
			}
		}
		if(selected && selected.position.y<-2.5f)selected.Translate(0,Time.deltaTime*3,0);
		for(int i=0;i<menus.Length;i++){
			if(menus[i].GetKeyDown())
			{
				if(p1confirm)
				{
					p1confirm=false;
					options[selectionID].transform.GetChild(2).gameObject.SetActive(false);
					if(selected)
					{
						slots.Add(selected);
						selected=null;
					}
				}else{
				menus[i].Close(this);
				menus[i].Open();
				if(confirmKey==KeyCode.None)
					opt.Select(selectionID,skinId-1);
				}
			}
		}
	}
	void CheckSelection()
	{
		if(selectionID>=options.Length)selectionID-=options.Length;
		if(selectionID<0)selectionID+=options.Length;
	}
	void CheckSkins()
	{
		//if(skinId>0 && !Locks.Skin(selectionID*3+skinId-1))skinId++;
		if(skinId>3)skinId=0;
		if(skinId<0)skinId=3;
		SkinSwitch.selectedChar=selectionID;
		SkinSwitch.selectedSkin=skinId;
	}
	public void Open(Vector3 vector)
	{
		transform.position=vector;
		update=MovingIn;
		gameObject.SetActive(true);
	}
	public void Close(Vector3 vector)
	{
		outVector=vector;
		update=MovingOut;
	}
	public void Expand(Image i)
	{
		rect=transform as RectTransform;
		rect.anchorMin=i.rectTransform.anchorMin;
		rect.anchorMax=i.rectTransform.anchorMax;
		GetComponent<Image>().sprite=i.sprite;
		update=Expanding;
		gameObject.SetActive(true);
	}
	public void Shrink(RectTransform rt)
	{
		rect=transform as RectTransform;
		rect.anchorMin=Vector2.zero;
		rect.anchorMax=Vector2.one;
		min=rt.anchorMin;
		max=rt.anchorMax;
		update=Shrinking;
	}
}
[System.Serializable]
public struct Menuoptions
{
	public enum SelectionType
	{
		World,Character,Weapon,Shop,None
	}
	public SelectionType selection;
	public UnityEvent comand;
	public WorldInfo[] worlds;
	public void Select(int i,int  j)
	{
		switch(selection){
			case SelectionType.World:
				EnemySpawner.world=worlds[i];
				return;
			case SelectionType.Character:
				Ship.playerID=i;
				Ship.skinID=j;
				if(EnemySpawner.world)Loader.Scene("WorldLoader");
				return;
			case SelectionType.Weapon:
				//implementar equips
				return;
			case SelectionType.Shop:
				ShopManager.buyID=i;
				comand?.Invoke();
				return;
		}
	}
}
