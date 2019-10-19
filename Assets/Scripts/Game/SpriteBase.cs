using UnityEngine;

public class SpriteBase : MonoBehaviour {
	public static SpriteBase I;
	public Sprite[] bullets;
	public Sprite[] item;
	public Sprite[] drone;
	public Sprite[] screens;
	public Material shock;
	void Awake () {
		I=this;
		Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
