using System.Collections;
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    [SerializeField] private float daño = 1f;

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoDaño;
    [SerializeField] private float volumen = 1f;

    [Header("Cooldown")]
    [SerializeField] private float tiempoEntreDaños = 0.5f;

    private bool puedeHacerDaño = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.transform.root.CompareTag("Player")) return;
        if (!puedeHacerDaño) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        StartCoroutine(AplicarDaño(playerHealth, collision.transform));
    }

    private IEnumerator AplicarDaño(PlayerHealth playerHealth, Transform playerTransform)
    {
        puedeHacerDaño = false;

        if (sonidoDaño != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDaño, playerTransform.position, volumen);
        }

        playerHealth.TakeDamage(daño);

        yield return new WaitForSeconds(tiempoEntreDaños);

        puedeHacerDaño = true;
    }
}