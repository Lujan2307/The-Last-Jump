
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject panelPausa;
    public GameObject panelOpciones;

    private bool estaPausado = false;

    void Start()
    {
        Time.timeScale = 1f;
        estaPausado = false;

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
        panelOpciones.SetActive(false);
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("PAUSA ACTIVADA");
    }

    public void Continuar()
    {
        estaPausado = false;
        panelPausa.SetActive(false);
        panelOpciones.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("JUEGO REANUDADO");
    }

    public void Opciones()
    {
        panelPausa.SetActive(false);
        panelOpciones.SetActive(true);
        Debug.Log("OPCIONES ABIERTAS");
    }

    public void VolverPausa()
    {
        panelOpciones.SetActive(false);
        panelPausa.SetActive(true);
        Debug.Log("VOLVIENDO A PAUSA");
    }

    public void VolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
