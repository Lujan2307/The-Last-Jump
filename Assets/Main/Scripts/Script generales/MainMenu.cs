using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject mainMenu;
    public GameObject optionsMenu;


    public void OpenOptionsPanel()
    {

        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenMenuPanel()
    {

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }


    public void Play()
    {
        SceneManager.LoadScene("Mapa (Fácil) 1");


    }
}
