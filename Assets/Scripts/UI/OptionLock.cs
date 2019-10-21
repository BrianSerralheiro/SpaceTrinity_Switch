using UnityEngine;
using UnityEngine.UI;
public class OptionLock : MonoBehaviour
{
	
	[SerializeField]
	private int skinId;
    [SerializeField]
    private Color color=Color.black;
	void OnEnable () {
		Graphic g=GetComponent<Graphic>();
		if(g)
		{
			g.raycastTarget=!Locks.Skin(skinId);
            g.color=g.raycastTarget?Color.white:color;
		}
		else Debug.LogError("OptionLock needs an UI Graphic to work");
	}
}
