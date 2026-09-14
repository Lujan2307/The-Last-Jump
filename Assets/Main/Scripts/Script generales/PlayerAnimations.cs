using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;

    private static readonly int IsRunningParameter = Animator.StringToHash("isRunning");
    private static readonly int IsJumpingParameter = Animator.StringToHash("isJumping");
    private static readonly int IsCrouchingParameter = Animator.StringToHash("isCrouching");

    void Awake()
    {
        if (controller == null) controller = GetComponent<PlayerController>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller == null || animator == null) return;

        if (!controller.enabled)
        {
            animator.SetBool(IsRunningParameter, false);
            animator.SetBool(IsJumpingParameter, false);
            animator.SetBool(IsCrouchingParameter, false);
            return;
        }

        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        // 1. Running
        bool isRunning = controller.MoveValue.sqrMagnitude > 0.1f;
        animator.SetBool(IsRunningParameter, isRunning);

        // 2. Jumping: Only set to TRUE if we are explicitly not grounded
        bool isJumping = false;
        if (controller.GroundCheck != null)
        {
            isJumping = !controller.GroundCheck.isGrounded;
        }

        animator.SetBool(IsJumpingParameter, isJumping);

        // 3. Crouching
        bool isCrouching = controller.IsCrouch;
        animator.SetBool(IsCrouchingParameter, isCrouching);
    }
}