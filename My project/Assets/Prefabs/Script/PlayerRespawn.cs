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
        }
    }
}