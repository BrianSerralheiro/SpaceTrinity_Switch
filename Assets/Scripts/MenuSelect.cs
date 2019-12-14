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
	Graphic selector,selector2;
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
	int selectionID,selectionID2;
	int[] skinId={-1,-1,-1,-1,-1,-1};
	HashSet<Transform> slots=new HashSet<Transform>();
	Transform[] selected={null,null};
	Vector3 outVector;
	public delegate void Del();
	private Del update;
	private Del Check;
	private RectTransform rect;
	private Vector2 min,max;
	bool p1confirm,p2confirm;
	void Awake()
    {
        //if(rowCount<2)rowCount=2;
		//Locks.Load();
		Check=CheckSelection;
		if(opt.selection==Menuoptions.SelectionType.Character){
			selectionID=Ship.player1;
			for (int i = 0; i < skinId.Length; i++)
			{
				skinId[i]=Ship.skinID[i]+1;
			}
			if(update==null)update=UpdateInputPilot;
			Check+=CheckSkins;
			lightsUP(0, 0.5f);
            lightsUP(selectionID, 1);
			if(PlayerInput.Conected(1)){
				selectionID2=Ship.player2;
				if(selectionID==selectionID2)selectionID2=0;
				lightsUP(selectionID2,1);
			}
		}
		if(update==null)update=UpdateInput;
    }
	void OnEnable()
	{
		if(opt.selection==Menuoptions.SelectionType.None)return;
		OnValueChanged();
		if(opt.selection==Menuoptions.SelectionType.Character && PlayerInput.Conected(1))OnValueChanged2();
		selector2?.gameObject.SetActive(PlayerInput.Conected(1));
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
			else update=UpdateInput;
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
			else update=UpdateInput;
		}
	}
	public void Open(Del d)
	{
		update=d;
	}
	void Update()
    {
		PlayerInput.WaitInput();
		if(opt.selection==Menuoptions.SelectionType.Character){
			if(PlayerInput.recentConect && PlayerInput.Conected(1)){
				selectionID2=Ship.player2;
				if(selectionID2==selectionID)selectionID2++;
				OnValueChanged2();
				lightsUP(selectionID2,1);
				selector2?.gameObject.SetActive(PlayerInput.Conected(1));
			}else if(PlayerInput.recentDisconect && !PlayerInput.Conected(1)){
				selector2?.gameObject.SetActive(PlayerInput.Conected(1));
				if(p2confirm){
					p2confirm=false;
					options[selectionID2].transform.GetChild(2).gameObject.SetActive(false);
					if(selected[1])
					{
						slots.Add(selected[1]);
						selected[1]=null;
					}
				}
				lightsUP(selectionID2,0.5f);
				OnValueChanged2();
			}
		}
		update?.Invoke();
	}
	public void GetInput()
	{
		if(opt.selection==Menuoptions.SelectionType.Character)update=UpdateInputPilot;
		else update=UpdateInput;
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
	void OnValueChanged2(){
		Check();
		if(options[selectionID2])
		{
			selector2.rectTransform.position=options[selectionID2].rectTransform.position;
			selector2.rectTransform.anchoredPosition=options[selectionID2].rectTransform.anchoredPosition;
			selector2.rectTransform.anchorMin=options[selectionID2].rectTransform.anchorMin;
			selector2.rectTransform.anchorMax=options[selectionID2].rectTransform.anchorMax;
			selector2.rectTransform.rotation=options[selectionID2].rectTransform.rotation;
			selector2.color=options[selectionID2].color;
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
	void UpdateInput()
	{
		int id=selectionID;
		if(Input.GetKeyDown(KeyCode.UpArrow))selectionID-=rowCount;
        if(Input.GetKeyDown(KeyCode.DownArrow))selectionID+=rowCount;
        if(Input.GetKeyDown(KeyCode.RightArrow))if(selectionID%rowCount==rowCount-1) selectionID-=rowCount-1;else selectionID++;
        if(Input.GetKeyDown(KeyCode.LeftArrow))if(selectionID%rowCount==0) selectionID+=rowCount-1;else selectionID--;
		if(id!=selectionID)
		{
			OnValueChanged();
			lightsUP(id, 0.5f);
			lightsUP(selectionID, 1);
		}
		
		if(Input.GetKeyDown(confirmKey) && options[selectionID].raycastTarget){
			opt.Select(selectionID,0);
		}
		for(int i=0;i<menus.Length;i++){
			if(menus[i].GetKeyDown()){
				menus[i].Close(this);
				menus[i].Open();
				if(confirmKey==KeyCode.None)
					opt.Select(selectionID,0);
			}
		}
	}
	void UpdateInputPilot2(){
		if(!PlayerInput.Conected(1)){
			return;
		}
		int id=selectionID2;
		int skin = skinId[id];
		if(Input.GetKeyDown(KeyCode.I)&& p2confirm)skinId[id]++;
        if(Input.GetKeyDown(KeyCode.K)&& p2confirm)skinId[id]--;
        if(Input.GetKeyDown(KeyCode.L)&& !p2confirm)selectionID2+=selectionID2+1==selectionID?2:1;
        if(Input.GetKeyDown(KeyCode.J)&& !p2confirm)selectionID2-=selectionID2-1==selectionID?2:1;
		if(id!=selectionID2 || skin != skinId[id])
		{
			OnValueChanged2();
			lightsUP(id, 0.5f);
			lightsUP(selectionID2, 1);
		}
		if(Input.GetKeyDown(PlayerInput.GetKeyShot(1)) && options[selectionID2].raycastTarget){
			if(p1confirm  && p2confirm){
				
			for (int i = 0; i < skinId.Length; i++)
			{
				Ship.skinID[i]=skinId[i]-1;
			}
				Ship.player1=selectionID;
				Ship.player2=selectionID2;
				Loader.Scene("WorldLoader");
			}else {
				p2confirm=true;
				selected[1]=options[selectionID2].transform.GetChild(3);
				options[selectionID2].transform.GetChild(2).gameObject.SetActive(true);
				slots.Remove(selected[1]);
			}
		}
		if(Input.GetKeyDown(PlayerInput.GetKeySpecial(1)) && p2confirm){
			p2confirm=false;
			options[selectionID2].transform.GetChild(2).gameObject.SetActive(false);
			if(selected[1])
			{
				slots.Add(selected[1]);
				selected[1]=null;
			}
		}
	}
	void UpdateInputPilot()
	{
		int id=selectionID;
		int skin = skinId[id];
		if(Input.GetKeyDown(KeyCode.W)&& p1confirm)skinId[id]++;
        if(Input.GetKeyDown(KeyCode.S)&& p1confirm)skinId[id]--;
        if(Input.GetKeyDown(KeyCode.D)&& !p1confirm)selectionID+=PlayerInput.Conected(1) && selectionID2==selectionID+1?2:1;
        if(Input.GetKeyDown(KeyCode.A)&& !p1confirm)selectionID-=PlayerInput.Conected(1) && selectionID2==selectionID-1?2:1;
		if(id!=selectionID || skin != skinId[id])
		{
			OnValueChanged();
			lightsUP(id, 0.5f);
			lightsUP(selectionID, 1);
		}
		if(PlayerInput.Conected(1))UpdateInputPilot2();
		if(Input.GetKeyDown(PlayerInput.GetKeyShot(0)) && options[selectionID].raycastTarget){
			if(p1confirm  && (p2confirm || !PlayerInput.Conected(1))){
				for (int i = 0; i < skinId.Length; i++)
				{
					Ship.skinID[i]=skinId[i]-1;
				}
				Ship.player1=selectionID;
				Ship.player2=selectionID2;
				Loader.Scene("WorldLoader");
			}else{
				p1confirm=true;
				selected[0]=options[selectionID].transform.GetChild(3);
				options[selectionID].transform.GetChild(2).gameObject.SetActive(true);
				slots.Remove(selected[0]);
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
		if(selected[0] && selected[0].position.y<-2.5f)selected[0].Translate(0,Time.deltaTime*3,0);
		if(selected[1] && selected[1].position.y<-2.5f)selected[1].Translate(0,Time.deltaTime*3,0);
		for(int i=0;i<menus.Length;i++){
			if(menus[i].GetKeyDown())
			{
				if(p1confirm)
				{
					p1confirm=false;
					options[selectionID].transform.GetChild(2).gameObject.SetActive(false);
					if(selected[0])
					{
						slots.Add(selected[0]);
						selected[0]=null;
					}
				}else if(!p2confirm){
				menus[i].Close(this);
				menus[i].Open();
				if(confirmKey==KeyCode.None)
					opt.Select(selectionID,skinId[selectionID]-1);
				}
			}
		}
	}
	void CheckSelection()
	{
		if(selectionID>=options.Length){
			selectionID-=options.Length;
			if(selectionID2==selectionID)selectionID++;
		}
		if(selectionID2>=options.Length){
			selectionID2-=options.Length;
			if(selectionID2==selectionID)selectionID2++;
		}
		if(selectionID<0){
			selectionID+=options.Length;
			if(selectionID2==selectionID)selectionID--;
		}
		if(selectionID2<0){
			selectionID2+=options.Length;
			if(selectionID2==selectionID)selectionID2--;
		}
	}
	void CheckSkins()
	{
		if(skinId[selectionID]>3)skinId[selectionID]=0;
		if(skinId[selectionID2]>3)skinId[selectionID2]=0;
		if(skinId[selectionID]<0)skinId[selectionID]=3;
		if(skinId[selectionID2]<0)skinId[selectionID2]=3;
		SkinSwitch.selectedChar[0]=selectionID;
		SkinSwitch.selectedChar[1]=selectionID2;
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
			//vazio por agr
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
