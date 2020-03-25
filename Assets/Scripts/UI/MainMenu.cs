using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using LanguagePack;

public class MainMenu : MonoBehaviour 
{

	[SerializeField]
	RectTransform[] options,groups,marks;
	[SerializeField]
	RectTransform selector,follow;
	[SerializeField]
	Slider music,sfx;
	int id,sub,spd;
	bool seleted;
	float time;
	delegate void Del();
	Del suboption;
	void Awake() 
	{
		Language.Load();
		Language.LoadMenu();
        UITranslatorRT.Update();
		SoundManager.Load();
		selector.position=options[0].position;
		selector.sizeDelta=options[0].rect.size;
		suboption=MainControl;
		follow=options[0];
		spd=Screen.width;
		music.value=SoundManager.GetVolumeSNG();
		sfx.value=SoundManager.GetVolumeSFX();
	}
	void Update () 
	{
		suboption();
		selector.position=Vector3.MoveTowards(selector.position,follow.position,Time.deltaTime*spd);
		selector.sizeDelta=Vector2.MoveTowards(selector.sizeDelta,follow.rect.size,Time.deltaTime*spd);	
	}
	void MainControl(){
		for (int i = 0; i < options.Length; i++)
		{
			if(groups[i].anchorMin==options[i].anchorMin)groups[i].gameObject.SetActive(false);
			else{
				groups[i].anchorMin=Vector2.MoveTowards(groups[i].anchorMin,options[i].anchorMin,Time.deltaTime/2);
				groups[i].anchorMax=Vector2.MoveTowards(groups[i].anchorMax,options[i].anchorMax,Time.deltaTime/2);
			}
		}
		if(PlayerInput.GetDir(0,true)==-1 && time<Time.time){
			id++;
			if(id>=options.Length)id=0;
			follow=options[id];
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetDir(0,true)==1 && time<Time.time){
			id--;
			if(id<0)id=options.Length-1;
			follow=options[id];
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetButtomDown("special")){
			id=options.Length-1;
			Action();
		}
		if(PlayerInput.GetButtomDown("shot")){
			Action();
		}
	}
	void ConfirmNew(){
		if(PlayerInput.GetDir(0,false)!=0 && time<Time.time){
			sub=sub==0?1:0;
			follow=groups[id].GetChild(sub+1)as RectTransform;
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetButtomDown("special")){
			follow=options[id];
			suboption=MainControl;
		}
		if(PlayerInput.GetButtomDown("shot")){
			if(sub==0)Loader.Scene("Intro");
			else suboption=MainControl;
			follow=options[id];
		}
		groups[id].anchorMin=Vector2.MoveTowards(groups[id].anchorMin,marks[id].anchorMin,Time.deltaTime);
		groups[id].anchorMax=Vector2.MoveTowards(groups[id].anchorMax,marks[id].anchorMax,Time.deltaTime);
	}
	void ConfirmContinue(){
		if(PlayerInput.GetDir(0,false)!=0 && time<Time.time){
			sub=sub==0?1:0;
			follow=groups[id].GetChild(sub+1)as RectTransform;
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetButtomDown("special")){
			suboption=MainControl;
			follow=options[id];
		}
		if(PlayerInput.GetButtomDown("shot")){
			if(sub==0)Loader.Scene("MenuSelection");
			else suboption=MainControl;
			follow=options[id];
		}
		groups[id].anchorMin=Vector2.MoveTowards(groups[id].anchorMin,marks[id].anchorMin,Time.deltaTime);
		groups[id].anchorMax=Vector2.MoveTowards(groups[id].anchorMax,marks[id].anchorMax,Time.deltaTime);
	}
	void Options(){
		if(PlayerInput.GetDir(0,true)==-1 && time<Time.time){
			sub++;
			if(sub>2)sub=0;
			follow=groups[id].GetChild(sub)as RectTransform;
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetDir(0,true)==1 && time<Time.time){
			sub--;
			if(sub<0)sub=2;
			follow=groups[id].GetChild(sub)as RectTransform;
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetDir(0,false)==1 && time<Time.time){
			switch (sub)
			{
				case 0:
					music.value+=0.1f;
					SoundManager.VolumeMusic(music.value);
					break;
				case 1:
					sfx.value+=0.1f;
					SoundManager.VolumeMusic(sfx.value);
					break;
				case 2:
					Language.id++;
					break;
			}
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetDir(0,false)==-1 && time<Time.time){
			switch (sub)
			{
				case 0:
					music.value-=0.1f;
					SoundManager.VolumeMusic(music.value);
					break;
				case 1:
					sfx.value-=0.1f;
					SoundManager.VolumeSFX(sfx.value);
					break;
				case 2:
					Language.id--;
					break;
			}
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetButtomDown("special")){
			follow=options[id];
			suboption=MainControl;
		}
		groups[id].anchorMin=Vector2.MoveTowards(groups[id].anchorMin,marks[id].anchorMin,Time.deltaTime);
		groups[id].anchorMax=Vector2.MoveTowards(groups[id].anchorMax,marks[id].anchorMax,Time.deltaTime);
	}
	void ConfirmQuit(){
		if(PlayerInput.GetDir(0,false)!=0 && time<Time.time){
			sub=sub==0?1:0;
			follow=groups[id].GetChild(sub+1)as RectTransform;
			time=Time.time+0.2f;
		}
		if(PlayerInput.GetButtomDown("special")){
			follow=options[id];
			suboption=MainControl;
		}
		if(PlayerInput.GetButtomDown("shot")){
			if(sub==0){
				Application.Quit();
				#if UNITY_EDITOR
					EditorApplication.isPlaying=false;
				#endif
			}
			else suboption=MainControl;
			follow=options[id];
		}
		groups[id].anchorMin=Vector2.MoveTowards(groups[id].anchorMin,marks[id].anchorMin,Time.deltaTime);
		groups[id].anchorMax=Vector2.MoveTowards(groups[id].anchorMax,marks[id].anchorMax,Time.deltaTime);
	}
	void Action(){
		sub=0;
		switch (id)
		{
			case 0:
				follow=groups[id].GetChild(sub+1)as RectTransform;
				suboption=ConfirmNew;
				break;
			case 1:
				suboption=ConfirmContinue;
				follow=groups[id].GetChild(sub+1)as RectTransform;
				break;
			case 2:
				follow=groups[id].GetChild(0)as RectTransform;
				sub=Language.id;
				suboption=Options;
				break;
			case 3:
				suboption=ConfirmQuit;
				follow=groups[id].GetChild(sub+1)as RectTransform;
				break;
		}
		groups[id].gameObject.SetActive(true);
	}
	void OnDestroy()
	{
		SoundManager.Save();
		Language.Save();
	}
}
