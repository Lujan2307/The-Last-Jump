using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;              // Arrastra aquí al jugador en el Inspector
    private NavMeshAgent agent;

    [Header("Configuración")]
    public float updateRate = 0.2f;       // Cada cuánto recalcula el camino (optimización)
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Si no asignaste el player manualmente, lo busca por tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            agent.SetDestination(player.position);
        }
    }

    // Si tu Collider tiene "Is Trigger" activado
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }

    // Si NO usas trigger (colisión física normal), usa este en vez del anterior
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}