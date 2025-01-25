using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    private VisualTreeAsset m_MainMenu;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var buttons = uiDocument.rootVisualElement.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
