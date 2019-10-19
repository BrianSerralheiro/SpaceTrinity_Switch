using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopUp : MonoBehaviour {
	[SerializeField]
	private Text text;
	[SerializeField]
	KeyCode confirmKey;
	[SerializeField]
	KeyCode cancelKey;
	UnityAction action;
	MenuSelect menu;

	public void Close() {
		gameObject.SetActive(false);
		menu.enabled=true;
	}
	void Update()
	{
		if(Input.GetKeyDown(cancelKey))Close();
		if(Input.GetKeyDown(confirmKey)){
			action?.Invoke();
			Close();
		}
	}
	public void Open (string s, UnityAction act,MenuSelect m) {
		gameObject.SetActive(true);
		action=act;
		menu=m;
		menu.enabled=false;
		text.text=s;
	}
}
