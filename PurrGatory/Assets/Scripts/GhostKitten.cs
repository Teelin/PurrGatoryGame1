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



    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        target = player.Find("Sprite").Find("KittenFollowTarget");
        AudioSource = GetComponent<AudioSource>();
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
        SacredFire.fireLit.AddListener(OnFireLit);
        SacredFire.fireDoused.AddListener(OnFireDoused);
    }
    private void OnDisable()
    {
        SacredFire.fireLit.RemoveListener(OnFireLit);
        SacredFire.fireDoused.RemoveListener(OnFireDoused);
    }
    // Update is called once per frame
    void Update()
    {
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
        agent.SetDestination(CapturingEnemyPosition);
        StartCoroutine(DestroyKitten());

    }

    public IEnumerator DestroyKitten()
    {
        AudioSource.PlayOneShot(kittenMeow);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
