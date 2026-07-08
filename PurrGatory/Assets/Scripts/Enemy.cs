using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{

    Transform player;
    Transform target;
    NavMeshAgent agent;
    
    //bool isFollowing = false;
    bool isStunned = false;
    bool hasLineOfSight = false;
    bool canAttack = true;
    bool playerIsNear, canBeStunned;

    private float timer, attackTimer;
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;
    [SerializeField] float attackCooldown;
    [SerializeField] LayerMask detectionMask;
    [SerializeField] int damage;

    float stunDistance = 2.5f;

    enum EnemyState
    {
        Wandering,
        Following,
        Attacking,
        Stunned
    }

   //private EnemyState currentState = EnemyState.Wandering;

    enum EnemyCanSee
    {
        none,
        player,
        Kitten,
    }
    private EnemyCanSee whatEnemyCanSee;

    private void OnDisable()
    {
        PlayerController.rattleAction -= Stun;
    }

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        PlayerController.rattleAction += Stun;
        target = player.Find("Sprite").Find("KittenFollowTarget");
        timer = wanderTimer;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


    }

    // Update is called once per frame
    void Update()
    {
        hasLineOfSight = CheckLineOfSight();
        whatEnemyCanSee = CheckEnemySight();


        
        if (player != null && !isStunned && hasLineOfSight)
        {
            //agent.isStopped = false;
            agent.SetDestination(new Vector3(target.position.x, target.position.y,0));
            

            playerIsNear = Physics2D.OverlapCircle(transform.position, .5f, detectionMask);
            canBeStunned = Physics2D.OverlapCircle(transform.position, stunDistance, detectionMask);
            if (playerIsNear && canAttack)
            {
                Attack();
                canAttack = false;
                attackTimer = attackCooldown;
            }
        }
        


        if (!hasLineOfSight && !isStunned)
        {
            Wander();
        }

        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                canAttack = true;
            }
        }
        /*
        



        if (!isStunned) {

            if (playerIsNear && canAttack)
            {
                currentState = EnemyState.Attacking;
            }
            else if (whatEnemyCanSee == EnemyCanSee.player) { currentState = EnemyState.Following; isFollowing = true; }

            else
            {
                currentState = EnemyState.Wandering;
                isFollowing = false;
            }
        }


            switch (currentState)
        {
            case EnemyState.Wandering:
                Wander();
                break;
            case EnemyState.Following:
                agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
                break;
            case EnemyState.Attacking:
                Attack();
                canAttack = false;
                attackTimer = attackCooldown;
                break;
            case EnemyState.Stunned:
                StartCoroutine(StunCoroutine(3f));
                break;

        }

        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                canAttack = true;
            }
        }*/

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




    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        // Handle player collision logic here
    //        Debug.Log("Enemy collided with Player.");
    //        isFollowing = true;
    //        target = player.Find("KittenFollowTarget");
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        isFollowing = false;
    //    }
    //}

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
        if (canBeStunned)
        {
            StartCoroutine(StunCoroutine(4f));
            //currentState = EnemyState.Stunned;

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
    EnemyCanSee CheckEnemySight()
    {
        var hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, Mathf.Infinity, detectionMask);
        if (hit.collider == null)
            return EnemyCanSee.none;
        else if (hit.collider.CompareTag("Player"))
            return EnemyCanSee.player;
        else if (hit.collider.CompareTag("GhostKitten"))
            return EnemyCanSee.Kitten;
        else
            return EnemyCanSee.none;
    }

    void Attack()
    {
        // Implement attack logic here
        Debug.Log("Enemy is attacking the player!");
        GameObject.FindGameObjectWithTag("Player").GetComponent<BastHealth>().TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to visualize the wander radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        if (hasLineOfSight) 
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




