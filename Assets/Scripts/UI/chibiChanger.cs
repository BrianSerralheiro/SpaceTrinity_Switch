using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chibiChanger : MonoBehaviour
{
    private float time;
    private Image image;

    private Transform jumpy;
    private Vector3 initialP;
    private float deltaP;

    public Sprite chibiHappy,chibiIdle;

    private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = Random.Range(3,4);
        time = Time.time + Random.Range(0.5f , 1.2f);
        image = GetComponent<Image>();
        image.sprite = chibiIdle;
        initialP = transform.position;
        deltaP = Random.Range(100, 200);
    }

    // Update is called once per frame
    void Update()
    {
        float f = Mathf.Abs(Mathf.Sin(Time.time * Speed));
        image.sprite = f > 0.5f ? chibiHappy:chibiIdle;
        if(f < 0.1f)
        {
            deltaP = Random.Range(100,200);
        }
        transform.position = initialP + Vector3.up * deltaP * f;
    }
}
