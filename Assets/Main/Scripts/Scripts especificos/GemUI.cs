
using UnityEngine;
using UnityEngine.UI;

public class GemUI : MonoBehaviour
{
    [Header("Iconos de las galletas")]
    [SerializeField] private Image[] iconosGemas;

    private void OnEnable()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.OnGemasActualizadas -= ActualizarInterfaz;
            GemManager.Instance.OnGemasActualizadas += ActualizarInterfaz;
        }
    }

    private void Start()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.OnGemasActualizadas -= ActualizarInterfaz;
            GemManager.Instance.OnGemasActualizadas += ActualizarInterfaz;

            ActualizarInterfaz();
        }
        else
        {
            Debug.LogError("No se encontró GemManager.");
        }
    }

    private void OnDisable()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.OnGemasActualizadas -= ActualizarInterfaz;
        }
    }

    private void ActualizarInterfaz()
    {
        if (GemManager.Instance == null)
            return;

        int cantidad = GemManager.Instance.ContarGemas();

        Debug.Log("Galletas recogidas: " + cantidad);

        for (int i = 0; i < iconosGemas.Length; i++)
        {
            if (iconosGemas[i] == null)
            {
                Debug.LogWarning("Falta asignar la imagen " + i);
                continue;
            }

            if (i < cantidad)
            {
                iconosGemas[i].color = Color.white;
            }
            else
            {
                iconosGemas[i].color = Color.gray;
            }

            Debug.Log("Imagen modificada: " + iconosGemas[i].name);
        }
    }
}
