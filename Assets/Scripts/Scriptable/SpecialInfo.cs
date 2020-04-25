using UnityEngine;
[System.Serializable]
public struct SpecialInfo
{
    public Vector3 area,offSet;
    [SerializeField]
    private bool followShip,lockShot;
    [Range(0.1f,1)]
    public float cost;
    public float damageInterval,speed,rotationAround,rotationSelf,duration,imuneTime;
    public int damage;
    private float time;
    private Transform transform,ship;
    [SerializeField]
    private GameObject gameObject;
    private Collider2D collider;
    public void Start(Transform t) {
        if(gameObject)gameObject.SetActive(true);
        else gameObject=new GameObject("playerbullet");
        Bullet bu=gameObject.GetComponent<Bullet>();
        bu.damage=damage;
        bu.pierce=true;
        bu.enabled=false;
        bu.bulletSpeed=0;
        bu.owner=t.name;
        bu.Timer(10);
        // renderer=gameObject.AddComponent<SpriteRenderer>();
        // renderer.sprite=sprites[0];
        collider=gameObject.GetComponent<Collider2D>();
        transform=gameObject.transform;
        transform.parent=null;
        transform.position=t.position+offSet;
        transform.localScale=area;
        time=Time.time+duration;
        ship=t;
    }
    public void  Update(){
        if(followShip)transform.position=ship.position+offSet;
        if(rotationAround!=0){
            Vector3 v=transform.position-ship.position;
            v=Quaternion.Euler(0,0,rotationAround*Time.deltaTime)*v.normalized;
            transform.position=ship.position+v*offSet.y;
        }
        if(rotationSelf!=0)transform.Rotate(0,0,rotationSelf*Time.deltaTime,Space.Self);
        if(speed!=0)transform.Translate(Vector3.up*speed*Time.deltaTime,Space.World);
        if(damageInterval>0)collider.enabled=Time.time%damageInterval>damageInterval/2;
        if(time<Time.time)gameObject.SetActive(false);
    }
    public bool Finished(){
        return !gameObject.activeSelf;
    }
    public bool AllowShot(){
        return !lockShot || Finished();
    }
}
