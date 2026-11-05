
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;

    [SerializeField]
    private string[] gemasNecesarias =
{
    "Galleta1", "Galleta2", "Galleta3", "Galleta4", "Galleta5"
};

    private bool[] gemasRecolectadas;

    public event Action OnGemasActualizadas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gemasRecolectadas = new bool[gemasNecesarias.Length];

        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= AlCargarEscena;
            Instance = null;
        }
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        ReiniciarGemas();
    }

    public void ReiniciarGemas()
    {
        gemasRecolectadas = new bool[gemasNecesarias.Length];

        OnGemasActualizadas?.Invoke();

        Debug.Log("Gemas reiniciadas.");
    }

    public void RecolectarGema(string nombreGema)
    {
        for (int i = 0; i < gemasNecesarias.Length; i++)
        {
            if (gemasNecesarias[i] == nombreGema)
            {
                if (!gemasRecolectadas[i])
                {
                    gemasRecolectadas[i] = true;

                    Debug.Log($"Gema recolectada: {nombreGema} ({ContarGemas()}/{gemasNecesarias.Length})");

                    OnGemasActualizadas?.Invoke();
                }

                return;
            }
        }

        Debug.LogWarning("No existe una gema con el nombre: " + nombreGema);
    }

    public int ContarGemas()
    {
        int contador = 0;

        foreach (bool recolectada in gemasRecolectadas)
        {
            if (recolectada)
                contador++;
        }

        return contador;
    }

    public int TotalGemas()
    {
        return gemasNecesarias.Length;
    }

    public bool TieneTodasLasGemas()
    {
        return ContarGemas() == gemasNecesarias.Length;
    }
}
