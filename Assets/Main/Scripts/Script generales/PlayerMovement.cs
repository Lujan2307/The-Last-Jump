using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Speed Boost")]
    [SerializeField] private float speedBoostMultiplier = 1f;

    [Header("Collider Settings")]
    [SerializeField] private float standingHeight = 0.007902517f;
    [SerializeField] private float crouchingHeight = 0.004f;

    private PlayerController playerController;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Coroutine speedBoostCoroutine;

    private float standingCenterY;
    private float crouchingCenterY;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider != null)
        {
            standingCenterY = capsuleCollider.center.y;
            float heightDifference = standingHeight - crouchingHeight;
            crouchingCenterY = standingCenterY - (heightDifference / 2f);
        }
    }

    void FixedUpdate()
    {
       rb.angularVelocity = Vector3.zero;
        
        if (playerController == null || !playerController.enabled) return;

        RotateTowardsDirection();
        Move();
        Jump();
        HandleCrouchCollider();
    }

    private Vector3 GetCameraRelativeDirection(Vector2 inputs)
    {
        if (Camera.main == null)
        {
            return new Vector3(inputs.x, 0f, inputs.y);
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        return (cameraForward * inputs.y) + (cameraRight * inputs.x);
    }

    public void Move()
    {
        Vector2 playerInputs = playerController.MoveValue;
        float currentSpeed = CurrentSpeed;

        Vector3 moveDirection = GetCameraRelativeDirection(playerInputs);

        rb.linearVelocity = new Vector3(
            moveDirection.x * currentSpeed,
            rb.linearVelocity.y,
            moveDirection.z * currentSpeed
        );
    }

    private void RotateTowardsDirection()
    {
        Vector2 playerInputs = playerController.MoveValue;

        if (playerInputs.sqrMagnitude <= 0.01f)
        {
            rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            return;
        }

        Vector3 moveDirection = GetCameraRelativeDirection(playerInputs);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetYaw = Quaternion.LookRotation(moveDirection);
            rb.rotation = targetYaw;
        }
    }

    private void Jump()
    {
        if (playerController.JumpRequested && !playerController.IsCrouch)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            playerController.JumpRequested = false;
        }
    }

    private void HandleCrouchCollider()
    {
        if (capsuleCollider == null) return;

        if (playerController.IsCrouch)
        {
            capsuleCollider.height = crouchingHeight;
            capsuleCollider.center = new Vector3(
                capsuleCollider.center.x,
                crouchingCenterY,
                capsuleCollider.center.z
            );
        }
        else
        {
            capsuleCollider.height = standingHeight;
            capsuleCollider.center = new Vector3(
                capsuleCollider.center.x,
                standingCenterY,
                capsuleCollider.center.z
            );
        }
    }

    public float CurrentSpeed
    {
        get
        {
            if (playerController == null) return velocity;

            float currentSpeed = playerController.IsCrouch
                ? velocity * crouchSpeedMultiplier
                : velocity;

            return currentSpeed * speedBoostMultiplier;
        }
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedBoostMultiplier = 1f;
        speedBoostCoroutine = null;
    }

    public void TeleportTo(Vector3 position, Quaternion rotation)
    {
        rb.position = position;
        rb.rotation = rotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}