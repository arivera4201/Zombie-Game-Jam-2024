using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    //Necessary to handle where the zombie spawns at before leaving through barrier
    [SerializeField] public Transform spawnPoint;
    void Start()
    {
        gameObject.tag = "Spawner";
    }
}
