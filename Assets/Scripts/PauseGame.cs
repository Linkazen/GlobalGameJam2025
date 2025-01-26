using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject pauseMenuUI;
    bool gamePaused = false;
    InputAction pauseAction;
    InputAction mouseAction;
    UIDocument m_PauseUI;
    Slider volumeSlider;

    [SerializeField] GameObject mainCamera;
    AudioSource m_audioSource;

    void OnEnable()
    {
        Time.timeScale = 1f;
    }
    void Start()
    {
        //Input action init
        pauseAction = InputSystem.actions.FindAction("Submit");
        mouseAction = InputSystem.actions.FindAction("Click");
        pauseMenuUI.SetActive(false);
        m_PauseUI = pauseMenuUI.GetComponent<UIDocument>();
        m_audioSource = mainCamera.GetComponent<AudioSource>();
        m_audioSource.volume = CrossSceneInformation.volume;
        volumeSlider = m_PauseUI.rootVisualElement.Query<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        m_audioSource.volume = CrossSceneInformation.volume / 4;
        if (pauseAction.WasPressedThisFrame() && !CrossSceneInformation.gameOver)
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
            StartCoroutine(HandleButtonInput());

            volumeSlider = m_PauseUI.rootVisualElement.Query<Slider>();
            CrossSceneInformation.volume = volumeSlider.value;
        }
    }

    public void Pause() 
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        volumeSlider = m_PauseUI.rootVisualElement.Query<Slider>();
        volumeSlider.value = CrossSceneInformation.volume;
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
        SceneManager.LoadScene(0);
    }

    public void RestartLevel(ClickEvent evt)
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator HandleButtonInput()
    {
        yield return new WaitForEndOfFrame();
        if (mouseAction.WasPressedThisFrame())
        {
            var buttons = m_PauseUI.rootVisualElement.Query<Button>();
            buttons.ForEach(RegisterHandler);
        }
        
    }
}
