using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
//using DialogueTree = DialogueTrees.DialogueTree;

public class EditorDialogueTree : EditorWindow
{
    private TreeView _treeView;
    private InspectorView _inspectorView;

    [MenuItem("Window/UI Toolkit/EditorDialogueTree")]
    public static void ShowExample()
    {
        EditorDialogueTree wnd = GetWindow<EditorDialogueTree>();
        wnd.titleContent = new GUIContent("EditorDialogueTree");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueTree_2/EditorDialogueTree.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueTree_2/EditorDialogueTree.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<TreeView>();
        _inspectorView = root.Q<InspectorView>();
    }

    private void OnSelectionChange()
    {
        DialogueTree tree = Selection.activeObject as DialogueTree;

        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            _treeView.PopulateView(tree);

        }
    }
}