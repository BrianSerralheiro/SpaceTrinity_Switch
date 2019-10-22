using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WorldLoader : MonoBehaviour
{
    public delegate void Setter(AudioClip ac);
    private delegate void Dele();
    private static Setter bossMusic;
    private static Setter BGMusic;
    private Dele update;
    [SerializeField]
    private Text text;
	private ResourceRequest request;
    void Start()
    {
        update=Step1;
    }
    void Update()
    {
        update?.Invoke();
    }
    void Step1(){
        if(request==null){
            text.text="Loading Level Song.";
            request=Resources.LoadAsync<AudioClip>("Audio/"+ EnemySpawner.world.songName);
        }
        else if(request.isDone){
            BGMusic(request.asset as AudioClip);
            request =null;
            update=Step2;
        }
    }
    void Step2(){
        if(request==null){
            text.text="Loading Level BackGround.";
            request=Resources.LoadAsync<AudioClip>("Art/"+ EnemySpawner.world.bgs[0].name);
        }
        else if(request.isDone){
           EnemySpawner.mundo= request.asset as Texture;
            request=null;
            update=Step3;
        }
    }
    void Step3(){
        if(request==null){
            text.text="Loading Boss Song.";
            request=Resources.LoadAsync<AudioClip>("Audio/"+ EnemySpawner.world.bossSong);
        }
        else if(request.isDone){
            bossMusic(request.asset as AudioClip);
            request=null;
            update=Step4;
        }
    }
    void Step4(){
        WorldInfo worldInfo=EnemySpawner.world;
        Bullet.sprites.Clear();
        if(worldInfo){
            foreach(EnemyInfo ei in worldInfo.enemies){
                ei.Register();
            }
        }
        update=Step5;
    }
    void Step5(){
        text.text="Loading Scene";
        SceneManager.LoadSceneAsync("cen");
        update=null;
    }
    public static void BossSong(Setter s){
        bossMusic=s;
    }
    public static void BGSong(Setter s){
        BGMusic=s;
    }
}
