using UnityEngine;

public class MissileSpecial : MonoBehaviour
{
    [SerializeField]
    private Bullet explosion;
    void Start()
    {
        Bullet bullet=GetComponent<Bullet>();
        explosion.damage=bullet.damage;
        explosion.owner=bullet.owner;
        explosion.transform.parent=null;
        bullet.transform.localScale=transform.localScale;
    }
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name.Contains("Player") || col.name.Contains("item"))return;
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        explosion.transform.position=transform.position;
        explosion.gameObject.SetActive(true);
    }
}
