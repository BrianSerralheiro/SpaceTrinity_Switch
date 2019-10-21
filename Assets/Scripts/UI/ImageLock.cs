using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLock : MonoBehaviour {

	[SerializeField]
	private int skinId;
	[SerializeField]
	private Color color=Color.black;
	[SerializeField]
	private bool unlock;
	void Start () {
		
	}
	void OnEnable()
	{
		Image i = GetComponent<Image>();
		if(i)
		{
			i.color=Locks.Skin(skinId)==unlock?Color.white:color;
		}
		else Debug.LogError("ImageLock needs an image or a button to work");
	}
}
