using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;
    private InputAction moveAction;
    private InputAction isjump;
    private InputAction crouchAction;

    public Vector2 MoveValue { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsCrouch { get; private set; }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        isjump = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        if (groundCheck == null)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
        }

        if (groundCheck == null)
        {
            Debug.LogError("EL SCRIPT GROUNDCHECK NO ESTA ASIGNADO NI ENCONTRADO EN LOS HIJOS");
        }
        if (isjump == null)
        {
            Debug.LogError("SE DAÑO EL SALTO ");
        }
        if (crouchAction == null)
        {
            Debug.LogWarning("crouch se daño.");
        }
    }

    void Update()
    {
        if (moveAction != null)
        {
            MoveValue = moveAction.ReadValue<Vector2>();
        }

        if (isjump != null && groundCheck != null)
        {
            IsJump = isjump.IsPressed() && groundCheck.isGrounded;
        }
        else
        {
            IsJump = false;
        }

        if (crouchAction != null)
        {
            IsCrouch = crouchAction.IsPressed();
        }
    }
}