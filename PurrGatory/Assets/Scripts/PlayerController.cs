using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private Rigidbody2D player_RB;
    private Animator player_Anim;

    Vector3 moveInput;

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
        
    }
    private void Start()
    {
        Respawn();
    }
    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        transform.position += moveInput * (moveSpeed* Time.deltaTime);
    }

    void Respawn()
    {
        Vector2Int startingPos = GameManager.Instance.GetStartingPosition();
        transform.position = new Vector3(startingPos.x*16, startingPos.y*9, 0);
        // Reset health and other player states here as needed
        Debug.Log("Player respawned at starting position.");
    }
}
