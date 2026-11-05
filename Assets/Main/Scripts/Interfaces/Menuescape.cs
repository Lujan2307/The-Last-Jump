using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Ponlo en el Canvas "menu escape". El objeto debe quedar ACTIVO en la escena:
// el script oculta el menú apagando el componente Canvas, no el objeto.
public class MenuEscape : MonoBehaviour
{
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private bool abierto;
    private CursorLockMode lockPrevio;
    private bool cursorVisiblePrevio;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
        Mostrar(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        bool esc = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        bool esc = Input.GetKeyDown(KeyCode.Escape);
#endif
        if (!esc) return;

        if (!abierto)
        {
            lockPrevio = Cursor.lockState;
            cursorVisiblePrevio = Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            Mostrar(true);
        }
        else
        {
            Reanudar();
        }
    }

    // Úsalo también en el OnClick de un botón "Continuar"
    public void Reanudar()
    {
        Mostrar(false);
        Time.timeScale = 1f;
        Cursor.lockState = lockPrevio;
        Cursor.visible = cursorVisiblePrevio;
    }

    private void Mostrar(bool valor)
    {
        abierto = valor;
        if (canvas != null) canvas.enabled = valor;
        if (raycaster != null) raycaster.enabled = valor;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}