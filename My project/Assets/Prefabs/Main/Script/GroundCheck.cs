using UnityEngine;
using UnityEngine.InputSystem;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded { get; private set; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground"))

        { isGrounded = true; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ground")) //el anterior tambien sirve, solo son dos formas de lo mismo

        { isGrounded = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ground"))
        { isGrounded = false; }
    }

}
