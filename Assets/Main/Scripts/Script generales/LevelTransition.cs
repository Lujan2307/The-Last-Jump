using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private TMP_Text mensajeTexto;
    [SerializeField] private float duracionMensaje = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoPasarNivel;

    private bool transicionEnCurso = false;

    private void Start()
    {
        if (mensajeTexto != null)
            mensajeTexto.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transicionEnCurso) return;

        Debug.Log("Something entered the transition portal: " + other.gameObject.name);

        if (other.GetComponentInParent<PlayerController>() != null)
        {
            if (GemManager.Instance != null && GemManager.Instance.TieneTodasLasGemas())
            {
                Debug.Log("Player detected! Loading scene: " + nextSceneName);

                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    StartCoroutine(CargarNivelConSonido());
                }
                else
                {
                    Debug.LogError("Next scene name is EMPTY in the Inspector!");
                }
            }
            else
            {
                MostrarMensajeFaltanGemas();
            }
        }
        else
        {
            Debug.LogWarning("Object entered.");
        }
    }

    private IEnumerator CargarNivelConSonido()
    {
        transicionEnCurso = true;

        float espera = 0f;

        if (sonidoPasarNivel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoPasarNivel, transform.position);
            espera = sonidoPasarNivel.length;
        }

        yield return new WaitForSeconds(espera);

        SceneManager.LoadScene(nextSceneName);
    }

    private void MostrarMensajeFaltanGemas()
    {
        int actuales = GemManager.Instance.ContarGemas();
        int total = GemManager.Instance.TotalGemas();

        if (mensajeTexto != null)
        {
            mensajeTexto.text = $"Te faltan gemas: {actuales}/{total}";
            mensajeTexto.gameObject.SetActive(true);
            CancelInvoke(nameof(OcultarMensaje));
            Invoke(nameof(OcultarMensaje), duracionMensaje);
        }

        Debug.LogWarning($"No puedes pasar de nivel. Gemas: {actuales}/{total}");
    }

    private void OcultarMensaje()
    {
        if (mensajeTexto != null)
            mensajeTexto.gameObject.SetActive(false);
    }
}