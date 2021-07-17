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
    }
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name.Contains("Player") || col.name.Contains("item") || col.isTrigger)return;
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        explosion.transform.position=transform.position;
        Instantiate(explosion).gameObject.SetActive(true);
        // explosion.gameObject.SetActive(true);
    }
}
