
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPausa;
    public GameObject panelOpciones;

    private bool estaPausado = false;

    void Start()
    {
        Time.timeScale = 1f;

        panelPausa.SetActive(false);
        panelOpciones.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (panelOpciones.activeSelf)
            {
                VolverPausa();
            }
            else if (estaPausado)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        estaPausado = true;
        panelPausa.SetActive(true);
        panelOpciones.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Continuar()
    {
        estaPausado = false;
        panelPausa.SetActive(false);
        panelOpciones.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Opciones()
    {
        panelPausa.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void VolverPausa()
    {
        panelOpciones.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void VolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
