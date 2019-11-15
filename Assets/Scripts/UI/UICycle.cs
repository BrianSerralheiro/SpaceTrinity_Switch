using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICycle : MonoBehaviour
{
    [SerializeField]
    private Color color1, color2;

    [SerializeField]
    private float cycleTime;

    [SerializeField]
    private float OnTimer;

    [SerializeField] [Range(0.1f, 1)]
    private float Delay;

    private Graphic graphic;

    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        float f = Time.time%cycleTime;
        graphic.color=Color.Lerp(color1,color2,Mathf.Min(f-OnTimer+Delay,OnTimer+Delay - f)/Delay);
    }
}
