using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class timetest : MonoBehaviour {
	private UnityWebRequest myHttpWebRequest;
	void Start () {
		myHttpWebRequest = UnityWebRequest.Get("https://www.worldtimeserver.com/current_time_in_BR-RJ.aspx");
		myHttpWebRequest.Send();

	}
	
	void Update () {

		string netTime = myHttpWebRequest.GetResponseHeader("date");
		Debug.Log(netTime);
	}
}
