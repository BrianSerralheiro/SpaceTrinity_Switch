using UnityEngine;

public class TurretHole : MonoBehaviour
{
    void Start()
    {
        transform.Translate(0,0,0.5f,Space.World);
    }
    void Update()
    {
        if(Ship.paused)return;
        transform.Translate(0,-Time.deltaTime*5f,0,Space.World);
        if(transform.position.y<-Scaler.sizeY-2)Destroy(gameObject);
    }
}
