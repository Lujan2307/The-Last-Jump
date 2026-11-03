
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 10f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction)
    {
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡La torreta golpeó al jugador!");

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
