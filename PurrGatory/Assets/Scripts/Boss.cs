using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{

    enum BossState
    {
        None,
        Attack1,
        Attack2,
        Enraged
    }

    private BossState currentState;

    Animator animator;
    BossHealth health;
    GameObject player;
    [SerializeField] Transform shootPosition;

    [SerializeField] GameObject projectile;
    [SerializeField] int baseAttackRate = 10;

    int attack2Counter = 0, attack1Counter = 0, enragedCounter = 0;

    bool isEnraged = false;
    bool enrangedFlipFlop = false;

    float attack1Timer = 1f, attack2Timer = .05f;

    [SerializeField] GameObject[] teleportPoints;
    GameObject teleportTarget;
    [SerializeField] float minTeleportTime = 3f, maxTeleportTime = 10f;
    bool changingTeleportTarget = false;
    [SerializeField] GameObject healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //animator = GetComponent<Animator>();
        health = GetComponent<BossHealth>();
        player = FindAnyObjectByType<PlayerController>().gameObject;
        currentState = BossState.None;
        Room.BossRoomEntered.AddListener(() => ChangeState(BossState.Attack1));
        teleportTarget = teleportPoints[Random.Range(0, teleportPoints.Length)];
        baseAttackRate = Random.Range(5, 15);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        switch (currentState)
        {
            case BossState.Attack1:
                RotateTowardsPlayer();
                if(attack1Timer <= 0f)
                {
                    attack1Counter++;
                    attack1Timer = 1f; // Reset the timer to 1 second
                    Attack1();
                }
                else
                {
                    attack1Timer -= Time.fixedDeltaTime; // Decrease the timer by the time elapsed since the last frame
                }
        
                
                if (attack1Counter >= baseAttackRate)
                {
                    if (isEnraged)
                    {
                        ChangeState(BossState.Enraged);
                    }
                    else
                    {
                        ChangeState(BossState.Attack2);
                    }
                    attack1Counter = 0;
                }

                break;

            case BossState.Attack2:
                
                if(attack2Timer <= 0f)
                {
                    attack2Counter++;
                    attack2Timer = .05f; // Reset the timer to .5f second
                    Attack2();
                }
                else
                {
                    attack2Timer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame
                }
                if (attack2Counter >= 72)
                {
                    if (isEnraged)
                    {
                        ChangeState(BossState.Enraged);
                    }
                    else
                    {
                        ChangeState(BossState.Attack1);
                    }
                    
                    attack2Counter = 0;
                }
                break;

            case BossState.Enraged:
                Enraged();
                if (enragedCounter >= 360)
                {
                    if (enrangedFlipFlop)
                    {
                        ChangeState(BossState.Attack1);
                    }
                    else
                    {
                        ChangeState(BossState.Attack2);
                    }
                    enrangedFlipFlop = !enrangedFlipFlop;
                    enragedCounter = 0;
                }
                break;
        }

        if (health.GetCurrentHealth() <= health.GetMaxHealth()/2 && !isEnraged)
        {
            ChangeState(BossState.Enraged);
            isEnraged = true;
        }
        if (teleportTarget.transform.position == transform.position && !changingTeleportTarget)
        {
            changingTeleportTarget = true;
            StartCoroutine(ChangeTeleportTarget());

        }
        transform.position = teleportTarget.transform.position;
        healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    void Attack1() 
    {
        Debug.Log("Boss is performing Attack1");
        GameObject projectileInstance = Instantiate(projectile, shootPosition.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce((player.transform.position - shootPosition.position).normalized * 5f, ForceMode2D.Impulse); // Adjust speed as needed

        //animator.SetTrigger("Attack1");
    }

    void Attack2()
    {
        Debug.Log("Boss is performing Attack2");
        GameObject projectileInstance = Instantiate(projectile, shootPosition.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce((transform.right).normalized * 5f, ForceMode2D.Impulse); // Adjust speed as needed
        transform.Rotate(0, 0, 5); 
        // animator.SetTrigger("Attack2");
    }
    void Enraged()
    {
        Debug.Log("Boss is Enraged");
        GameObject projectileInstance = Instantiate(projectile, shootPosition.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce((transform.right).normalized * 5f, ForceMode2D.Impulse); // Adjust speed as needed
        transform.Rotate(0, 0, 2);
        enragedCounter++;
        // animator.SetTrigger("Enraged");
    }

    IEnumerator ChangeTeleportTarget()
    {
        yield return new WaitForSeconds(Random.Range(minTeleportTime, maxTeleportTime));
        teleportTarget = teleportPoints[Random.Range(0, teleportPoints.Length)];
        
        changingTeleportTarget = false;
    }

}
