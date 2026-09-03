using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;
    private InputAction moveAction;
    private InputAction isjump;
    private InputAction crouchAction;

    public Vector2 MoveValue { get; private set; }
    public bool JumpRequested { get; set; }
    public bool IsCrouch { get; private set; }
    public GroundCheck GroundCheck => groundCheck;

    private PlayerHealth playerHealth;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        isjump = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        // Habilitamos las acciones para asegurar que respondan desde el inicio
        moveAction?.Enable();
        isjump?.Enable();
        crouchAction?.Enable();

        if (groundCheck == null)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
        }

        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += DisableInputs;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= DisableInputs;
        }
    }

    private void DisableInputs()
    {
        MoveValue = Vector2.zero;
        JumpRequested = false;
        IsCrouch = false;
        enabled = false;
    }

    void Update()
    {
        if (moveAction != null)
        {
            MoveValue = moveAction.ReadValue<Vector2>();
        }

        if (isjump != null && isjump.WasPressedThisFrame() && groundCheck != null && groundCheck.isGrounded)
        {
            JumpRequested = true;
        }

        if (crouchAction != null)
        {
            IsCrouch = crouchAction.IsPressed();
        }
    }
}