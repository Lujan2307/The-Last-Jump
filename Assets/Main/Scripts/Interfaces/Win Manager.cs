using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject panelGanaste;

    public void Ganar()
    {
        panelGanaste.SetActive(true);
        Time.timeScale = 0f;
    }
}