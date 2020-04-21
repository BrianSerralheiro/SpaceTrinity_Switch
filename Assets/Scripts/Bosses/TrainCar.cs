using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TrainCar:MonoBehaviour {
    public Train train;
    void Awake()
    {
        Rigidbody2D rigidbody=GetComponent<Rigidbody2D>();
        rigidbody.useFullKinematicContacts=rigidbody.isKinematic=true;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        train.OnCollisionEnter2D(col);
    }
}