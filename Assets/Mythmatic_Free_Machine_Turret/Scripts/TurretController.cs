using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls a turret that can rotate its base and weapon to track and shoot at enemies.
/// The turret has two parts: a rotating base and a weapon that can aim up/down.
/// Supports multiple projectile spawn points for simultaneous firing and explosion effects on impact.
/// </summary>

namespace Mythmatic.TurretSystem
{
    public class TurretController : MonoBehaviour
    {
        #region Turret Movement
        [Space(0)]
        [Header("================ TURRET MOVEMENT ================")]
        [Header("Controls how the turret base and weapon mount rotate to track targets")]
        [Header("")]

        [Header(">> Base Rotation")]
        [Tooltip("How fast the base can rotate left/right")]
        [Range(0, 360)]
        [SerializeField] private float baseRotationSpeed = 180f;

        [Tooltip("Angle precision for stopping rotation")]
        [Range(0.01f, 1f)]
        [SerializeField] private float alignmentThreshold = 0.1f;

        [Space(30)]
        [Header(">> Weapon Mount")]
        [Tooltip("How fast the weapon can pivot up/down (degrees per second)")]
        [Range(0, 360)]
        [SerializeField] private float weaponRotationSpeed = 180f;

        [Tooltip("Physical part of the turret that aims up/down")]
        [SerializeField] private Transform weaponMount;

        [Tooltip("Target point for aiming calculations (usually at barrel tip)")]
        [SerializeField] private Transform aimReference;
        #endregion

        #region Enemy Detection
        [Space(30)]
        [Header("================ TARGET DETECTION ================")]
        [Header("Settings for how the turret detects and engages enemies")]
        [Header("")]

        [Tooltip("Maximum distance to engage detected enemies")]
        [Range(1f, 200f)]
        [SerializeField] private float attackRange = 30f;
        #endregion

        #region Combat Settings
        [Space(30)]
        [Header("================== COMBAT ===================")]
        [Header("Weapon firing behavior and projectile settings")]
        [Header("")]

        [Header(">> Firing Controls")]
        [Tooltip("List of points where projectiles spawn from")]
        [SerializeField] private List<Transform> projectileSpawnPoints = new List<Transform>();

        [Tooltip("Projectile object to create when firing")]
        [SerializeField] private GameObject projectilePrefab;

        [Tooltip("Particle effect prefab for projectile explosion")]
        [SerializeField] private GameObject explosionPrefab;

        [Tooltip("Shots per second per spawn point")]
        [Range(0.1f, 10f)]
        [SerializeField] private float fireRate = 1f;

        [Space(10)]
        [Header(">> Projectile Behavior")]
        [Tooltip("Projectile travel speed")]
        [Range(1f, 100f)]
        [SerializeField] private float projectileSpeed = 20f;

        [Tooltip("How quickly projectiles can turn")]
        [Range(0f, 360f)]
        [SerializeField] private float projectileRotationSpeed = 180f;

        [Tooltip("Projectile tracking aggression (higher = tighter tracking)")]
        [Range(0f, 10f)]
        [SerializeField] private float projectileHomingStrength = 5f;

        [Tooltip("Time before projectile self-destructs")]
        [Range(0.1f, 10f)]
        [SerializeField] private float projectileLifetime = 3f;
        #endregion

        #region Animation
        [Space(30)]
        [Header("================== ANIMATION ==================")]
        [Header("Animator component for turret firing animations")]
        [Header("")]

        [SerializeField] private Animator animator;

        [Tooltip("Speed multiplier for attack animation")]
        [Range(0.1f, 10f)]
        public float animationSpeed = 1f;
        #endregion

        // Private tracking variables
        private GameObject enemy;                    
        private bool isBaseRotating = false;         
        private bool isWeaponRotating = false;       
        private float targetWeaponAngle = 0f;       
        private float currentWeaponAngle = 0f;      
        private bool isInRange = false;              
        private float nextFireTime = 0f;            
        private bool isReadyToFire = false;          
        private bool isFiring = false;              

        private void Start()
        {
            
            enemy = GameObject.FindGameObjectWithTag("Player");

            if (enemy == null)
            {
                Debug.LogWarning("TurretController: No GameObject found with tag 'Player' in the scene!");
            }

            
            if (projectileSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No projectile spawn points assigned to turret. Please assign at least one spawn point in the inspector.");
            }

            
            int projectileLayer = LayerMask.NameToLayer("Projectile");
            if (projectileLayer != -1)
            {
                Physics.IgnoreLayerCollision(projectileLayer, projectileLayer, true);
            }
            else
            {
                Debug.LogError("Layer 'Projectile' does not exist! Go to Edit > Project Settings > Tags and Layers and create a layer named 'Projectile'.");
            }
        }

