using UnityEngine;

public class DetectarVictoria : MonoBehaviour
{
    public WinManager winManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winManager.Ganar();
        }
    }
}