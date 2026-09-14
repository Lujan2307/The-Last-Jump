using UnityEngine;
using TMPro;

/// <summary>
/// Maneja toda la UI del jugador en un solo lugar: vida (signos),
/// escudo, y contador de velocidad actual (para ver el buff de Velocidad).
/// </summary>
public class UiManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Vida (signos)")]
    [SerializeField] private GameObject[] signosDeVida;

    [Header("Escudo")]
    [Tooltip("Objeto contenedor del escudo en la UI, se oculta solo si el escudo está en 0.")]
    [SerializeField] private GameObject shieldContainer;
    [SerializeField] private TMP_Text shieldText;

    [Header("Velocidad")]
    [SerializeField] private TMP_Text speedText;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateVida;
        playerHealth.OnShieldChanged += UpdateEscudo;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateVida;
        playerHealth.OnShieldChanged -= UpdateEscudo;
    }

    private void Start()
    {
        UpdateVida(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        UpdateEscudo(playerHealth.CurrentShield, playerHealth.MaxShield);
    }

    private void Update()
    {
        if (speedText != null && playerMovement != null)
        {
            speedText.text = $"Velocidad: {playerMovement.CurrentSpeed:F1}";
        }
    }

    private void UpdateVida(float current, float max)
    {
        int vidaActual = Mathf.RoundToInt(current);
        

        for (int i = 0; i < signosDeVida.Length; i++)
        {
            if (signosDeVida[i] != null)
            {
                signosDeVida[i].SetActive(i < vidaActual);
            }
            else
            {
                Debug.LogWarning($"[UiManager] signosDeVida[{i}] es null.");
            }
        }
    }

    private void UpdateEscudo(float current, float max)
    {
        if (shieldContainer != null)
        {
            shieldContainer.SetActive(current > 0f);
        }

        if (shieldText != null)
        {
            shieldText.text = $"Escudo: {current:F0}/{max:F0}";
        }
    }
}