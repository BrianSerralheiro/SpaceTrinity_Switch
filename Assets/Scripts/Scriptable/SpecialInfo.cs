using UnityEngine;
[System.Serializable]
public struct SpecialInfo
{
    public Vector2 area,offSet;
    [SerializeField]
    private bool followShip;
    [Range(0.1f,1)]
    public float cost;
    public float damageInterval,fps,speed,rotationAround,rotationSelf,duration;
    public int damage;
    private float spriteId;
    private Transform transform;
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
        bu.bulleSpeed=0;
        bu.owner=t.name;
        bu.Timer(10);
        // renderer=gameObject.AddComponent<SpriteRenderer>();
        // renderer.sprite=sprites[0];
        collider=gameObject.GetComponent<Collider2D>();
        transform=gameObject.transform;
        if(followShip){
            transform.parent=t;
            transform.localPosition=new Vector3(offSet.x,offSet.y);
        }
        else {
            transform.parent=null;
            transform.position=t.position+(Vector3)offSet;
        }
        transform.localScale=new Vector3(area.x,area.y,1);
    }
    public void  Update(){
        if(rotationAround!=0){
            Vector3 v=transform.localPosition;
            v=Quaternion.Euler(0,0,rotationAround*Time.deltaTime)*v;
            transform.localPosition=v;
        }
        if(rotationSelf!=0)transform.Rotate(0,0,rotationSelf*Time.deltaTime,Space.Self);
        if(speed!=0)transform.Translate(Vector3.up*speed*Time.deltaTime,Space.World);
        if(fps>0)spriteId+=Time.deltaTime*fps;
        if(damageInterval>0)collider.enabled=Time.time%damageInterval>damageInterval/2;
    }
    public void Finish(){
        // GameObject.Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
