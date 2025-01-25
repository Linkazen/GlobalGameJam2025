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

    UIDocument m_GameOverUIDocument;
    void Start()
    {
        m_audioSource = mainCamera.GetComponent<AudioSource>();
        m_GameOverUIDocument = gameOverUI.GetComponent<UIDocument>();
        gameOverUI.SetActive(false);

    }

    void Update()
    {
        if (CrossSceneInformation.gameOver)
        {
            if (!gameOverUI.activeSelf)
            {
                m_audioSource.Stop();
                m_audioSource.PlayOneShot(m_audioClip);
                gameOverUI.SetActive(true);
            }
            var buttons = m_GameOverUIDocument.rootVisualElement.Query<Button>();
            buttons.ForEach(RegisterHandler);
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
}
