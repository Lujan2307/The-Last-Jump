using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 5f;
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
        Vector3 playerDirection = new Vector3(playerInputs.x, rb.linearVelocity.y, playerInputs.y);

        rb.linearVelocity = new Vector3(playerInputs.x * velocity, rb.linearVelocity.y, playerInputs.y * velocity);
    }

    private void RotateTowardsDirection()
    {
        Vector2 playerInputs = playerController.MoveValue;

        if (playerInputs.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Vector3 direction = new Vector3(
            playerInputs.x,
            0f,
            playerInputs.y
        );

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.rotation = targetRotation;

    }

    private void Jump()
    {
        if (playerController.IsJump)
        {
            rb.AddForce(new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z), ForceMode.Impulse);
            Debug.Log("Si saltó");
        }
    }


}