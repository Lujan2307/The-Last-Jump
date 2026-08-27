using UnityEngine;

public enum PowerUpType
{
    Health,
    Shield,
    Speed
}

/// <summary>
/// Power-up único y configurable: elegís el tipo en el Inspector.
/// Reemplaza a HealthPowerUp, ShieldPowerUp, DamagePowerUp y SpeedPowerUp.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;

    [Tooltip("Health: cantidad curada. Shield: cantidad de escudo. Damage: multiplicador de daño. Speed: multiplicador de velocidad.")]
    [SerializeField] private float amount = 1f;

    [Tooltip("Solo se usa para Damage y Speed (duración del buff en segundos).")]
    [SerializeField] private float duration = 5f;

    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] private GameObject pickupVFX;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"Power-up recogido: {type}");

        switch (type)
        {
            case PowerUpType.Health:
                other.GetComponent<PlayerHealth>()?.Heal(amount);
                break;

            case PowerUpType.Shield:
                other.GetComponent<PlayerHealth>()?.AddShield(amount);
                break;


            case PowerUpType.Speed:
                other.GetComponent<PlayerMovement>()?.ApplySpeedBoost(amount, duration);
                RegisterBuff(other, "Velocidad");
                Debug.Log($"Velocidad aumentada x{amount} durante {duration} segundos.");
                break;
        }

        if (pickupVFX != null)
        {
            Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    private void RegisterBuff(Collider other, string buffName)
    {
        ManagerBuff managerBuff = other.GetComponent<ManagerBuff>();
        if (managerBuff != null)
        {
            managerBuff.AddBuff(buffName, duration);
        }
    }
}