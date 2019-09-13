using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossWarning : MonoBehaviour
{
	private static float timer;
	private RawImage image;
	private RectTransform rect;
	private RectTransform rect2;
	private Color color=Color.white;
	private static GameObject go;
	private void Start()
	{
		rect=GetComponent<RectTransform>();
		rect2=rect.GetChild(0) as RectTransform;
		image=rect2.GetComponent<RawImage>();
		go=gameObject;
		go.SetActive(false);
	}
	public static void Show(){
		SoundManager.PlayEffects(20);
		go.SetActive(true);
		timer=6;
	}
	private void Update()
	{
		timer-=Time.deltaTime;
		color.a=1f-Mathf.PingPong(timer,1f);
		image.color=color;
		rect.localScale=Vector3.right*(6f-timer)+Vector3.up;
		rect2.localScale=Vector3.right/rect.localScale.x+Vector3.up;
		if(timer<0)go.SetActive(false);
	}
}