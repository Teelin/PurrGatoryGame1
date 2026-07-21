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
    [SerializeField] int damage, attackDistance;

    Animator beetleAnim;

    Vector3 targetPos;

    [SerializeField] AudioClip attackClip, walkClip;
    [SerializeField] AudioSource audioSource;

    

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
        beetleAnim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        audioSource.clip = walkClip;


    }

    // Update is called once per frame
    void Update()
    {
        hasLineOfSight = CheckLineOfSight();
        whatEnemyCanSee = CheckEnemySight();


        
        if (player != null && !isStunned && hasLineOfSight)
        {
            //agent.isStopped = false;
            targetPos = new Vector3(target.position.x, target.position.y, 0);


            agent.SetDestination(targetPos);
            

            playerIsNear = Vector2.Distance(transform.position, player.transform.position) < attackDistance;
            canBeStunned = Vector2.Distance(transform.position, player.transform.position) < player.GetComponent<PlayerController>().GetRattleRange();
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
        if (!canAttack && !isStunned)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
       
        var direction = (targetPos - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);

    }
    
    void Wander()
    {
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            targetPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(targetPos);
            timer = 0;
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
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<BastHealth>().TakeDamage(damage);
        beetleAnim.SetTrigger("Attack");
        audioSource.PlayOneShot(attackClip);
    }

    
}




