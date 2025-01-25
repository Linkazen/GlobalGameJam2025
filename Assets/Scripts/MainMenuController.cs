using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Properties;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    private VisualTreeAsset m_MainMenu;
    [SerializeField] GameObject mainCamera;
    AudioSource m_audioSource;
    Slider volumeSlider;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var buttons = uiDocument.rootVisualElement.Query<Button>();
        buttons.ForEach(RegisterHandler);
        volumeSlider = uiDocument.rootVisualElement.Query<Slider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_audioSource = mainCamera.GetComponent<AudioSource>();
        volumeSlider = GetComponent<UIDocument>().rootVisualElement.Query<Slider>();
        volumeSlider.value = CrossSceneInformation.volume;
        m_audioSource.volume = CrossSceneInformation.volume;

    }

    // Update is called once per frame
    void Update()
    {
        m_audioSource.volume = volumeSlider.value;
        CrossSceneInformation.volume = volumeSlider.value;
    }

    private void RegisterHandler(Button button)
    {
        if (button.name == "Play")
        {
            button.RegisterCallback<ClickEvent>(Play);
        }
        else if (button.name == "Options")
        {
            button.RegisterCallback<ClickEvent>(PrintClickMessage);
        }
        else if (button.name == "Quit")
        {
            button.RegisterCallback<ClickEvent>(PrintClickMessage);
            Application.Quit();
        }
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        string buttonName = button.name;
        Debug.Log(buttonName);
    }

    private void Play(ClickEvent evt)
    {
        SceneManager.LoadScene(1);
    }
}
