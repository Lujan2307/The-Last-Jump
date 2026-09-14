using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Initialize to true so the player doesn't start in a jump pose on frame 1
    public bool isGrounded { get; private set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }
}