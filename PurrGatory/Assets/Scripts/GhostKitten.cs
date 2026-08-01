using System.Collections;
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
    bool isCaptured = false;

    public static UnityEvent KittenSaved = new UnityEvent();
    

    [SerializeField] AudioClip kittenMeow;
    AudioSource AudioSource;

    Animator animator;
    [SerializeField] private float moveSpeed = 6.0f;
    private KittenManager kittenManager;
    private int followerIndex = 0;




    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        target = player.Find("Sprite").Find("KittenFollowTarget");
        AudioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator.SetTrigger("Spawn");


    }
    private void OnEnable()
    {
        SacredFire.fireLit.AddListener(OnFireLit);
        SacredFire.fireDoused.AddListener(OnFireDoused);
    }
    private void OnDisable()
    {
        SacredFire.fireLit.RemoveListener(OnFireLit);
        SacredFire.fireDoused.RemoveListener(OnFireDoused);
    }

    public void InitializeFollower(KittenManager tracker, int index)
    {
        kittenManager = tracker;
        followerIndex = index;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (!isCaptured)
        {
            if (player != null && isFollowing)
            {
                agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));

            }
            isBySacredFire = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SacredFire"));
            isByBarge = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SunBarge"));

            if (isByBarge && !kittenSaved)
            {
                SaveKitten();
            }
            if (isBySacredFire && isFireLit)
            {
                isFollowing = false;
                agent.SetDestination(transform.position);
            }
            else if (!isByBarge)
            {
                isFollowing = true;
            }
        }*/
        if (kittenManager == null || kittenManager.positionHistory.Count == 0) return;

        if (kittenManager != null && isFollowing)
        {
            // Calculate which recorded position node this specific kitten should target
            // We space them out along the position history based on their index
            int targetHistoryIndex = kittenManager.positionHistory.Count - 1 - followerIndex;

            // Ensure the index doesn't dip below zero if the player hasn't walked far yet
            targetHistoryIndex = Mathf.Max(0, targetHistoryIndex);

            Vector3 targetPosition = kittenManager.positionHistory[targetHistoryIndex];

            // Move towards the designated trail node
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Smoothly rotate to face the direction it is moving
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        isBySacredFire = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SacredFire"));
        isByBarge = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("SunBarge"));

        if (isByBarge && !kittenSaved)
        {
            SaveKitten();
        }
        if (isBySacredFire && isFireLit)
        {
            isFollowing = false;
            agent.SetDestination(transform.position);
            //make them move to the fire and stay there until the fire is doused
        }
        else if (!isByBarge)
        {
            isFollowing = true;
        }

    }

    void OnFireLit()
    {
        isFireLit = true;
    }
    void OnFireDoused()
    {
        isFireLit = false;
    }

    void SaveKitten()
    {
        kittenManager.RemoveKitten(gameObject);
        isFollowing = false;
        kittenSaved = true;
        GameObject sunBarge = GameObject.FindGameObjectWithTag("SunBarge");
        agent.SetDestination(sunBarge.transform.position);
        KittenSaved?.Invoke();
    }

    public void IsCaptured(Vector2 CapturingEnemyPosition)
    {
        isFollowing = false;
        isCaptured = true;
        kittenManager.RemoveKitten(gameObject);
        agent.SetDestination(CapturingEnemyPosition);
        StartCoroutine(DestroyKitten());

    }

    public IEnumerator DestroyKitten()
    {
        AudioSource.PlayOneShot(kittenMeow);
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
