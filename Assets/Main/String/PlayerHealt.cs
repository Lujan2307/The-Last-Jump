using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth;

    [Header("Escudo")]
    [SerializeField] private float maxShield = 50f;
    [SerializeField] private float currentShield = 0f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxShield => maxShield;
    public float CurrentShield => currentShield;
    public bool IsDead => currentHealth <= 0f;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnShieldChanged;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
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
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            OnDeath?.Invoke();
            
        }
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