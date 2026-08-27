using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();

        if (movement != null && spawnPoint != null)
        {
            movement.TeleportTo(spawnPoint.position, spawnPoint.rotation);

            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(health.MaxHealth);
            }
        }
    }
}