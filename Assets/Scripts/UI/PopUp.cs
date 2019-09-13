using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopUp : MonoBehaviour {
	[SerializeField]
	private Button ok;
	[SerializeField]
	private Text text;

	public void Close() {
		gameObject.SetActive(false);
	}
	
	public void Open (string s, UnityAction action) {
		gameObject.SetActive(true);
		ok.onClick.RemoveAllListeners();
		ok.onClick.AddListener(action);
		text.text=s;
	}
}
