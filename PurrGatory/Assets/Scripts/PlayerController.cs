using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction moveAction, lookAction;
    private Rigidbody2D player_RB;
    private Animator player_Anim;

    private Vector2Int currentRoom;

    Vector3 moveInput, lookInput;

    public static event Action rattleAction, useAction;

    InputAction rattleUsed;
    InputAction useItem;

    [SerializeField] private float moveSpeed = 5f;

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
        player_RB = GetComponent<Rigidbody2D>();
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        rattleUsed = inputActions.FindAction("Rattle");
        useItem = inputActions.FindAction("Use");
    }
    private void Start()
    {
        Respawn();
        Camera.main.GetComponent<CameraController>().UpdateCameraPosition();
    }
    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        transform.position += moveInput * (moveSpeed* Time.deltaTime);
        RotatePlayer();
        GameManager.Instance.SetPlayerRoom(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)));

        if (rattleUsed.triggered)
        {
            Attack();
        }
        if (useItem.triggered)
        {
            UseItem();
        }
    }

    private void FixedUpdate()
    {
        if(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)) != currentRoom)
        {
            currentRoom = new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9));
            GameManager.Instance.SetPlayerRoom(currentRoom);
            Camera.main.GetComponent<CameraController>().UpdateCameraPosition();
        }
    }

    void Respawn()
    {
        Vector2Int startingPos = GameManager.Instance.GetStartingPosition();
        transform.position = new Vector3(startingPos.x*16, startingPos.y*9, 0);
        // Reset health and other player states here as needed
        Debug.Log("Player respawned at starting position.");
        currentRoom = startingPos;
        GameManager.Instance.SetPlayerRoom(new Vector2Int(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 9)));

    }

    void RotatePlayer()
    {
        Camera mainCamera = Camera.main;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Attack()
    {
        // Implement attack logic here
        rattleAction?.Invoke();
    }

    void UseItem()
    {
        // Implement use item logic here
        useAction?.Invoke();
    }
}
