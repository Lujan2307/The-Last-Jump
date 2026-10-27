using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 LastCheckpoint;
    public static Quaternion LastRotation;
    public static bool HasCheckpoint;

    [SerializeField] private Transform spawnPoint; // opcional

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() == null) return;

        Transform point = spawnPoint ? spawnPoint : transform;
        LastCheckpoint = point.position;
        LastRotation = point.rotation;
        HasCheckpoint = true;

        Debug.Log("Checkpoint activado: " + LastCheckpoint);
    }
}