using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/MainMenu")]
    public static void ShowExample()
    {
        MainMenu wnd = GetWindow<MainMenu>();
        wnd.titleContent = new GUIContent("MainMenu");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        SetupButtonHandler();
    }

    private void SetupButtonHandler()
    {
        VisualElement root = rootVisualElement;

        var buttons = root.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        if (button.name == "Options")
        {
            button.RegisterCallback<ClickEvent>(OptionsMenu);
            Debug.Log(button.name);

        }
        else if(button.name == "Play")
        {
            button.RegisterCallback<ClickEvent>(PlayGame);
            Debug.Log(button.name);
        }

    }

    private void OptionsMenu(ClickEvent evt)
    {
        VisualElement root = rootVisualElement;
        Debug.Log("Options Button Clicked");
    }

    private void PlayGame(ClickEvent evt)
    {
        VisualElement root = rootVisualElement;
        Debug.Log("Play Button Clicked");
    }
}
