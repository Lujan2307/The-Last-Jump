using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (CheckpointManager.instance != null)
            {
                CheckpointManager.instance.ActualizarCheckpoint(transform.position);
            }
        }
    }
}