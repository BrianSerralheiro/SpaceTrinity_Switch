using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour 
{
	[SerializeField]
	private Text Stage;
	public static float Timer;
	[SerializeField]
	private Color textColor;
	private Color Transparent;
	public static int worldID;
	
	void Start () 
	{
		Transparent = new Color(0,0,0,0);
	}
	
	void Update () 
	{
		if(Timer > 0)
		{
			Timer -= Time.deltaTime;
			if(worldID == 12)
			{
				Stage.text = "Final Stage";
			}
			else
			{
				Stage.text = "World "+ (worldID / 3 + 1) + " Stage " + (worldID % 3 + 1);
			}
			Stage.color = Color.Lerp(Transparent, textColor, Mathf.PingPong(Timer,1));
		}
	}
}
