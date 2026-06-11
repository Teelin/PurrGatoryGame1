using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private Rigidbody2D player_RB;
    private Animator player_Anim;

    Vector2 moveInput;

    [SerializeField] private float moveSpeed = 50f;

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
    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        player_RB.linearVelocity = moveInput * (moveSpeed* Time.deltaTime);
    }
}
