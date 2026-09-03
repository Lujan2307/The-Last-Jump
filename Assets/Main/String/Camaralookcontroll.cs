using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

/// <summary>
/// Permite mirar hacia arriba (clic izquierdo) o hacia abajo (clic derecho)
/// modificando el TargetOffset del CinemachineRotationComposer.
///
/// El recorrido hacia arriba y hacia abajo se puede configurar
/// independientemente desde el Inspector.
///
/// La CinemachineCamera NO debe ser hija del jugador.
/// Debe tener asignado el jugador como Follow/Look At target.
/// </summary>
[DisallowMultipleComponent]
public class CameraLookControl : MonoBehaviour
{
    [Header("Referencia")]
    [Tooltip("Arrastra aquí el CinemachineRotationComposer de tu CinemachineCamera")]
    public CinemachineRotationComposer rotationComposer;

    [Header("Velocidad")]
    [Tooltip("Velocidad con la que la cámara se mueve mientras mantienes el click")]
    public float velocidadRotacion = 1f;

    [Header("Recorrido de la cámara")]
    [Tooltip("Cuánto puede subir la cámara desde su posición inicial")]
    public float recorridoArriba = 0.5f;

    [Tooltip("Cuánto puede bajar la cámara desde su posición inicial")]
    public float recorridoAbajo = 0.5f;

    [Header("Controles")]
    [Tooltip("Si está activado, invierte arriba y abajo")]
    public bool invertirControles = false;

    [Header("Retorno automático")]
    [Tooltip("La cámara vuelve al centro al soltar el click")]
    public bool volverAlCentro = true;

    [Tooltip("Velocidad con la que vuelve al centro")]
    public float velocidadRetorno = 1f;

    private float offsetInicialY;

    void Start()
    {
        if (rotationComposer == null)
        {
            Debug.LogWarning(
                "CameraLookControl: no se asignó el CinemachineRotationComposer."
            );
            return;
        }

        // Guardamos la posición inicial del TargetOffset
        offsetInicialY = rotationComposer.TargetOffset.y;
    }

    void Update()
    {
        if (rotationComposer == null)
            return;

        if (Mouse.current == null)
            return;

        float input = 0f;

        bool clickIzquierdo = Mouse.current.leftButton.isPressed;
        bool clickDerecho = Mouse.current.rightButton.isPressed;

        // ==========================================
        // CONTROLES
        // ==========================================

        if (invertirControles)
        {
            // Izquierdo = abajo
            // Derecho = arriba

            if (clickIzquierdo)
                input = -1f;
            else if (clickDerecho)
                input = 1f;
        }
        else
        {
            // Izquierdo = arriba
            // Derecho = abajo

            if (clickIzquierdo)
                input = 1f;
            else if (clickDerecho)
                input = -1f;
        }

        // ==========================================
        // TARGET OFFSET
        // ==========================================

        Vector3 offset = rotationComposer.TargetOffset;

        if (input != 0f)
        {
            offset.y += input * velocidadRotacion * Time.deltaTime;
        }
        else if (volverAlCentro)
        {
            offset.y = Mathf.MoveTowards(
                offset.y,
                offsetInicialY,
                velocidadRetorno * Time.deltaTime
            );
        }

        // ==========================================
        // LÍMITES INDEPENDIENTES
        // ==========================================

        float limiteSuperior =
            offsetInicialY + recorridoArriba;

        float limiteInferior =
            offsetInicialY - recorridoAbajo;

        offset.y = Mathf.Clamp(
            offset.y,
            limiteInferior,
            limiteSuperior
        );

        // ==========================================
        // APLICAR
        // ==========================================

        rotationComposer.TargetOffset = offset;
    }
}
