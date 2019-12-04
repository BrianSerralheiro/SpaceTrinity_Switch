using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] ships;
    void Awake()
    {
        if(!PlayerInput.Conected(1))Instantiate(ships[Ship.player1]);
        else{
            Instantiate(ships[Ship.player1]).transform.position=Vector3.left*3;
            Instantiate(ships[Ship.player2]).transform.position=Vector3.right*3;
        }
        Destroy(this);
    }
}
