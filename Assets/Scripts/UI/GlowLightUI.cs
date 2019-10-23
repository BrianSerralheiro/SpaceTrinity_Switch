using UnityEngine;
using UnityEngine.UI;

public class GlowLightUI : MonoBehaviour
{
    [SerializeField]
    float cicleTime;
    [SerializeField]
    float offTime;
    [SerializeField]
    float timer;
    Color color;
    private Graphic graphic;
    void Start()
    {
        graphic=GetComponent<Graphic>();
        if(!graphic)Destroy(this);
        timer =Time.time;
        color=graphic.color;
    }

    void Update()
    {
        float d=Time.deltaTime;
        if(timer<offTime)color.a-=d;
        else color.a+=d;
        color.a=Mathf.Clamp01(color.a);
        graphic.color=color;
        timer+=d;
        if(timer>=cicleTime)timer-=cicleTime;
    }
}
