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
    private Image boss;
    [SerializeField]
    private HUDInfo[] HUDs;
    [SerializeField]
    private PilotInfo[] pilots;
    [SerializeField]
    private Skin[] skins;
    [SerializeField]
    Sprite[] webshot;
    [SerializeField]
    Sprite[] squidshot;
    [SerializeField]
    EnemyInfo batinfo;
    [SerializeField]
    ParticleSystem defaultExplosion,inpact,block,trail,pickUp;
	private ResourceRequest request;
    void Start()
    {
        update=Step1;
        Ship.continues=new int[]{-1,-1};
        InGame_HUD.HUD[0]=HUDs[Ship.player1];
        InGame_HUD.HUD[1]=HUDs[Ship.player2];
        PilotInfo.pilot[0]=pilots[Ship.player1];
        PilotInfo.pilot[1]=pilots[Ship.player2];
        if(Ship.skinID[Ship.player1]>-1)Ship.skins[0]=skins[Ship.player1 * 3 + Ship.skinID[Ship.player1]];
        if(Ship.skinID[Ship.player2]>-1)Ship.skins[1]=skins[Ship.player2 * 3 + Ship.skinID[Ship.player2]];
        PlayerInput.Unload();
        boss.sprite=EnemySpawner.world.bossFace;
    }
    void Update()
    {
        update?.Invoke();
        boss.transform.Translate(-Time.deltaTime*Screen.width/4,0,0);
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
           //colocar loading correspondente
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
            worldInfo.Boss?.Register();
            worldInfo.subBoss?.Register();
            worldInfo.drone?.Register();
        }
		Ship.webSprite=Bullet.Register(webshot[0]);
		Bullet.Register(webshot[1]);
		Ship.squidSprite=Bullet.Register(squidshot[0]);
		Bullet.Register(squidshot[1]);
		Bullet.Register(squidshot[2]);
		Bullet.Register(squidshot[3]);
        Ship.batInfo=batinfo;
        update=Step5;
    }
    void Step5(){
        WorldInfo worldInfo=EnemySpawner.world;
        ParticleManager.Clear();
        ParticleManager.Register(defaultExplosion);
        ParticleManager.Register(worldInfo.explosion);
        ParticleManager.Register(inpact);
        ParticleManager.Register(block);
        ParticleManager.Register(trail);
        ParticleManager.Register(pickUp);
        if(worldInfo){
            foreach(EnemyInfo ei in worldInfo.enemies){
                ei.Particles();
            }
            worldInfo.Boss?.Particles();
            worldInfo.subBoss?.Particles();
            worldInfo.drone?.Particles();
        }
        update=Step6;
    }
    void Step6(){
        if(boss.transform.position.x<Screen.width/3){
            SceneManager.LoadSceneAsync("cen");
            update=null;
        }
    }
    public static void BossSong(Setter s){
        bossMusic=s;
    }
    public static void BGSong(Setter s){
        BGMusic=s;
    }
}
