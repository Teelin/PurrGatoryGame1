using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class GhostKitten : MonoBehaviour
{

    Transform player;
    Transform target;
    NavMeshAgent agent;
    public bool isBySacredFire = false;
    bool isFollowing = true;
    


    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        target = player.Find("KittenFollowTarget");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SacredFire.fireLit += OnFireLit;
        SacredFire.fireDoused += OnFireDoused;

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && isFollowing)
        {
            agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));

        }
    }

    void OnFireLit(SacredFire fire)
    {
        if (isBySacredFire)
        {
            isFollowing = false;
            agent.SetDestination(transform.position);
            
        }     
        
    }
    void OnFireDoused(SacredFire fire)
    {
        if (!isBySacredFire)
        {
            isFollowing = true;
        }
    }

    public void IsNearFire(bool nearFire)
    {
        isBySacredFire = nearFire;
    }
}
