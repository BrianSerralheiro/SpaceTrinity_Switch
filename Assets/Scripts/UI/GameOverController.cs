using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour 
{
	[SerializeField]
	int id;
	[SerializeField]
	private Image panel;
	[SerializeField]
	private Image image;
	[SerializeField]
	private Text gameover;
	[SerializeField]
	private Text LevelCleared;
	[SerializeField]
	private Text Score;
	[SerializeField]
	private Text Stars;
	[SerializeField]
	private Continue cont;
	public Sprite[] panels;
	[SerializeField]
	private Graphic[] graphics;
	private static float Timer;

	private static Ship ship;

	private static GameObject menu;

	private bool highscore;
	public delegate void Dele();
	private static Dele enable;
	[SerializeField]
	private Text gameoverTEXT;
	
	[SerializeField]
	private GameObject noContinues;
	[SerializeField]
	private Image gameoverDialog;

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
			Timer-=Time.deltaTime;
			color.a=(2f-Timer)/2f;
			PilotInfo.pilot[ship.input.id].color.a=color.a;
			foreach(Graphic g in graphics)
			{
				g.color=color;
			}
			gameover.color=PilotInfo.pilot[ship.input.id].color;
			if(Timer<=0)gameoverDialog.gameObject.SetActive(true);
		}
		else {
			if(charCount<fullText.Length)
			{
				charCount+=Time.deltaTime * 14;
				if(gameoverTEXT.text.Length<Mathf.FloorToInt(charCount))gameoverTEXT.text=fullText.Substring(0,Mathf.FloorToInt(charCount));
			}
			if(ship.input.GetKeyDown("shoot"))RevivePopUp();
			if(ship.input.GetKeyDown("special"))QuitGame();
		}

	}
	
	void Enable () 
	{
		if(PlayerPrefs.GetInt("highscore") <= EnemySpawner.points[ship.input.id])
		{
			highscore = true;
			PlayerPrefs.SetInt("highscore", EnemySpawner.points[ship.input.id]);
		}
		else
		{
			highscore = false;
		}
		LevelCleared.text = (Transition.worldID + 1).ToString();
		charCount = 0;
		gameoverTEXT.text = "";
		fullText = highscore ? DialogBox.GetText(5):DialogBox.GetText(6);
		panel.sprite=panels[ship.ID()];
		image.sprite=highscore ? PilotInfo.pilot[ship.input.id].happy:PilotInfo.pilot[ship.input.id].normal;
		Score.text = EnemySpawner.points.ToString();
		int cashStars = EnemySpawner.points[id] / 400;
		EnemySpawner.points[id]=0;
		Stars.text = cashStars.ToString();
		Cash.totalCash += cashStars;
		Cash.Save();
		gameoverDialog.sprite = DialogBox.getBox();
		gameoverTEXT.fontSize = Mathf.CeilToInt(Screen.height/10/DialogBox.getSize(highscore?5:6));
	}

	public void Close()
	{
		if(charCount>=fullText.Length)
		{
			gameoverDialog.gameObject.SetActive(false);
		}
		else
		{
			charCount=fullText.Length;
			gameoverTEXT.text=fullText.Substring(0,Mathf.FloorToInt(charCount));
		}
	}
	
	public static void Open(Ship s)
	{
		if(Ship.continues[0]>0 || Ship.continues[1]>0)return;
		Ship.paused = true;
		ship = s;
		enable();
		menu.SetActive(true);
		Timer = 2f;
	}

	public void RevivePopUp()
	{
		cont.ship=ship;
		if(cont.HasContinue()){
			SoundManager.PlayEffects(0);
			cont.Active=gameObject.SetActive;
			cont.gameObject.SetActive(true);
		}
		else
		{
			SoundManager.PlayEffects(11);
			noContinues.SetActive(true);
		}
	}
	public void QuitGame()
	{
		SoundManager.PlayEffects(0);
		Ship.paused = false;
		Loader.Scene("SelectionTest");
	}
}
