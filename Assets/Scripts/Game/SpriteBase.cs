using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBase : MonoBehaviour {
	public static SpriteBase I;
	public Sprite bullet;
	public Sprite[] bullets;
	public Sprite[] shooter;
	public Sprite[] diver;
	public Sprite[] carrier;
	public Sprite[] round;
	public Sprite[] Lasor;
	public Sprite[] grabber;
	public Sprite[] launcher;
	public Sprite[] bat;
	public Sprite[] batgirl;
	public Sprite[] header;
	public Sprite[] invader;
	public Sprite[] slasher;
	public Sprite[] bomber;
	public Sprite[] zapper;
	public Sprite[] item;
	public Sprite[] drone;
	public Sprite[] boss1;
	public Sprite[] boss2;
	public Sprite[] boss3;
	public Sprite[] boss4;
	public Sprite[] screens;
	public Sprite[] MFBat;
	public Sprite shield;
	public Material shock;
	public GameObject explosion;
	void Awake () {
		I=this;
		Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
