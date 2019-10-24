using UnityEngine;
using UnityEngine.UI;

public class GlowLightUI : MonoBehaviour
{
    [SerializeField]
    float cicleTime;
    [SerializeField]
    float offTime;
    float amount;
    [SerializeField]
    [ColorUsageAttribute(true, true)]
    Color color1;
    [SerializeField]
    [ColorUsageAttribute(true, true)]
    Color color2;
    private Graphic graphic;
    void Start()
    {
        graphic=GetComponent<Graphic>();
        if(!graphic)Destroy(this);
    }

    void Update()
    {
        graphic.color=Color.Lerp(color1,color2,Time.time%cicleTime-offTime);
    }
}
