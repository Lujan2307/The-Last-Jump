
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform player;
    public Transform shootPoint;
    public GameObject projectilePrefab;

    public float detectionRange = 15f;
    public float fireRate = 2f;
    public float rotationSpeed = 5f;

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            AimAtPlayer();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void AimAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void Shoot()
    {
        Vector3 direction =
            (player.position - shootPoint.position).normalized;

        GameObject bullet = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.LookRotation(direction)
        );

        bullet.GetComponent<TurretProjectile>()
            .Launch(direction);
    }
}
