using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float jumpForce = 5f;

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
            // Si está detenido, mantiene la rotación actual solo en el eje Y
            rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            return;
        }

        Vector3 direction = new Vector3(playerInputs.x, 0f, playerInputs.y);
        Quaternion targetYaw = Quaternion.LookRotation(direction);

        // Rota hacia la dirección del movimiento sin inclinaciones por agacharse
        rb.rotation = targetYaw;
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