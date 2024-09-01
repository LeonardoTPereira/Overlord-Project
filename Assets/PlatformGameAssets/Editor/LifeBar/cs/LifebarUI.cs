using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class LifebarUI : EditorWindow
{
    [MenuItem("Window/UI Toolkit/LifebarUI")]
    public static void ShowExample()
    {
        LifebarUI wnd = GetWindow<LifebarUI>();
        wnd.titleContent = new GUIContent("LifebarUI");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/PlatformGameAssets/Editor/LifeBar/uxml/LifebarUI.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
    }
}