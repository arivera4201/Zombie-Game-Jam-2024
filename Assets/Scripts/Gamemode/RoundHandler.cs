using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [SerializeField] Transform player;
    public GameObject[] zombieObject;
    public int round = 0;
    private int zombiesToSpawn;
    private int zombiesSpawned;
    private int spawnCap = 24;
    private AudioSource roundStartNoise;

    //Initiates first round
    void Start()
    {
        roundStartNoise = GetComponent<AudioSource>();
        //RoundStart();
    }

    //Starts next round, determines number of zombies to spawn
    public void RoundStart()
    {
        roundStartNoise.Play();
        round++;
        GrandmaCheck();
        Debug.Log("Round " + round);
        zombiesToSpawn = (int)(5 + round * 1.5f);
        Invoke("RoundLoop", 7f);
    }

    //Loops spawning in zombies until there are no more zombies left to spawn or the cap is reached
    void RoundLoop()
    {
        if (zombiesToSpawn > 0 && zombiesSpawned < spawnCap)
        {
            SpawnZombie();
            Invoke("RoundLoop", 1f);
        }
    }

    //Spawns in a zombie, keeps track of zombies
    void SpawnZombie()
    {
        if (zombiesToSpawn > 0)
        {
            int selectedZombieObjectIndex = Random.Range(0, zombieObject.Length);
            GameObject selectedZombieObject = zombieObject[selectedZombieObjectIndex];
            Instantiate(selectedZombieObject);
            zombiesToSpawn--;
            zombiesSpawned++;
        }
    }

    //When a zombie dies, decreases the count and spawns a new one or begins the next round
    public void ZombieDeath()
    {
        zombiesSpawned--;
        if (zombiesSpawned <= 0)
        {
            Invoke("RoundStart", 3f);
        }
        else
        {
            SpawnZombie();
        }
    }

    void GrandmaCheck()
    {
        if (round % 4 == 0)
        {
            Grandma grandma = FindObjectOfType<Grandma>();
            grandma.ActivateGrandma();
        }
    }
}
