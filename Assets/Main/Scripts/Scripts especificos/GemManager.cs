using UnityEngine;
using UnityEngine.SceneManagement;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;

    // Array con las gemas necesarias
    [SerializeField] private string[] gemasNecesarias = { "Rubi", "Zafiro", "Esmeralda", "Topacio", "Amatista" };

    // Array paralelo que indica si cada gema fue recolectada
    private bool[] gemasRecolectadas;

    private void Awake()
    {
        // Singleton simple
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

        // Cada vez que se carga una escena (siguiente nivel o reinicio), las gemas se reinician
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDestroy()
    {
        // Solo la instancia original se suscribió al evento
        if (Instance == this)
            SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        ReiniciarGemas();
    }

    public void ReiniciarGemas()
    {
        gemasRecolectadas = new bool[gemasNecesarias.Length];
        Debug.Log("Gemas reiniciadas para el nuevo nivel.");
    }

    // Llamar esta función cuando el jugador recoja una gema
    public void RecolectarGema(string nombreGema)
    {
        for (int i = 0; i < gemasNecesarias.Length; i++)
        {
            if (gemasNecesarias[i] == nombreGema)
            {
                gemasRecolectadas[i] = true;
                Debug.Log($"Gema recolectada: {nombreGema} ({ContarGemas()}/{gemasNecesarias.Length})");
                return;
            }
        }
    }

    // Cuenta cuántas gemas van recolectadas
    public int ContarGemas()
    {
        int contador = 0;
        foreach (bool recolectada in gemasRecolectadas)
        {
            if (recolectada) contador++;
        }
        return contador;
    }

    public int TotalGemas()
    {
        return gemasNecesarias.Length;
    }

    // Revisa si ya tiene todas las gemas
    public bool TieneTodasLasGemas()
    {
        return ContarGemas() == gemasNecesarias.Length;
    }
}