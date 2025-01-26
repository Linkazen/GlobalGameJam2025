using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndScene : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    AudioSource m_audioSource;
    UIDocument m_EndUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_audioSource = mainCamera.GetComponent<AudioSource>();
        m_audioSource.volume = CrossSceneInformation.volume;
    }

    // Update is called once per frame
    void Update()
    {
        m_EndUI = GetComponent<UIDocument>();
        var buttons = m_EndUI.rootVisualElement.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(ChangeScene);
    }

    private void ChangeScene(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        if (button.name == "Title")
        {
            SceneManager.LoadScene(0);
        } 
        if (button.name == "Quit")
        {
            Application.Quit();
        }
    }
}
