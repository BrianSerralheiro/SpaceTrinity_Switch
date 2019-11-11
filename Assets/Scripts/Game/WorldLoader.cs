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
    [SerializeField]
    private HUDInfo[] HUDs;
    [SerializeField]
    private PilotInfo[] pilots;
	private ResourceRequest request;
    void Start()
    {
        update=Step1;
        InGame_HUD.HUD=HUDs[Ship.playerID];
        PilotInfo.pilot=pilots[Ship.playerID];
    }
    void Update()
    {
        update?.Invoke();
        text.text = "Loading";
        for (int i = 0; i < Mathf.CeilToInt(Time.time * 3 % 3); i++)
        {
            text.text += ".";
        }
    }
    void Step1(){
        if(request==null){
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
