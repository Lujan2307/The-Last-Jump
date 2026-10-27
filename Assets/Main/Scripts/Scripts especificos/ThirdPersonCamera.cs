using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;              // arrastra aquí al jugador
    [SerializeField] private Vector3 pivotOffset = new Vector3(0f, 1.5f, 0f); // punto al que mira (altura de la cabeza/pecho)

    [Header("Distancia")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 1f;

    [Header("Sensibilidad")]
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private bool invertY = false;

    [Header("Límites verticales")]
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Suavizado")]
    [SerializeField] private float followSmoothing = 15f;

    [Header("Colisión con paredes")]
    [SerializeField] private LayerMask collisionMask = ~0;  // quita la capa del Player aquí
    [SerializeField] private float collisionRadius = 0.25f;

    private float yaw;
    private float pitch = 15f;
    private Vector3 smoothedPivot;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("[ThirdPersonCamera] Falta asignar el Target.");
            enabled = false;
            return;
        }

        yaw = transform.eulerAngles.y;
        smoothedPivot = target.position + pivotOffset;
        LockCursor(true);
    }

    private void Update()
    {
        // Esc libera el mouse, clic lo vuelve a bloquear
        if (EscapePressed()) LockCursor(false);
        else if (Cursor.lockState != CursorLockMode.Locked && ClickPressed()) LockCursor(true);

        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 delta = GetLookDelta() * sensitivity;
        yaw += delta.x;
        pitch += invertY ? delta.y : -delta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Pivote suavizado para que no tiemble con el Rigidbody
        Vector3 desiredPivot = target.position + pivotOffset;
        smoothedPivot = Vector3.Lerp(smoothedPivot, desiredPivot, 1f - Mathf.Exp(-followSmoothing * Time.deltaTime));

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 direction = rotation * Vector3.back;

        // Si hay una pared entre el pivote y la cámara, acerca la cámara
        float finalDistance = distance;
        if (Physics.SphereCast(smoothedPivot, collisionRadius, direction, out RaycastHit hit,
                distance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            finalDistance = Mathf.Max(minDistance, hit.distance);
        }

        transform.rotation = rotation;
        transform.position = smoothedPivot + direction * finalDistance;
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    // ---- Lectura de input (funciona con el Input System nuevo o el viejo) ----
    private Vector2 GetLookDelta()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
#else
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * 10f;
#endif
    }

    private bool EscapePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    private bool ClickPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
}