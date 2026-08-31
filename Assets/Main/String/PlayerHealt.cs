using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth;

    [Header("Escudo")]
    [SerializeField] private float maxShield = 50f;
    [SerializeField] private float currentShield = 0f;

    [Header("Respawn")]
    [SerializeField] private Transform puntoRespawn;
    [SerializeField] private float tiempoRespawn = 2f;

    [Header("Referencias")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerController playerController;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxShield => maxShield;
    public float CurrentShield => currentShield;
    public bool IsDead => currentHealth <= 0f;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnShieldChanged;
    public event Action OnDeath;
    public event Action OnRespawn;

    private Rigidbody rb;

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (playerController == null) playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || IsDead) return;

        if (currentShield > 0f)
        {
            float absorbed = Mathf.Min(currentShield, amount);
            currentShield -= absorbed;
            amount -= absorbed;
            OnShieldChanged?.Invoke(currentShield, maxShield);
        }

        if (amount > 0f)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            Debug.Log($"Player recibió {amount} de daño.");
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Debug.Log("Player está muerto.");
            OnDeath?.Invoke();
            StartCoroutine(Morir());
        }
    }

    private IEnumerator Morir()
    {
        SetJugadorActivo(false);

        yield return new WaitForSeconds(tiempoRespawn);

        Respawn();
    }

    private void SetJugadorActivo(bool activo)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = activo;

        foreach (var collider in GetComponentsInChildren<Collider>())
            collider.enabled = activo;

        if (playerMovement != null)
            playerMovement.enabled = activo;

        if (playerController != null)
            playerController.enabled = activo;

        if (rb != null)
            rb.isKinematic = !activo;
    }

    private void Respawn()
    {
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
            transform.rotation = puntoRespawn.rotation;
        }

        SetJugadorActivo(true);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
        }

        currentHealth = maxHealth;
        currentShield = 0f;

        Debug.Log("Player reapareció.");
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnShieldChanged?.Invoke(currentShield, maxShield);
        OnRespawn?.Invoke();
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void AddShield(float amount)
    {
        if (amount <= 0f) return;
        currentShield = Mathf.Min(maxShield, currentShield + amount);
        OnShieldChanged?.Invoke(currentShield, maxShield);
    }
}