using UnityEngine;

/// <summary>
/// Controls a projectile that follows a target and creates an explosion effect on impact.
/// </summary>
namespace Mythmatic.TurretSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class HomingProjectile : MonoBehaviour
    {
        [Header("Target Settings")]
        [Tooltip("The target that this projectile will follow")]
        public GameObject target;

        [Header("Movement Settings")]
        [Tooltip("How fast the projectile moves in units per second")]
        [Range(1f, 100f)]
        public float speed = 20f;

        [Tooltip("How quickly the projectile can turn in degrees per second")]
        [Range(0f, 720f)]
        public float rotationSpeed = 360f;

        [Tooltip("How aggressively the projectile tracks the target. Higher values mean tighter tracking")]
        [Range(0f, 10f)]
        public float homingStrength = 10f;

        [Header("Lifetime Settings")]
        [Tooltip("Maximum time in seconds before the projectile self-destructs")]
        [Range(0.1f, 10f)]
        public float maxLifetime = 3f;

        [Header("Effects")]
        [Tooltip("Particle effect prefab spawned when projectile hits target")]
        [SerializeField] private GameObject _explosionPrefab;
        public GameObject explosionPrefab
        {
            get { return _explosionPrefab; }
            set { _explosionPrefab = value; }
        }

        [Tooltip("Number of particles to emit on impact")]
        [Range(1, 100)]
        public int particleCount = 30;

        // Private runtime variables
        private Rigidbody rb;
        private float creationTime;
        private bool isDestroyed = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        /// <summary>
        /// Sets up the projectile with its target and optional speed override
        /// </summary>
        public void Initialize(GameObject enemy, float? projectileSpeed = null)
        {
            target = enemy;

            if (projectileSpeed.HasValue)
            {
                speed = projectileSpeed.Value;
            }

            creationTime = Time.time;

            // Set initial forward velocity
            if (target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }

        private void FixedUpdate()
        {
            if (isDestroyed) return;

            // Lifetime expiration check
            if (Time.time - creationTime > maxLifetime)
            {
                DestroyProjectile(transform.position);
                return;
            }

            // If target is destroyed mid-flight, keep moving forward instead of vanishing
            if (target == null)
            {
                rb.linearVelocity = transform.forward * speed;
                return;
            }

            // Calculate direction and steering
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Rotate velocity vector toward target
            Vector3 newVelocity = Vector3.Lerp(
                rb.linearVelocity.normalized,
                directionToTarget,
                homingStrength * Time.fixedDeltaTime
            ).normalized * speed;

            rb.linearVelocity = newVelocity;

            // Align rotation with moving direction using Rigidbody physics
            if (rb.linearVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            }
        }

        private void SpawnExplosion(Vector3 position)
        {
            if (_explosionPrefab == null) return;

            GameObject explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
            ParticleSystem particleSys = explosion.GetComponent<ParticleSystem>();

            if (particleSys != null)
            {
                var emission = particleSys.emission;
                emission.enabled = false;
                particleSys.Emit(particleCount);

                float lifetime = particleSys.main.duration + particleSys.main.startLifetime.constantMax;
                Destroy(explosion, lifetime);
            }
            else
            {
                Destroy(explosion, 1f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isDestroyed) return;

            Vector3 impactPosition = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;

            if (target == null || collision.gameObject == target)
            {
                SpawnExplosion(impactPosition);
            }

            DestroyProjectile(impactPosition);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDestroyed) return;

            if (target == null || other.gameObject == target)
            {
                SpawnExplosion(transform.position);
            }

            DestroyProjectile(transform.position);
        }

        private void DestroyProjectile(Vector3 position)
        {
            if (isDestroyed) return;
            isDestroyed = true;

            Destroy(gameObject);
        }
    }
}