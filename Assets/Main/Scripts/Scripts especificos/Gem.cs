using UnityEngine;

public class Gem : MonoBehaviour
{
    [Tooltip("Debe coincidir EXACTAMENTE con uno de los nombres del array en GemManager")]
    [SerializeField] private string nombreGema;

    [Header("Efectos opcionales")]
    [SerializeField] private AudioClip sonidoRecoleccion;
    [SerializeField] private GameObject efectoParticulas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            RecolectarGema();
        }
    }

    private void RecolectarGema()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.RecolectarGema(nombreGema);
        }
        else
        {
            Debug.LogError("No se encontró GemManager en la escena.");
        }

        // Efecto de sonido
        if (sonidoRecoleccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);
        }

        // Efecto de partículas
        if (efectoParticulas != null)
        {
            Instantiate(efectoParticulas, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}