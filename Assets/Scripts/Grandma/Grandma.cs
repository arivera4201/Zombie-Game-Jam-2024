using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Grandma : MonoBehaviour
{
    // Grandma Positions
    public Transform grandmaBedroomPos;
    public Transform[] grandmaPossibleLocations;
    public AudioClip[] grandmaLocationAudios;

    public AudioClip[] grandmaChaseLines;

    private Player player;
    private PlayerController playerController;
    private PlayerCamera playerCamera;
    private NavMeshAgent navAgent;
    private float originalPlayerSpeed;
    private float originalPlayerSensitivity;
    private float distanceToPlayer;
    private int grandmaCurrentLocation;
    private AudioSource grandmaAudio;
    private AudioSource lobotomyAudio;
    private AudioSource chaseAudio;
    private Animator animator;
    bool reachedLocation = false;
    public bool isActive = false;
    public bool start = false;
    bool isChasing = false;

    public AudioSource snore;
    public AudioSource witch;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
        playerController = player.GetComponent<PlayerController>();
        playerCamera = player.GetComponent<PlayerCamera>();
        navAgent = GetComponent<NavMeshAgent>();
        grandmaAudio = audioSources[0];
        lobotomyAudio = audioSources[1];
        chaseAudio = audioSources[2];
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            ChasePlayer();
        }

        else if (isActive)
        {
            StartRoutine();
        }

        float distanceToBedroom = Vector3.Distance(
            grandmaBedroomPos.position,
            transform.position
        );

        if (distanceToBedroom < 1) {
            witch.volume = 0;
            snore.volume = 0.3f;
        } else {
            witch.volume = 0.3f;
            snore.volume = 0f;
        }
    }

    void StartRoutine() {
        if (grandmaCurrentLocation == 0)
            grandmaCurrentLocation = Random.Range(0, grandmaPossibleLocations.Length);
        
        // Get Distance To Grandma's Next Routine
        float distanceToLocation = Vector3.Distance(
            grandmaPossibleLocations[grandmaCurrentLocation].position,
            transform.position
        );

        if (distanceToLocation < 1)
        {
            Debug.Log($"Grandma Chasing Player: {grandmaCurrentLocation}, Distance To Location: {distanceToLocation}, reachedLcoation: {reachedLocation}");
            grandmaAudio.clip = grandmaLocationAudios[grandmaCurrentLocation];

            if (!grandmaAudio.isPlaying)
                grandmaAudio.Play();

            LookForPlayer();
        }
        else
        {
            Debug.Log($"Grandma Going To Location At Position: {grandmaCurrentLocation}");
            navAgent.SetDestination(grandmaPossibleLocations[grandmaCurrentLocation].position);
        }
    }

    void ChasePlayer()
    {
        if (!isChasing) {
            isChasing = true;
            chaseAudio.clip = grandmaChaseLines[Random.Range(0, grandmaChaseLines.Length)];
            chaseAudio.Play();
        }

        grandmaAudio.Stop();
        navAgent.SetDestination(player.transform.position);
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer);
        if (distanceToPlayer < 2)
        {
            Debug.Log("I AM LOBOTOMIZING YOU");
            reachedLocation = false;
            LobotomizePlayer();
            DeactivateGrandma();
            start = false;
        }
    }

    void LookForPlayer()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 rayDirection = (player.transform.position - transform.position).normalized;

        Debug.DrawRay(origin, rayDirection * 1000, Color.red);

        if (Physics.Raycast(origin, rayDirection, out hit, 1000))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("I FOUND YOU GRANDSON");
                start = true;
            }
            else
            {
                //Debug.Log("WHERE ARE YOU.");
            }
        }
    }

    void LobotomizePlayer()
    {
        player.brainrot = 0f;
        lobotomyAudio.Play();
        animator.Play("Lobotomize");
        navAgent.SetDestination(transform.position);
        originalPlayerSpeed = playerController.speed;
        originalPlayerSensitivity = playerCamera.sensitivity;
        playerController.speed /= 4.0f;
        playerCamera.sensitivity /= 4.0f;
        Invoke("ResetMovement", 2.0f);
        isChasing = false;
        reachedLocation = false;
    }

    void ResetMovement()
    {
        playerController.speed = originalPlayerSpeed;
        playerCamera.sensitivity = originalPlayerSensitivity;
    }

    void DeactivateGrandma()
    {
        Zombie[] zombies = GameObject.FindObjectsOfType<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            zombie.Death();
        }

        isActive = false;
        navAgent.SetDestination(grandmaBedroomPos.position);
        grandmaCurrentLocation = 0;
        reachedLocation = false;
    }

    public void ActivateGrandma()
    {
        isActive = true;
    }
}
