using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Collider Settings")]
    [SerializeField] private float standingHeight = 0.007902517f;
    [SerializeField] private float crouchingHeight = 0.004f; // Ajusta este valor si lo quieres más pequeño

    private PlayerController playerController;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private float standingCenterY;
    private float crouchingCenterY;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider != null)
        {
            // Guardamos el centro original cuando está de pie
            standingCenterY = capsuleCollider.center.y;

            // Calculamos automáticamente el centro al agacharse para mantener los pies pegados al suelo
            float heightDifference = standingHeight - crouchingHeight;
            crouchingCenterY = standingCenterY - (heightDifference / 2f);
        }
    }

    void FixedUpdate()
    {
        RotateTowardsDirection();
        Move();
        Jump();
        HandleCrouchCollider();
    }

    public void Move()
    {
        Vector2 playerInputs = playerController.MoveValue;
        float currentSpeed = playerController.IsCrouch ? velocity * crouchSpeedMultiplier : velocity;

        rb.linearVelocity = new Vector3(playerInputs.x * currentSpeed, rb.linearVelocity.y, playerInputs.y * currentSpeed);
    }

    private void RotateTowardsDirection()
    {
        Vector2 playerInputs = playerController.MoveValue;

        if (playerInputs.sqrMagnitude <= 0.01f)
        {
            rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            return;
        }

        Vector3 direction = new Vector3(playerInputs.x, 0f, playerInputs.y);
        Quaternion targetYaw = Quaternion.LookRotation(direction);

        rb.rotation = targetYaw;
    }

    private void Jump()
    {
        if (playerController.IsJump && !playerController.IsCrouch)
        {
            rb.AddForce(new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z), ForceMode.Impulse);
        }
    }

    private void HandleCrouchCollider()
    {
        if (capsuleCollider == null) return;

        if (playerController.IsCrouch)
        {
            capsuleCollider.height = crouchingHeight;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, crouchingCenterY, capsuleCollider.center.z);
        }
        else
        {
            capsuleCollider.height = standingHeight;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, standingCenterY, capsuleCollider.center.z);
        }
    }
}