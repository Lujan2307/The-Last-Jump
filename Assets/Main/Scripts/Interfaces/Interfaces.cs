
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelInicio;
    public GameObject panelOpciones;

    public void Jugar()
    {
        SceneManager.LoadScene("Mapa1");
    }

    public void Opciones()
    {
        panelInicio.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void Volver()
    {
        panelOpciones.SetActive(false);
        panelInicio.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }


    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
    }

}
