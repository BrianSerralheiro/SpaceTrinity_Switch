using UnityEngine;

public class TurretHole : MonoBehaviour
{
    void Update()
    {
        transform.Translate(0,-Time.deltaTime*2.5f,0,Space.World);
        if(transform.position.y<-Scaler.sizeY-2)Destroy(gameObject);
    }
}
