using UnityEngine;

public enum PowerUpType
{
    Health,
    Shield,
    Speed
}

/// <summary>
/// Power-up único y configurable: elegís el tipo en el Inspector.
/// Al recogerlo se oculta, y (si "Reaparecer Al Morir" está activo) vuelve a aparecer
/// cuando el jugador reaparece después de morir.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;

    [Tooltip("Health: cantidad curada. Shield: cantidad de escudo. Speed: multiplicador de velocidad.")]
    [SerializeField] private float amount = 1f;

    [Tooltip("Solo se usa para Speed (duración del buff en segundos).")]
    [SerializeField] private float duration = 5f;

    [Tooltip("Si 'Reaparecer Al Morir' está desactivado, destruye el power-up al recogerlo.")]
    [SerializeField] private bool destroyOnPickup = true;

    [Tooltip("El power-up se oculta al recogerlo y vuelve a aparecer cuando el jugador reaparece tras morir.")]
    [SerializeField] private bool reaparecerAlMorir = true;

    [SerializeField] private GameObject pickupVFX;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoRecoleccion;
    [SerializeField] private float volumenSonido = 1f;

    private bool recogido;
    private Renderer[] renderers;
    private Collider[] colliders;
    private Light[] luces;
    private PlayerHealth jugador;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
        luces = GetComponentsInChildren<Light>(true);
    }

    private void Start()
    {
        if (!reaparecerAlMorir) return;

        jugador = FindFirstObjectByType<PlayerHealth>();
        if (jugador != null)
            jugador.OnRespawn += Reaparecer;
        else
            Debug.LogWarning("[PowerUp] No se encontró PlayerHealth en la escena; no podrá reaparecer.", this);
    }

    private void OnDestroy()
    {
        if (jugador != null)
            jugador.OnRespawn -= Reaparecer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (recogido) return; // Evita que cuente dos veces si el jugador tiene varios colliders

        // Busca al jugador subiendo por la jerarquía: funciona aunque el collider esté en un hijo
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;

        recogido = true;
        Debug.Log($"Power-up recogido: {type}");

        switch (type)
        {
            case PowerUpType.Health:
                health.Heal(amount);
                break;

            case PowerUpType.Shield:
                health.AddShield(amount);
                Debug.Log($"Escudo agregado: +{amount}. Escudo actual: {health.CurrentShield}");
                break;

            case PowerUpType.Speed:
                PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
                if (movement != null) movement.ApplySpeedBoost(amount, duration);
                RegisterBuff(other, "Velocidad");
                Debug.Log($"Velocidad aumentada x{amount} durante {duration} segundos.");
                break;
        }

        if (sonidoRecoleccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position, volumenSonido);
        }

        if (pickupVFX != null)
        {
            Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }

        if (reaparecerAlMorir)
        {
            SetVisible(false);
        }
        else if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    // Se llama cuando el jugador reaparece después de morir
    private void Reaparecer()
    {
        if (!recogido) return;

        recogido = false;
        SetVisible(true);
    }

    // Oculta/muestra el power-up sin desactivar el objeto (para que siga escuchando el respawn)
    private void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers) if (r != null) r.enabled = visible;
        foreach (Collider c in colliders) if (c != null) c.enabled = visible;
        foreach (Light l in luces) if (l != null) l.enabled = visible;
    }

    private void RegisterBuff(Collider other, string buffName)
    {
        ManagerBuff managerBuff = other.GetComponentInParent<ManagerBuff>();
        if (managerBuff != null)
        {
            managerBuff.AddBuff(buffName, duration);
        }
    }
}