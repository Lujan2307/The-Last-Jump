using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class MenuEscape : MonoBehaviour
{
    public Canvas canvasOpciones;

    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private bool abierto;
    private bool opcionesAbiertas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        Mostrar(false);

        if (canvasOpciones != null)
            canvasOpciones.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        bool esc = Keyboard.current != null &&
                   Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        bool esc = Input.GetKeyDown(KeyCode.Escape);
#endif

        if (!esc) return;

        if (!abierto)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Mostrar(true);
        }
        else if (opcionesAbiertas)
        {
            VolverPausa();
        }
        else
        {
            Reanudar();
        }
    }

    public void Reanudar()
    {
        Mostrar(false);

        if (canvasOpciones != null)
            canvasOpciones.gameObject.SetActive(false);

        opcionesAbiertas = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AbrirOpciones()
    {
        Mostrar(false);
        canvasOpciones.gameObject.SetActive(true);
        opcionesAbiertas = true;
    }

    public void VolverPausa()
    {
        canvasOpciones.gameObject.SetActive(false);
        opcionesAbiertas = false;
        Mostrar(true);
    }

    private void Mostrar(bool valor)
    {
        abierto = valor;

        if (canvas != null)
            canvas.enabled = valor;

        if (raycaster != null)
            raycaster.enabled = valor;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}