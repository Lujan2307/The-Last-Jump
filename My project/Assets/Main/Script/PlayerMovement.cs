using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Floor Facing Settings")]
    [SerializeField] private float floorFacingAngle = 45f; // Angle in degrees to tilt forward

    private PlayerController playerController;
    private Rigidbody rb;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RotateTowardsDirection();
        Move();
        Jump();
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
            // If stopped, maintain current Y yaw but toggle floor pitch angle
            if (playerController.IsCrouch)
            {
                Quaternion currentYaw = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
                rb.rotation = currentYaw * Quaternion.Euler(floorFacingAngle, 0f, 0f);
            }
            else
            {
                rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            }
            return;
        }

        Vector3 direction = new Vector3(playerInputs.x, 0f, playerInputs.y);
        Quaternion targetYaw = Quaternion.LookRotation(direction);

        if (playerController.IsCrouch)
        {
            // Combines target movement direction with forward pitch
            rb.rotation = targetYaw * Quaternion.Euler(floorFacingAngle, 0f, 0f);
        }
        else
        {
            rb.rotation = targetYaw;
        }
    }

    private void Jump()
    {
        if (playerController.IsJump && !playerController.IsCrouch)
        {
            rb.AddForce(new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z), ForceMode.Impulse);
            Debug.Log("Si saltó");
        }
    }
}