using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    GameObject spawner;
    [SerializeField]
    Text speechText;
    [SerializeField]
    Image[] actors;
    [SerializeField]
    Color lit,unlit;
    [SerializeField]
    DialogInfo dialogInfo;
    [SerializeField]
    Dialog dialog;
    [SerializeField]
    Speech speech;
    float charCount;
    int cps=15;
    void Start()
    {
        dialogInfo=EnemySpawner.world.begining;
        if(!dialogInfo){
            gameObject.SetActive(false);
            return;
        }
        dialogInfo.id=0;
        dialog=dialogInfo.Next();
        if(dialog.IsEmpty())gameObject.SetActive(false);
        if(dialog.HasSpeech())Speech(dialog.GetSpeech());
    }
    void OnDisable()
    {
        spawner.SetActive(true);
    }
    void Update()
    {
        charCount+=cps*Time.deltaTime;
        if(speechText.text.Length<(int)charCount && charCount<=speech.text.Length+1)
            speechText.text=speech.text.Substring(0,(int)charCount);
        if(PlayerInput.GetKeyShotDown(0)){
            if(charCount>speech.text.Length){
                if(dialog.HasSpeech())Speech(dialog.GetSpeech());
                else {
                    dialog=dialogInfo.Next();
                    if(dialog.IsEmpty() || !dialog.HasSpeech())gameObject.SetActive(false);
                    else Speech(dialog.GetSpeech());
                }
            }
            else {
                charCount=speech.text.Length;
                speechText.text=speech.text;
            }
        } 
        for (int i = 0; i < actors.Length; i++)
        {
            if(actors[i].sprite){
                float f=speech.characters[i].proportion;
                actors[i].transform.localScale=Vector3.MoveTowards(actors[i].transform.localScale,new Vector3(f,Mathf.Abs(f)),Time.deltaTime*2);
                Vector3 pos=actors[i].transform.position;
                pos.x=Mathf.MoveTowards(pos.x,speech.characters[i].position*Screen.width,Screen.width*Time.deltaTime);
                actors[i].transform.position=pos;
            }
        }
    }
    void Speech(Speech s){
        speech=s;
        charCount=0;
        speechText.text="";
        for (int i = 0; i < actors.Length; i++)
        {
            if(speech.characters.Length<=i)actors[i].enabled=false;
            else{
                if(!actors[i].enabled){
                    Vector3 pos=actors[i].transform.position;
                    pos.x=(Mathf.Round(speech.characters[i].position)*2-0.5f)*Screen.width;
                    actors[i].transform.position=pos;
                }
                float f=speech.characters[i].proportion;
                actors[i].transform.localScale=new Vector3(f,Mathf.Abs(f));
                actors[i].enabled=true;
                actors[i].sprite=speech.characters[i].picture;
                actors[i].color=speech.characters[i].lit?lit:unlit;
            }
        }
    }
}
