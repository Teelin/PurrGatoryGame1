using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction moveAction, lookAction;
    //private Rigidbody2D player_RB;
    private Animator player_Anim;

    private Vector2Int currentRoom;

    Vector3 moveInput, lookInput;

    public static event Action rattleAction, useAction;

    InputAction rattleUsed;
    InputAction useItem;
    InputAction sacraficeLife;

    //[SerializeField] private float moveSpeed = 5f;

    bool canAttack = true;
    float timer = 0f;
    //[SerializeField] float attackCoolDown = 5f;

    bool nearBarge = false;

    CinemachineImpulseSource impulseSource;

    float moveSpeed, sightRange, rattleRange, rattleCooldown, damage;

    [SerializeField] Light2D rattleLight, eyeLight;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip rattleClip, useClip, sacraficeClip;
    [SerializeField] ParticleSystem rattleDust;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        //player_RB = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        player_Anim = GetComponentInChildren<Animator>();
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        rattleUsed = inputActions.FindAction("Rattle");
        useItem = inputActions.FindAction("Use");
        sacraficeLife = inputActions.FindAction("SacraficeLife");

    }
    private void Start()
    {
        Respawn();
        GetPlayerStats();
        rattleLight.pointLightOuterRadius = rattleRange;
        eyeLight.pointLightOuterRadius = sightRange;
        GameObject.FindGameObjectWithTag("CameraTarget").GetComponent<CameraController>().UpdateCameraPosition();
       //Camera.main.GetComponent<CameraController>().UpdateCameraPosition();
    }
    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        Vector3 movemntSpeed = moveInput * (moveSpeed * Time.deltaTime);
        transform.position += movemntSpeed;
        if(movemntSpeed != Vector3.zero)
        {
            transform.localScale = new Vector3(1, 1, 1);
            RotatePlayer();
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            //transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        
        LevelManager.Instance.SetPlayerRoom(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)));
        nearBarge = Physics2D.OverlapCircle(transform.position, .5f, LayerMask.GetMask("SunBarge"));

        if (rattleUsed.triggered)
        {
            if(canAttack)
            { 
                Attack();
                canAttack = false;
            }
            
        }
        if (useItem.triggered)
        {
            UseItem();
        }
        if (sacraficeLife.triggered)
        {
            SacraficeLife();
        }

        if(!canAttack) 
        {
            timer += Time.deltaTime;
            if (timer >= rattleCooldown)
            {
                canAttack = true;
                timer = 0;
            }
        }

        /*if (movemntSpeed.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (movemntSpeed.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }*/

        //player_Anim.SetFloat("XSpeed", Mathf.Abs(moveInput.x));
        //player_Anim.SetFloat("SpeedY", moveInput.y);
        player_Anim.SetFloat("Speed", movemntSpeed.sqrMagnitude);

    }

    void GetPlayerStats()
    {
        moveSpeed = GameManager.Instance.GetSpeed();
        sightRange = GameManager.Instance.GetSightRange();
        rattleRange = GameManager.Instance.GetRattleRange();
        rattleCooldown = GameManager.Instance.GetRattleCooldown();
        damage = GameManager.Instance.GetDamage();
    }


    private void FixedUpdate()
    {
        if(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)) != currentRoom)
        {
            currentRoom = new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9));
            LevelManager.Instance.SetPlayerRoom(currentRoom);
            GameObject.FindGameObjectWithTag("CameraTarget").GetComponent<CameraController>().UpdateCameraPosition();
            //Camera.main.GetComponent<CameraController>().UpdateCameraPosition();
        }
    }

    void Respawn()
    {
        Vector2Int startingPos = LevelManager.Instance.GetStartingPosition();
        transform.position = new Vector3(startingPos.x*16, startingPos.y*9, 0);
        // Reset health and other player states here as needed
        Debug.Log("Player respawned at starting position.");
        currentRoom = startingPos;
        LevelManager.Instance.SetPlayerRoom(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)));

    }

    void RotatePlayer()
    {
        //Camera mainCamera = Camera.main;
        //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));

        Vector2 direction = moveInput;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Attack()
    {
        // Implement attack logic here
        rattleAction?.Invoke();
        StartCoroutine(Rattle());
        impulseSource.GenerateImpulse();
        audioSource.PlayOneShot(rattleClip);
        rattleDust.Play();
    }

    public float GetRattleDamege()
    {
        return damage;
    }
    public float GetRattleRange()
    {
        return rattleRange;
    }
    public float GetRattleCooldown()
    {
        return timer;
    }

    void UseItem()
    {
        // Implement use item logic here
        useAction?.Invoke();

        if(nearBarge)
        {
            LevelManager.Instance.LevelCompletedCheck();
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(useClip);
        }
        
    }

    void SacraficeLife() 
    { 
        GetComponent<BastHealth>().TakeLife();
        LevelManager.Instance.KittenSaved();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sacraficeClip);
        }
    }

    IEnumerator Rattle()
    {
        GameObject.FindGameObjectWithTag("RattleLight").GetComponent<Light2D> ().intensity = 10f;
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("RattleLight").GetComponent<Light2D> ().intensity = 1f;
    }

}
