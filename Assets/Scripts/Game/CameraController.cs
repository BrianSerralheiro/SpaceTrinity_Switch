using UnityEngine;

public class CameraController : MonoBehaviour
{
    delegate void Del();
    Del update;
    void Start()
    {
        if(Ship.player2==-1)update=Single;
        else update=Double;
        GetComponent<Camera>().depthTextureMode=DepthTextureMode.Depth;
        Light l=gameObject.GetComponentInChildren<Light>();
        l.intensity=EnemySpawner.world.lightIntensity;
        l.color=EnemySpawner.world.lightColor;
    }
    void Single(){
        Vector3 v=transform.position;
        v.x=Mathf.MoveTowards(v.x,EnemyBase.players[0].position.x/(Scaler.sizeX/4),Time.deltaTime);
        transform.position=v;
    }
    void Double(){
        Vector3 v=transform.position;
        v.x=Mathf.MoveTowards(v.x,(EnemyBase.players[0].position.x-EnemyBase.players[1].position.x)/2/(Scaler.sizeX/4),Time.deltaTime);
        transform.position=v;
    }
    void Update()
    {
        update?.Invoke();
    }
}
