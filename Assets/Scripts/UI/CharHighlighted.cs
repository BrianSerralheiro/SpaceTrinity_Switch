using UnityEngine;
using UnityEngine.UI;

public class CharHighlighted : MonoBehaviour
{
    [SerializeField]
    private Graphic selector;
    [SerializeField]
    float glowSpeed=2;
    private Graphic graphic;

    private RectTransform rect;

    void Start()
    {
        rect=GetComponent<RectTransform>();   
        graphic=GetComponent<Graphic>();     
    }

    void Update()
    {
        rect.anchoredPosition=selector.rectTransform.anchoredPosition;
		rect.anchorMin=selector.rectTransform.anchorMin;
		rect.anchorMax=selector.rectTransform.anchorMax;
        rect.rotation=selector.rectTransform.rotation;
        Color color= graphic.color;
        color.a=Mathf.PingPong(Time.time,glowSpeed)/glowSpeed;
        graphic.color=color;
    }
}