        private void Update()
        {
            
            if (enemy == null)
            {
                enemy = GameObject.FindGameObjectWithTag("Player");
            }

            
            if (enemy == null || weaponMount == null || aimReference == null)
            {
                Debug.LogWarning("Missing required components: " +
                    (enemy == null ? "Enemy/Player " : "") +
                    (weaponMount == null ? "WeaponMount " : "") +
                    (aimReference == null ? "AimReference" : ""));
                UpdateFiringState(false);
                return;
            }

            
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            bool newInRange = distanceToEnemy <= attackRange;

            
            if (newInRange != isInRange)
            {
                isInRange = newInRange;
            }

            
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            Vector3 flatDirection = new Vector3(directionToEnemy.x, 0, directionToEnemy.z);

            
            if (flatDirection != Vector3.zero)
            {
                
                Quaternion targetBaseRotation = Quaternion.LookRotation(flatDirection, Vector3.up);

                
                if (!Mathf.Approximately(Quaternion.Angle(transform.rotation, targetBaseRotation), 0f))
                {
                    isBaseRotating = true;
                    RotateBase(targetBaseRotation);
                }
                
                else if (isBaseRotating)
                {
                    isBaseRotating = false;
                    CalculateWeaponRotation();
                }

                
                if (!isBaseRotating)
                {
                    CalculateWeaponRotation();
                    if (isWeaponRotating)
                    {
                        RotateWeapon();
                    }
                }

                
                isReadyToFire = isInRange && !isBaseRotating && !isWeaponRotating;

                
                if (isReadyToFire && Time.time >= nextFireTime)
                {
                    FireProjectile();
                    UpdateFiringState(true);
                }
                else if (!isReadyToFire && isFiring)
                {
                    UpdateFiringState(false);
                }
            }
            else
            {
                UpdateFiringState(false);
            }

            
            if (enemy != null && aimReference != null)
            {
                Debug.DrawLine(aimReference.position, enemy.transform.position, Color.blue);
            }
        }

        private void UpdateFiringState(bool firing)
        {
            if (isFiring != firing)
            {
                isFiring = firing;
                if (animator != null)
                {
                    animator.SetBool("Attack", firing);
                    animator.speed = animationSpeed;
                }
            }
        }

        private void RotateBase(Quaternion targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                baseRotationSpeed * Time.deltaTime
            );
        }

        private void CalculateWeaponRotation()
        {
            Vector3 toEnemy = enemy.transform.position - aimReference.position;
            Vector3 localToEnemy = transform.InverseTransformDirection(toEnemy);
            Vector3 localAimForward = transform.InverseTransformDirection(aimReference.forward);

            float targetAngle = -Mathf.Atan2(localToEnemy.y, localToEnemy.z) * Mathf.Rad2Deg;
            currentWeaponAngle = -Mathf.Atan2(localAimForward.y, localAimForward.z) * Mathf.Rad2Deg;

            float angleDifference = Mathf.DeltaAngle(currentWeaponAngle, targetAngle);

            if (Mathf.Abs(angleDifference) > alignmentThreshold)
            {
                isWeaponRotating = true;
                targetWeaponAngle = targetAngle;
            }
            else
            {
                isWeaponRotating = false;
            }
        }

        private void RotateWeapon()
        {
            // Calculate the shortest rotation path
            float angleDifference = Mathf.DeltaAngle(currentWeaponAngle, targetWeaponAngle);

            // Calculate maximum rotation this frame based on rotation speed
            float maxRotationThisFrame = weaponRotationSpeed * Time.deltaTime;

            // Determine actual rotation amount, clamped by speed
            float rotationAmount = Mathf.Clamp(angleDifference, -maxRotationThisFrame, maxRotationThisFrame);

            // Apply rotation
            weaponMount.localRotation *= Quaternion.Euler(rotationAmount, 0, 0);
            currentWeaponAngle += rotationAmount;

            // Check if we've reached the target angle
            if (Mathf.Abs(angleDifference) <= alignmentThreshold)
            {
                isWeaponRotating = false;
            }
        }

        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private void FireProjectile()
        {
            if (projectilePrefab == null || projectileSpawnPoints.Count == 0)
            {
                Debug.LogError("Cannot fire: " +
                    (projectilePrefab == null ? "No projectile prefab assigned. " : "") +
                    (projectileSpawnPoints.Count == 0 ? "No spawn points assigned." : ""));
                return;
            }

            int projectileLayer = LayerMask.NameToLayer("Projectile");

            // Fire from each spawn point
            foreach (Transform spawnPoint in projectileSpawnPoints)
            {
                if (spawnPoint == null)
                {
                    Debug.LogWarning("Null spawn point found in list!");
                    continue;
                }

                GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

                // Set the layer safely if it exists
                if (projectileLayer != -1)
                {
                    SetLayerRecursively(projectile, projectileLayer);
                }

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = projectile.AddComponent<Rigidbody>();
                }

                rb.useGravity = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                HomingProjectile behavior = projectile.AddComponent<HomingProjectile>();
                behavior.rotationSpeed = projectileRotationSpeed;
                behavior.homingStrength = projectileHomingStrength;
                behavior.maxLifetime = projectileLifetime;
                behavior.explosionPrefab = explosionPrefab;  // Pass the explosion prefab
                behavior.Initialize(enemy, projectileSpeed);
            }

            nextFireTime = Time.time + (1f / fireRate);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Draw attack range
            Gizmos.color = Color.yellow;
            DrawWireDisc(transform.position, Vector3.up, attackRange);
        }
#endif

        private void DrawWireDisc(Vector3 center, Vector3 normal, float radius)
        {
            int segments = 32;
            Vector3 previousPoint = center + (Vector3.forward * radius);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * 2 * Mathf.PI / segments;
                Vector3 newPoint = center + new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);
                Gizmos.DrawLine(previousPoint, newPoint);
                previousPoint = newPoint;
            }
        }
    }
}