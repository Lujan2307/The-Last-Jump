
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePause : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject menuOpciones;

    public bool gamePaused = false;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (menuOpciones.activeSelf)
            {
                VolverPausa();
            }
            else if (gamePaused)
            {
                unpause();
            }
            else
            {
                pause();
            }
        }
    }

    public void pause()
    {
        menuPause.SetActive(true);
        menuOpciones.SetActive(false);

        Time.timeScale = 0;
        gamePaused = true;
    }

    public void unpause()
    {
        Debug.Log("BOTÓN CONTINUAR FUNCIONA");
        menuPause.SetActive(false);
        menuOpciones.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    public void AbrirOpciones()
    {
        Debug.Log("BOTÓN OPCIONES FUNCIONA");
        menuPause.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void VolverPausa()
    {
        menuOpciones.SetActive(false);
        menuPause.SetActive(true);
    }
}













