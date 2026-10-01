using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    [Header("Posición de Reaparición Actual")]
    public Vector3 puntoDeRespawnActual;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            puntoDeRespawnActual = player.transform.position;
        }
    }

    public void ActualizarCheckpoint(Vector3 nuevaPosicion)
    {
        
        puntoDeRespawnActual = nuevaPosicion + new Vector3(0, 0.5f, 0);
        Debug.Log("¡Checkpoint guardado en: " + puntoDeRespawnActual);
    }
}