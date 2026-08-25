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
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (controller != null && animator != null)
        {
            UpdateMovementAnimation();
        }
    }

    private void UpdateMovementAnimation()
    {
        bool isRunning = controller.MoveValue.sqrMagnitude > 0.1f;
        animator.SetBool(IsRunningParameter, isRunning);

        bool isJumping = controller.IsJump;
        animator.SetBool(IsJumpingParameter, isJumping);

        bool isCrouching = controller.IsCrouch;
        animator.SetBool(IsCrouchingParameter, isCrouching);
    }
}