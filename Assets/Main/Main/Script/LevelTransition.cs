using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Debug log to confirm ANY collision is happening
        Debug.Log("Something entered the transition portal: " + other.gameObject.name);

        // Check if the object or any of its parents has PlayerController
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            Debug.Log("Player detected! Loading scene: " + nextSceneName);

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Next scene name is EMPTY in the Inspector!");
            }
        }
        else
        {
            Debug.LogWarning("Object entered.");
        }
    }
}