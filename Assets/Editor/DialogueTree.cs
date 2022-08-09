using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogueTree : EditorWindow
{
    [MenuItem("Window/UI Toolkit/DialogueTree")]
    public static void OpenWindow()
    {
        DialogueTree wnd = GetWindow<DialogueTree>();
        wnd.titleContent = new GUIContent("DialogueTree");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DialogueTree.uxml");
        visualTree.CloneTree(root); //prevents the creation of an intermediate object

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueTree.uss");
        root.styleSheets.Add(styleSheet);
    }
}