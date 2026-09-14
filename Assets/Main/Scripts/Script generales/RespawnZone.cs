using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(health.MaxHealth);
        }
    }
}