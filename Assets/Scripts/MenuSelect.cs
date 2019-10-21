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
	Text displayText;
	[SerializeField]
	Image displayImage;
	[SerializeField]
	Sprite[] sprites;
	[SerializeField]
	KeyCode confirmKey;
	public Menuoptions opt;
	[SerializeField]
	MenuTransition[] menus;
	int selectionID;
	int skinId;
	Vector3 outVector;
	private delegate void Del();
	private Del update;
	private Del Check;
	void Awake()
    {
        if(rowCount<2)rowCount=2;
		//Locks.Load();
		update=MovingIn;
		Check=CheckSelection;
		if(opt.selection==Menuoptions.SelectionType.Character)Check+=CheckSkins;
		if(opt.selection==Menuoptions.SelectionType.Character){
			selectionID=Ship.playerID;
			skinId=Ship.skinID+1;
		}
    }
	void OnEnable()
	{
		selector.rectTransform.position=options[selectionID].rectTransform.position;
		selector.rectTransform.anchoredPosition=options[selectionID].rectTransform.anchoredPosition;
		selector.rectTransform.anchorMin=options[selectionID].rectTransform.anchorMin;
		selector.rectTransform.anchorMax=options[selectionID].rectTransform.anchorMax;
		selector.rectTransform.rotation=options[selectionID].rectTransform.rotation;
		if(displayText){
				displayText.text=options[selectionID].name;
				displayText.color=options[selectionID].color;
		}
		if(displayImage)displayImage.sprite=sprites[selectionID];
	}
	void MovingIn(){
		Vector3 vector=new Vector3(Screen.width/2,Screen.height/2);
		transform.position=Vector3.MoveTowards(transform.position,vector,Screen.width*Time.deltaTime);
		if((transform.position-vector).sqrMagnitude<0.1f){
			transform.position=vector;
			update=UpdateInput;
		}
	}
	void MovingOut(){
		transform.position=Vector3.MoveTowards(transform.position,outVector,Screen.width* Time.deltaTime);
		if((transform.position-outVector).sqrMagnitude<0.1f){
			gameObject.SetActive(false);
		}
	}
	void Update()
    {
		update();
	}
	void UpdateInput(){
		if(Input.GetKeyDown(KeyCode.UpArrow))if(opt.selection==Menuoptions.SelectionType.Character)skinId++;else selectionID-=rowCount;
        if(Input.GetKeyDown(KeyCode.DownArrow))if(opt.selection==Menuoptions.SelectionType.Character)skinId--;else selectionID+=rowCount;
        if(Input.GetKeyDown(KeyCode.RightArrow))if(selectionID%rowCount==rowCount-1) selectionID-=rowCount-1;else selectionID++;
        if(Input.GetKeyDown(KeyCode.LeftArrow))if(selectionID%rowCount==0) selectionID+=rowCount-1;else selectionID--;
		Check();

		if(options[selectionID])
		{
			selector.rectTransform.position=options[selectionID].rectTransform.position;
			selector.rectTransform.anchoredPosition=options[selectionID].rectTransform.anchoredPosition;
			selector.rectTransform.anchorMin=options[selectionID].rectTransform.anchorMin;
			selector.rectTransform.anchorMax=options[selectionID].rectTransform.anchorMax;
			selector.rectTransform.rotation=options[selectionID].rectTransform.rotation;
			if(displayText){
				displayText.text=options[selectionID].name;
				displayText.color=options[selectionID].color;
			}
			if(displayImage)displayImage.sprite=sprites[selectionID];
		}
		if(Input.GetKeyDown(confirmKey) && options[selectionID].raycastTarget){
			opt.Select(selectionID,skinId-1);
		}
		for(int i=0;i<menus.Length;i++){
			if(menus[i].GetKeyDown()){
				menus[i].Open();
				menus[i].Close(this);
				if(confirmKey==KeyCode.None)
					opt.Select(selectionID,skinId-1);
			}
		}
	}
	void CheckSelection(){
		if(selectionID>=options.Length)selectionID-=options.Length;
		if(selectionID<0)selectionID+=options.Length;
	}
	void CheckSkins(){
		//if(skinId>0 && !Locks.Skin(selectionID*3+skinId-1))skinId++;
		if(skinId>3)skinId=0;
		if(skinId<0)skinId=3;
		SkinSwitch.selectedChar=selectionID;
		SkinSwitch.selectedSkin=skinId;
	}
	public void Open(Vector3 vector){
		transform.position=vector;
		update=MovingIn;
		gameObject.SetActive(true);
	}
	public void Close(Vector3 vector){
		outVector=vector;
		update=MovingOut;
	}
}
[System.Serializable]
public struct Menuoptions
{
	public enum SelectionType
	{
		World,Character,Weapon,Shop
	}
	public SelectionType selection;
	public UnityEvent comand;
	public WorldInfo[] worlds;
	public void Select(int i,int  j){
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
				//implementar shop
				ShopManager.buyID=i;
				comand?.Invoke();
				return;
		}
	}
}
