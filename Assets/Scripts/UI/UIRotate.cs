using UnityEngine;

public class UIRotate : MonoBehaviour
{
    [SerializeField]
    private float mindSpeed;
    [SerializeField]
    private float rotationSpeed;
    float delta;
    void OnEnable()
    {
        delta=rotationSpeed-mindSpeed;
    }
    void Update()
    {
        transform.Rotate(0,0,(mindSpeed+ Mathf.PingPong(Time.time, rotationSpeed)*delta) * 360 * Time.deltaTime);
    }
}
