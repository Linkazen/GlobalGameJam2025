using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] bool gamePaused = false;
    InputAction pauseAction;
    void Start()
    {
        //Input action init
        pauseAction = InputSystem.actions.FindAction("Submit");

        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause(); 
            }
        }
    }

    public void Pause() 
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {

    }
}
