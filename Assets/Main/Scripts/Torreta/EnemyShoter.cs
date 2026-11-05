using UnityEngine;

/// <summary>
/// Enemigo que dispara proyectiles al jugador cuando está dentro del rango.
/// Ponlo en el enemigo. Asigna: Projectile Prefab y Fire Point (un empty hijo en el frente/arma).
/// El jugador debe tener el tag "Player".
/// </summary>
public class EnemyShooter : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;            // Si lo dejas vacío, busca el tag "Player"
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Ajustes")]
    [SerializeField] private float detectionRange = 15f;  // Distancia a la que empieza a disparar
    [SerializeField] private float fireRate = 0.7f;       // Segundos entre disparos (más bajo = más rápido)
    [SerializeField] private float rotationSpeed = 10f;   // Qué tan rápido gira hacia el jugador
    [SerializeField] private float fallbackAimHeight = 1f; // Solo se usa si el jugador no tiene Collider

    private float nextFireTime;
    private PlayerHealth playerHealth;
    private Collider playerCollider;

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
        {
            playerHealth = player.GetComponentInParent<PlayerHealth>();
            playerCollider = player.GetComponentInChildren<Collider>();
        }

        if (firePoint == null) firePoint = transform;
    }

    private void Update()
    {
        // No hace nada si no hay jugador o si está muerto (esperando respawn)
        if (player == null || (playerHealth != null && playerHealth.IsDead)) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > detectionRange) return;

        LookAtPlayer();

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Punto al que apunta: el centro del collider del jugador (su "cuerpo"), no sus pies ni su cabeza
    private Vector3 GetAimPoint()
    {
        if (playerCollider != null) return playerCollider.bounds.center;
        return player.position + Vector3.up * fallbackAimHeight;
    }

    private void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f; // Solo gira en horizontal
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector3 dir = (GetAimPoint() - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null) p.Launch(dir);
    }

    // Dibuja el rango en el editor (esfera amarilla al seleccionar el enemigo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}