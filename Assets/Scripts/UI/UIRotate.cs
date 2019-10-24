using UnityEngine;

public class UIRotate : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Mathf.PingPong(Time.time, rotationSpeed) * 360 * Time.deltaTime);
    }
}
