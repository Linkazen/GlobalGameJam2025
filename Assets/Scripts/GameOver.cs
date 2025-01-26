using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject mainCamera;
    AudioSource m_audioSource;
    [SerializeField] AudioClip m_audioClip;
    InputAction clickAction;

    UIDocument m_GameOverUIDocument;
    void Start()
    {
        m_audioSource = mainCamera.GetComponent<AudioSource>();
        m_GameOverUIDocument = gameOverUI.GetComponent<UIDocument>();
        gameOverUI.SetActive(false);
        clickAction = InputSystem.actions.FindAction("Click");
    }

    void Update()
    {
        if (CrossSceneInformation.gameOver)
        {
            if (!gameOverUI.activeSelf)
            {
                StartCoroutine(StopGame());
            }
            StartCoroutine(HandleButtons());
        }
    }

    private void RegisterHandler(Button button)
    {
        if(button.name == "Restart")
        {
            button.RegisterCallback<ClickEvent>(Restart);
            
        }
        if(button.name == "Quit")
        {
            button.RegisterCallback<ClickEvent>(Quit);
        }
    }

    private void Restart(ClickEvent evt)
    {
        SceneManager.LoadScene(0);
        CrossSceneInformation.gameOver = false;
        gameOverUI.SetActive(false);
        m_audioSource.Play();
    }

    private void Quit(ClickEvent evt)
    {
        SceneManager.LoadScene(2);
    }
    
    IEnumerator StopGame()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0f;
        m_audioSource.Stop();

       
        m_audioSource.PlayOneShot(m_audioClip);
        gameOverUI.SetActive(true);
    }

    IEnumerator HandleButtons()
    {
        yield return new WaitForEndOfFrame();
        if (clickAction.WasPressedThisFrame())
        {
            var buttons = m_GameOverUIDocument.rootVisualElement.Query<Button>();
            buttons.ForEach(RegisterHandler);
        }
    }
}
