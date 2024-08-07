using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    // Adjustable Variables
    public float damage = 40f;
    public float range = 2f;
    public float fatness = 2f;
    public Vector3 knockBackDeath = new Vector3(10, 0, 0);


    private ZombieSpawn[] spawners;
    private ZombieSpawn selectedSpawner;
    private NavMeshAgent navAgent;
    private Transform target;
    private Player player;
    private RoundHandler roundHandler;
    private float health = 150.0f;
    private float distanceToPlayer;
    private bool canHit = true;
    private bool outsideMap = true;
    private Animator animator;
    private bool isDead;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    private float impulseMultiplier = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        roundHandler = FindObjectOfType<RoundHandler>();
        spawners = FindObjectsOfType<ZombieSpawn>();
        selectedSpawner = spawners[Random.Range(0, spawners.Length)];
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
        target = player.transform;
        navAgent.Warp(selectedSpawner.spawnPoint.position);
        SetHealth();
    }

    //If the zombie is alive, either chase the player or enter the map through a barricade
    void Update()
    {
        if (!isDead)
        {
            if (outsideMap) OutsideMap();
            else ChasePlayer();
        }
    }

    //Decreases zombie's health when damage is taken
    public void TakeDamage(float damage)
    {
        health = health - damage;
        if (health <= 0 && !isDead)
        {
            player.score += 100;
            Death();
        } 
        else if (isDead)
        {
            rb.AddForce(player.transform.GetChild(0).TransformDirection(Vector3.forward) * (player.weapon.damage * impulseMultiplier), ForceMode.Impulse);
        }
        else player.score += 10;
    }

    //Chase the player up until distance is 1.7, once distance is 1.7, attack player
    void ChasePlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToPlayer > fatness)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(target.position);
        }
        else
        {
            navAgent.isStopped = true;
            Attack();
        }
    }

    //Enter the map through the barricade, start and end animation for climbing
    void OutsideMap()
    {
        navAgent.SetDestination(selectedSpawner.transform.position);
        if (Vector3.Distance(transform.position, selectedSpawner.transform.position) <= 2.5f)
        {
            animator.SetBool("isClimbing", true);
        }
        if (Vector3.Distance(transform.position, selectedSpawner.transform.position) <= 0.5f)
        {
            animator.SetBool("isClimbing", false);
            outsideMap = false;
        }
    }

    //Attack the player by casting a ray in their direction, if hit, apply damage
    void Attack()
    {
        if (canHit)
        {
            animator.Play("Attack");
            canHit = false;
            Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
            RaycastHit hit;

            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360f);

            if (Physics.Raycast(transform.position + offset, transform.TransformDirection(Vector3.forward), out hit, range))
            {
                Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.tag == "Player")
                {
                    Player player = hit.collider.gameObject.GetComponent<Player>();
                    player.TakeDamage(damage);
                }
            }
            else
            {
                Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward), Color.green);
            }
            Invoke("ResetHit", 1f);
        }
    }

    //Resets the zombie's ability to hit the player
    void ResetHit()
    {
        canHit = true;
    }

    //Upon death, play death animation and signal the round handler, then remove from game
    public void Death()
    {
        isDead = true;
        navAgent.isStopped = true;
        rb.isKinematic = false;
        //animator.enabled = false;
        navAgent.enabled = false;        
        animator.SetBool("isDead", true);
        rb.AddForce(player.transform.GetChild(0).TransformDirection(Vector3.forward) * (player.weapon.damage * impulseMultiplier), ForceMode.Impulse);
        //rb.AddForce(knockBackDeath, ForceMode.Impulse);
        roundHandler.ZombieDeath();
        //Object.Destroy(GetComponent<CapsuleCollider>());
        Invoke("Destroy", 3f);
    }

    //Destroy the zombie after it is killed
    void Destroy()
    {
        Object.Destroy(this.gameObject);
    }

    void SetHealth()
    {
        int round = roundHandler.round;
        if (round < 10)
        {
            health = health + 100.0f;
        }
        else
        {
            health *= 1.1f;
        }
    }
}
