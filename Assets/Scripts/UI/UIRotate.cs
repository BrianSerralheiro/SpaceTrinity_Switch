using UnityEngine;

public class UIRotate : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float rotationSpeed;
    float delta;
    void OnEnable()
    {
        delta=rotationSpeed-minSpeed;
    }
    void Update()
    {
        transform.Rotate(0,0,(minSpeed+ Mathf.PingPong(Time.time, rotationSpeed)*delta) * 360 * Time.deltaTime);
    }
}
