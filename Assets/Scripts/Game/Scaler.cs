using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour {
	[SerializeField]
	private Camera cam;
	public static float sizeY;
	public static float sizeX;
	public static float x;
	void Start () {
		float f=(float)Screen.width/(float)Screen.height;
		sizeY=cam.orthographicSize;
		sizeX=f*sizeY*2f;
		transform.localScale=new Vector3(f*sizeY*2f,sizeY*2f);
		x=f/2f*sizeY*2f;
		//cam.transform.position=transform.position=new Vector3(f/2f*size*2f,size,5);
		//cam.transform.Translate(0,0,-10);
	}
}
