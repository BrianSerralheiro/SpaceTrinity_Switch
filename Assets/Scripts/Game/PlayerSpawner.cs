﻿using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] ships;
    void Awake()
    {
        EnemyBase.players=new Transform[2];
        if(!PlayerInput.Conected(1)){
            Instantiate(ships[Ship.player1]);
            Ship.player2=-1;
        }
        else{
            Instantiate(ships[Ship.player1]).transform.position=Vector3.left*3;
            Instantiate(ships[Ship.player2]).transform.position=Vector3.right*3;
        }
        Destroy(this);
    }
}
