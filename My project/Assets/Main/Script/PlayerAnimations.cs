using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;

    private static readonly int IsRunningParameter
        = Animator.StringToHash("isRunning");

    private static readonly int IsJumpingParameter
        = Animator.StringToHash("isJumping");
    void Start()
    {

    }


    void Update()
    {
        updateMovementAnimation();
    }

    private void updateMovementAnimation()
    {
        bool isRunning = controller.MoveValue.sqrMagnitude > 0.1f;

        animator.SetBool(IsRunningParameter, isRunning);

        bool isJumping = controller.IsJump;

        animator.SetBool(IsJumpingParameter, isJumping);
    }

}


