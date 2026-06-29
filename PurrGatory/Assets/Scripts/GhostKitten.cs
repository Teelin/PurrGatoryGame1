using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class GhostKitten : MonoBehaviour
{

    Transform player;
    Transform target;
    NavMeshAgent agent;
    bool isBySacredFire = false;
    bool isFollowing = true;
    bool isByBarge = false;
    bool isFireLit = false;
    bool kittenSaved = false;

    public static UnityEvent KittenSaved = new UnityEvent();



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
        

    }
    private void OnEnable()
    {
        SacredFire.fireLit += OnFireLit;
        SacredFire.fireDoused += OnFireDoused;
    }
    private void OnDisable()
    {
        SacredFire.fireLit -= OnFireLit;
        SacredFire.fireDoused -= OnFireDoused;
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null && isFollowing)
        {
            agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));

        }
        isBySacredFire = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SacredFire"));
        isByBarge = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SunBarge"));

        if(isByBarge && !kittenSaved)
        {
            SaveKitten();
        }
        if(isBySacredFire && isFireLit)
        {
            isFollowing = false;
            agent.SetDestination(transform.position);
        }
        else
        {
            isFollowing = true;
        }
    }

    void OnFireLit(SacredFire fire)
    {
        isFireLit = true;
    }
    void OnFireDoused(SacredFire fire)
    {
        isFireLit = false;
    }

    void SaveKitten()
    {
        isFollowing = false;
        kittenSaved = true;
        GameObject sunBarge = GameObject.FindGameObjectWithTag("SunBarge");
        agent.SetDestination(sunBarge.transform.position);
        KittenSaved?.Invoke();
    }
}
