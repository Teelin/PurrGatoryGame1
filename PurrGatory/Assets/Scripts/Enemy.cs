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

    private float timer;
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;


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
        if (player != null && isFollowing && !isStunned)
        {
            //agent.isStopped = false;
            agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
        }
        if(!isFollowing && !isStunned)
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
            //agent.isStopped = true;
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
}




