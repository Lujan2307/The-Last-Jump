using UnityEngine;

/// <summary>
/// Muestra una esfera de escudo alrededor del jugador mientras currentShield > 0.
/// Ponlo en el mismo objeto que PlayerHealth (la raíz del jugador).
/// Arrastra al campo "Shield Object" la esfera hija del jugador (sin collider).
/// La esfera se centra automáticamente en el collider del jugador y tiene un tamaño
/// fijo en el mundo, sin importar la escala del jugador.
/// </summary>
public class ShieldVisual : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject shieldObject;

    [Header("Tamaño y posición")]
    [Tooltip("Diámetro de la esfera en unidades del mundo (no depende de la escala del jugador)")]
    [SerializeField] private float shieldDiameter = 2.5f;
    [Tooltip("Desplazamiento extra desde el centro del jugador (en unidades del mundo)")]
    [SerializeField] private Vector3 centerOffset = Vector3.zero;

    [Header("Efecto de pulso")]
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float pulseAmount = 0.05f;

    private Collider playerCollider;

    private void Awake()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();

        playerCollider = GetComponent<Collider>();
        if (playerCollider == null) playerCollider = GetComponentInChildren<Collider>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnShieldChanged += ActualizarEscudo;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnShieldChanged -= ActualizarEscudo;
    }

    private void Start()
    {
        if (playerHealth == null)
            Debug.LogError("[ShieldVisual] No encuentra PlayerHealth. Este script debe estar en el mismo objeto que PlayerHealth.", this);
        if (shieldObject == null)
            Debug.LogError("[ShieldVisual] El campo Shield Object está vacío. Arrastra la esfera del escudo.", this);

        // Estado inicial: visible solo si ya hay escudo
        if (playerHealth != null)
            ActualizarEscudo(playerHealth.CurrentShield, playerHealth.MaxShield);
    }

    private void LateUpdate()
    {
        if (shieldObject == null || !shieldObject.activeSelf) return;
        PosicionarEscudo();
    }

    // Centra la esfera en el jugador y le da un tamaño fijo en el mundo
    private void PosicionarEscudo()
    {
        Transform t = shieldObject.transform;

        // Centro del jugador: el centro de su collider (o su posición si no hay collider activo)
        Vector3 centro = transform.position;
        if (playerCollider != null && playerCollider.enabled)
            centro = playerCollider.bounds.center;

        t.position = centro + centerOffset;

        // Compensa la escala del padre para que el diámetro sea el mismo en el mundo
        Vector3 escalaPadre = t.parent != null ? t.parent.lossyScale : Vector3.one;
        float pulso = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        float d = shieldDiameter * pulso;

        t.localScale = new Vector3(
            d / Mathf.Max(0.0001f, Mathf.Abs(escalaPadre.x)),
            d / Mathf.Max(0.0001f, Mathf.Abs(escalaPadre.y)),
            d / Mathf.Max(0.0001f, Mathf.Abs(escalaPadre.z)));
    }

    private void ActualizarEscudo(float actual, float maximo)
    {
        Debug.Log($"[ShieldVisual] Escudo cambió: {actual}/{maximo}. Esfera {(actual > 0f ? "visible" : "oculta")}.");

        if (shieldObject == null) return;

        shieldObject.SetActive(actual > 0f);
        if (actual > 0f) PosicionarEscudo(); // Aparece ya centrada, sin saltos
    }

    // Prueba rápida: clic derecho en el título del componente en el Inspector (en Play)
    [ContextMenu("Probar: mostrar esfera")]
    private void ProbarMostrar()
    {
        if (shieldObject == null) return;
        shieldObject.SetActive(true);
        PosicionarEscudo();
    }

    [ContextMenu("Probar: ocultar esfera")]
    private void ProbarOcultar()
    {
        if (shieldObject != null) shieldObject.SetActive(false);
    }
}