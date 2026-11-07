using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject menuOpciones;
    public GameObject menuPrincipal;

    private bool pausado = false;

    void Start()
    {
        Time.timeScale = 1f;

        menuPause.SetActive(false);
        menuOpciones.SetActive(false);
        menuPrincipal.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (menuOpciones.activeSelf)
                VolverPausa();
            else if (pausado)
                Continuar();
            else
                Pausar();
        }
    }

    public void Pausar()
    {
        menuPause.SetActive(true);
        menuOpciones.SetActive(false);
        Time.timeScale = 0f;
        pausado = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continuar()
    {
        menuPause.SetActive(false);
        menuOpciones.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AbrirOpciones()
    {
        menuPause.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void VolverPausa()
    {
        menuOpciones.SetActive(false);
        menuPause.SetActive(true);
    }

    public void VolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menú principal");
    }
}