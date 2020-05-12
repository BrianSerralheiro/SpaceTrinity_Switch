using UnityEngine.UI;
using UnityEngine;
using LanguagePack;

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
    [Range(0.1f,1)]
    float  speed=1;
    [SerializeField]
    [Range(0.5f,2)]
    float expand=2;
    [SerializeField]
    int cps=15;
    [SerializeField]
    DialogInfo dialogInfo;
    Dialog dialog;
    Speech speech;
    float charCount;
    string fulltext;
    static DialogManager manager;
    public static DialogInfo open{
        set{
            manager.dialogInfo=value;
            if(value)manager.gameObject.SetActive(true);
        }
    }
    void Awake()
    {
        manager=this;
    }
    void OnEnable(){
        // dialogInfo=EnemySpawner.world.begining;
        if(!dialogInfo){
            gameObject.SetActive(false);
            return;
        }
        //spawner.SetActive(false);
        dialogInfo.id=0;
        Ship.paused=true;
        dialog=dialogInfo.Next();
        if(dialog.IsEmpty())gameObject.SetActive(false);
        Language.LoadDialog(dialogInfo.dialogName);
        if(dialog.HasSpeech())Speech(dialog.GetSpeech());
    }
    void OnDisable()
    {
        Ship.paused=false;
        // spawner.SetActive(true);
    }
    void Update()
    {
        charCount+=cps*Time.unscaledDeltaTime;
        if(speechText.text.Length<(int)charCount && charCount<=fulltext.Length+1)
            speechText.text=fulltext.Substring(0,(int)charCount);
        if(PlayerInput.GetKeyShotDown(0)){
            if(charCount>fulltext.Length){
                if(dialog.HasSpeech())Speech(dialog.GetSpeech());
                else {
                    dialog=dialogInfo.Next();
                    if(dialog.IsEmpty() || !dialog.HasSpeech())gameObject.SetActive(false);
                    else Speech(dialog.GetSpeech());
                }
            }
            else {
                charCount=fulltext.Length;
                speechText.text=fulltext;
            }
        } 
        for (int i = 0; i < actors.Length; i++)
        {
            if(actors[i].sprite){
                float f=speech.characters[i].proportion;
                actors[i].transform.localScale=Vector3.MoveTowards(actors[i].transform.localScale,new Vector3(f,Mathf.Abs(f)),Time.unscaledDeltaTime*expand);
                Vector3 pos=actors[i].transform.position;
                pos.x=Mathf.MoveTowards(pos.x,speech.characters[i].position*Screen.width,speed*Screen.width*Time.unscaledDeltaTime);
                actors[i].transform.position=pos;
            }
        }
    }
    void Speech(Speech s){
        speech=s;
        fulltext=Language.GetDialog(s.text);
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
