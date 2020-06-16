using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameOverController : MonoBehaviour 
{
	[SerializeField]
	private Image[] pilots;
	[SerializeField]
	private CanvasGroup group;
	[SerializeField]
	private Text[] Scores;
	[SerializeField]
	private Text[] Stars;
	private static float Timer;
	private static GameObject menu;
	private bool highscore;
	public delegate void Dele();
	private static Dele enable;
	private string fullText;
	private float charCount;
	private Color color=Color.white;
	void Start () 
	{
		enable=Enable;
		menu = gameObject;
		gameObject.SetActive(false);
	}
	void Update ()
	{
		if(Timer>0)
		{
			Timer-=Time.unscaledDeltaTime;
			group.alpha=(2f-Timer)/2f;
		}
	}
	void Enable () 
	{
		Scores[0].text = EnemySpawner.points[0].ToString();
		if(PlayerInput.Conected(1))Scores[1].text = EnemySpawner.points[1].ToString();
		int cashStars = EnemySpawner.points[0] / 400;
		EnemySpawner.points[0]=0;
		Stars[0].text = cashStars.ToString();
		Cash.totalCash += cashStars;
		cashStars = EnemySpawner.points[1] / 400;
		EnemySpawner.points[1]=0;
		Stars[1].text = cashStars.ToString();
		Cash.totalCash += cashStars;
		pilots[0].sprite=PilotInfo.pilot[0].sad;
		pilots[1].sprite=PilotInfo.pilot[1].sad;
		pilots[1].gameObject.SetActive(PlayerInput.Conected(1));
		Cash.Save();
	}
	public void Quit()
	{
		Ship.paused = false;
		#if UNITY_EDITOR
			EditorApplication.isPlaying=false;
		#endif
		Application.Quit();
	}
	public static void Open(Ship s)
	{
		if(Ship.continues[0]>-1 || Ship.continues[1]>-1){
			Ship.shipToRevive = s;
			Ship.pointsToRevive=EnemySpawner.points[s.input.id==0?1:0]+1000;
		}else{
			Ship.paused = true;
			enable();
			menu.SetActive(true);
			Timer = 2f;
		}
	}
	public void Retry()
	{
		Ship.paused = false;
        PlayerInput.Unload();
		Loader.Scene("cen");
	}
	public void Back()
	{
		SoundManager.PlayEffects(0);
		Ship.paused = false;
		Loader.Scene("MenuSelection");
	}
}
