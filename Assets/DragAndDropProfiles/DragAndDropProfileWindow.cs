using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropProfileWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Drag and Drop")]
    public static void ShowExample()
    {
        var wnd = GetWindow<DragAndDropProfileWindow>();
        wnd.titleContent = new GUIContent("Drag and Drop");
    }
    
    public void CreateGUI()
    {
        Debug.Log("Create GUI");
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/DragAndDropProfiles/DragAndDropProfileWindow.uxml");
        VisualElement labelFromUxml = visualTree.Instantiate();
        root.Add(labelFromUxml);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DragAndDropProfiles/DragAndDropProfileWindow.uss");

        DragAndDropManipulator manipulator = new(rootVisualElement.Q<VisualElement>("object"));
    }
}