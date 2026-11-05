

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
        Debug.Log("BOTON PAUSAR FUNCIONA");

        estaPausado = true;
        panelPausa.SetActive(true);
        panelOpciones.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Continuar()
    {
        Debug.Log("BOTON CONTINUAR FUNCIONA");

        estaPausado = false;
        panelPausa.SetActive(false);
        panelOpciones.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Opciones()
    {
        Debug.Log("BOTON OPCIONES FUNCIONA");

        panelPausa.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void VolverPausa()
    {
        Debug.Log("BOTON VOLVER A PAUSA FUNCIONA");

        panelOpciones.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void VolverMenu()
    {
        Debug.Log("BOTON VOLVER AL MENU FUNCIONA");

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
