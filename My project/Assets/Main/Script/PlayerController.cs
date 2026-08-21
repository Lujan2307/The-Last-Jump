using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;
    private InputAction moveAction;
    private InputAction isjump;

    public Vector2 MoveValue { get; private set; }
    public bool IsJump { get; private set; }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        isjump = InputSystem.actions.FindAction("Jump");


        if (groundCheck == null)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
        }


        if (groundCheck == null)
        {
            Debug.LogError("EL SCRIPT NO ESTA AGREGADO AL CUBO");
        }
        if (isjump == null)
        {
            Debug.LogError("SE DAÑO EL SALTO");
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
    }
}