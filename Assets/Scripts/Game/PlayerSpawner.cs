using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] ships;
    void Awake()
    {
        Instantiate(ships[Ship.playerID]);
        Destroy(this);
    }
}
