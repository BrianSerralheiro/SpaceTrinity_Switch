using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopUp : MonoBehaviour {
	[SerializeField]
	private Text text;
	[SerializeField]
	string confirmKey,cancelKey;
	UnityAction action,cancel;
	MenuSelect menu;

	public void Close() {
		gameObject.SetActive(false);
		menu.enabled=true;
	}
	void Update()
	{
		if(PlayerInput.GetButtomDown(cancelKey)){
			cancel?.Invoke();
			Close();
		}
		if(PlayerInput.GetButtomDown(confirmKey)){
			action?.Invoke();
			Close();
		}
	}
	public void Open (string s, UnityAction act,UnityAction can,MenuSelect m) {
		gameObject.SetActive(true);
		action=act;
		cancel=can;
		menu=m;
		menu.enabled=false;
		text.text=s;
	}
}
