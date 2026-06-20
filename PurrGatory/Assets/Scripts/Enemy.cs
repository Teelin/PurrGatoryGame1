using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{

    Transform player;
    Transform target;
    NavMeshAgent agent;
    
    bool isFollowing = false;
    bool isStunned = false;
    bool hasLineOfSight = false;

    private float timer;
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;
    [SerializeField] LayerMask detectionMask;


    private void OnDisable()
    {
        PlayerController.rattleAction -= Stun;
    }

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        PlayerController.rattleAction += Stun;
        target = player.Find("KittenFollowTarget");
        timer = wanderTimer;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    // Update is called once per frame
    void Update()
    {
        hasLineOfSight = CheckLineOfSight();

        if (player != null && isFollowing && !isStunned && hasLineOfSight)
        {
            //agent.isStopped = false;
            agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
        }
        if((!isFollowing || !hasLineOfSight) && !isStunned)
        {
            Wander();
        }
    }
    
    void Wander()
    {
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle player collision logic here
            Debug.Log("Enemy collided with Player.");
            isFollowing = true;
            target = player.Find("KittenFollowTarget");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void Stun()
    {
        if (isFollowing)
        { 
            StartCoroutine(StunCoroutine(3f));
        }
    }
    IEnumerator StunCoroutine(float duration)
    {
        agent.isStopped = true;
        isStunned = true;
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
        isStunned = false;
    }

    bool CheckLineOfSight()
    {
        // Implement line of sight logic here
        var hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, Mathf.Infinity, detectionMask);
        //Debug.DrawRay(transform.position, (player.position - transform.position).normalized * 10, Color.red);
        if (hit.collider == null)
            return false;
        else if (hit.collider.CompareTag("Player"))
            return true;
        else
            return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to visualize the wander radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        if(hasLineOfSight) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}




