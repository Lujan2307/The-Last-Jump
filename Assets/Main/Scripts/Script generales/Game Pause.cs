using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class GamePause : MonoBehaviour
{

    private InputAction Pause; 


    public GameObject menuPause; 
    public bool gamePaused = false;

    private void Start()
    {
        Pause = InputSystem.actions.FindAction("Escape");
    }

    private void Update()
    {
        if (Pause.WasPressedThisFrame())
        {
            Debug.Log("Si está tomando el esc");
            if (gamePaused)
            {
                unpause();

            }
            else
            {

                pause();

            }
        }
    }
        public void unpause()
        {

            menuPause.SetActive(false);

            Time.timeScale = 1;
            gamePaused = false;

        }

        public void pause()
        {
            menuPause.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;


        }
 
   }












