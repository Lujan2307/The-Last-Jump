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
    private Animator animator;
    private Vector3 ultimaPosicion;

    void Start()
    {
        // Busca el componente Animator en el objeto hijo (tu modelo 3D)
        animator = GetComponentInChildren<Animator>();
        ultimaPosicion = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        // Mover hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position, player.position, speed * Time.deltaTime);

        // Rotar para mirar hacia el jugador
        Vector3 direccion = (player.position - transform.position).normalized;
        direccion.y = 0; // Mantener la rotación horizontal
        if (direccion != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), Time.deltaTime * 10f);
        }

        // Controlar animación de caminar según movimiento real
        float distanciaMovida = Vector3.Distance(transform.position, ultimaPosicion);
        bool estaMoviendose = distanciaMovida > 0.001f;

        if (animator != null)
        {
            animator.SetBool("IsWalking", estaMoviendose);
        }

        ultimaPosicion = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.transform.root.CompareTag("Player")) return;
        if (!puedeHacerDaño) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        // Disparar la animación de patada en el modelo 3D
        if (animator != null)
        {
            animator.SetTrigger("Kick");
        }

        if (sonidoDaño != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDaño, collision.transform.position, volumen);
        }

        playerHealth.TakeDamage(daño);
    }
}