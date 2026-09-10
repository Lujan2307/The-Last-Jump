using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    [Header("Daño")]
    [SerializeField] private float daño = 999f; // Suficiente para matar de un golpe

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoDaño;
    [SerializeField] private float volumen = 1f;

    private bool puedeHacerDaño = true;

    void Update()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.transform.root.CompareTag("Player")) return;
        if (!puedeHacerDaño) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        if (sonidoDaño != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDaño, collision.transform.position, volumen);
        }

        playerHealth.TakeDamage(daño);
    }
}