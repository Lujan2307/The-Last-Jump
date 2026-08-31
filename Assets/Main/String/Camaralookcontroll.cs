using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

/// <summary>
/// Permite mirar hacia arriba (clic izquierdo) o hacia abajo (clic derecho)
/// modificando el eje de Tilt de un CinemachinePanTilt.
/// La CinemachineCamera NO debe ser hija del jugador: solo debe tener
/// asignado el jugador como Follow/Look At target.
/// </summary>
[DisallowMultipleComponent]
public class CameraLookControl : MonoBehaviour
{
    [Header("Referencia")]
    [Tooltip("Arrastra aquí el componente CinemachinePanTilt de tu CinemachineCamera")]
    public CinemachinePanTilt panTilt;

    [Header("Configuración de rotación")]
    [Tooltip("Grados por segundo al mantener presionado el click")]
    public float velocidadRotacion = 60f;

    [Tooltip("Ángulo máximo mirando hacia arriba")]
    public float tiltMaximo = 35f;

    [Tooltip("Ángulo máximo mirando hacia abajo (negativo)")]
    public float tiltMinimo = -35f;

    [Tooltip("Si al presionar clic izquierdo la cámara mira hacia abajo en vez de arriba, activa esto")]
    public bool invertirControles = false;

    [Header("Retorno automático")]
    [Tooltip("Si la cámara vuelve sola al centro al soltar el click")]
    public bool volverAlCentro = true;
    public float velocidadRetorno = 40f;

    void Update()
    {
        if (panTilt == null)
        {
            Debug.LogWarning("CameraLookControl: no se asignó el CinemachinePanTilt.");
            return;
        }

        float input = 0f;

        if (Mouse.current == null)
        {
            Debug.LogWarning("CameraLookControl: no se detectó un mouse (Mouse.current es null).");
            return;
        }

        bool clickIzquierdo = Mouse.current.leftButton.isPressed;  // mirar arriba
        bool clickDerecho = Mouse.current.rightButton.isPressed;   // mirar abajo

        if (invertirControles)
        {
            if (clickIzquierdo) input = -1f;
            else if (clickDerecho) input = 1f;
        }
        else
        {
            if (clickIzquierdo) input = 1f;
            else if (clickDerecho) input = -1f;
        }

        float tiltActual = panTilt.TiltAxis.Value;

        if (input != 0f)
        {
            tiltActual += input * velocidadRotacion * Time.deltaTime;
        }
        else if (volverAlCentro)
        {
            tiltActual = Mathf.MoveTowards(tiltActual, 0f, velocidadRetorno * Time.deltaTime);
        }

        tiltActual = Mathf.Clamp(tiltActual, tiltMinimo, tiltMaximo);
        panTilt.TiltAxis.Value = tiltActual;
    }
}