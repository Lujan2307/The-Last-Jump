using UnityEngine;

/// <summary>
/// Proyectil. Ponlo en el prefab del proyectil. Necesita un Collider (el script lo pone como Trigger).
/// El Rigidbody es opcional: si no lo tiene, el script lo agrega solo.
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 1f;   // Tu vida máxima es 3, así que 1 = un tercio de vida
    [SerializeField] private float lifeTime = 5f;

    [Header("Depuración")]
    [SerializeField] private bool logCollisions = true; // Muestra en la Consola con qué chocó

    private Vector3 direction;
    private bool launched;

    private void Awake()
    {
        // Forzamos la configuración para que la física no lo empuje ni lo haga "volar"
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        launched = true;
        Destroy(gameObject, lifeTime); // Se autodestruye si no choca con nada
    }

    private void Update()
    {
        if (!launched) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignora al enemigo y a otros proyectiles
        if (other.GetComponentInParent<EnemyShooter>() != null) return;
        if (other.GetComponentInParent<Projectile>() != null) return;

        // 1) Si es el jugador, le hace daño y se destruye
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            if (logCollisions) Debug.Log($"Proyectil golpeó al jugador: {other.name}");
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 2) Ignora otros triggers (checkpoints, gemas, power-ups, zonas invisibles...)
        if (other.isTrigger) return;

        // 3) Choca con algo sólido del escenario
        if (logCollisions) Debug.Log($"Proyectil destruido por chocar con: {other.name}", other.gameObject);
        Destroy(gameObject);
    }
}