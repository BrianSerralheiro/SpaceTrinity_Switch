using UnityEngine;
using UnityEngine.UI;

public class chibiChanger : MonoBehaviour
{
    private float time;
    private Image image;

    private Transform jumpy;
    private Vector3 initialP;
    private float deltaP;
    [SerializeField]
    private Sprite chibiHappy,chibiIdle;

    private float Speed;

    void Start()
    {
        Speed = Random.Range(3,4);
        time = Time.time + Random.Range(0.5f , 1.2f);
        image = GetComponent<Image>();
        image.sprite = chibiIdle;
        initialP = transform.position;
        deltaP = Random.Range(Screen.height/ 10, Screen.height/3);
    }

    void Update()
    {
        float f = Mathf.Abs(Mathf.Sin(Time.time * Speed));
        image.sprite = f > 0.5f ? chibiHappy:chibiIdle;
        if(f < 0.1f)
        {
            deltaP = Random.Range(Screen.height/ 10, Screen.height/3);
        }
        transform.position = initialP + Vector3.up * deltaP * f;
    }
}
