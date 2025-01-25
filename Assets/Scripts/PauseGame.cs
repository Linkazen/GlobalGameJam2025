using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private VisualTreeAsset m_PauseMenu;
    [SerializeField] bool gamePaused = false;
    InputAction pauseAction;
    UIDocument m_PauseUI;
    void Start()
    {
        //Input action init
        pauseAction = InputSystem.actions.FindAction("Submit");
        pauseMenuUI.SetActive(false);
        m_PauseUI = pauseMenuUI.GetComponent<UIDocument>();
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
        if (gamePaused)
        {
            var buttons = m_PauseUI.rootVisualElement.Query<Button>();
            buttons.ForEach(RegisterHandler);
        }
    }

    public void Pause() 
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume(ClickEvent evt = null)
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    private void RegisterHandler(Button button)
    {
        if (button.name == "Resume")
        {
            button.RegisterCallback<ClickEvent>(Resume);
        }
        else if (button.name == "Restart")
        {
            button.RegisterCallback<ClickEvent>(RestartLevel);
        }
        else if (button.name == "Quit")
        {
            button.RegisterCallback<ClickEvent>(QuitGame);
        }
    }


    public void QuitGame(ClickEvent evt)
    {
        Debug.Log("Quit");
    }

    public void RestartLevel(ClickEvent evt)
    {

    }
}
