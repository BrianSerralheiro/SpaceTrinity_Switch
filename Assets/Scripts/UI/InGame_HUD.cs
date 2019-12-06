using UnityEngine;
using UnityEngine.UI;

public class InGame_HUD : MonoBehaviour 
{
	[SerializeField]
	private bool p1;
	private int id;
	private float currentHp,_shipHealth,_special;
	[SerializeField]
	private Text scoreHUD,pilotName;
	private static float[] _currentHP={1,1};
	public static float[] shipHealth={1,1},special={0,0};

	[SerializeField]
	private Image lifeBar,lifeFill,specialFill,pilotPic, pilotMask;

	private Color color;
	private Color otherColor=Color.white;
	public static HUDInfo[] HUD=new HUDInfo[2];
	void Start()
	{
		if(!p1 && !PlayerInput.Conected(1))gameObject.SetActive(false);
		id=p1?0:1;
		lifeBar.sprite=HUD[id].lifeBar;
		lifeFill.sprite=HUD[id].lifeFill;
		pilotName.text=HUD[id].name;
		pilotPic.sprite=HUD[id].picture;
		specialFill.color=pilotMask.color=HUD[id].color;
		color=specialFill.color;
		lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
	}
	
	void Update()
	{
		if(shipHealth[id]<_currentHP[id]){
			_currentHP[id]-=Time.deltaTime;
			if(shipHealth[id]>_currentHP[id])_currentHP[id]=shipHealth[id];
			lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
		}
		if(shipHealth[id]>_currentHP[id]){
			_currentHP[id]+=Time.deltaTime;
			if(shipHealth[id]<_currentHP[id])_currentHP[id]=shipHealth[id];
			lifeFill.material.SetFloat("_CurrentHP",_currentHP[id]);
		}
		scoreHUD.text = EnemySpawner.points[id].ToString();
		if(special[id] >=1)
		{
			special[id] = 1;
			specialFill.color=Color.Lerp(color,otherColor,Mathf.Cos(Time.time*8));
		}else specialFill.color=color;
		specialFill.fillAmount=special[id];
	}
}
