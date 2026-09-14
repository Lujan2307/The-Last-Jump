using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip musicaFondo;

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

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource != null && musicaFondo != null)
        {
            audioSource.clip = musicaFondo;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}