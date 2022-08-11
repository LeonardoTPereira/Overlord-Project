using DialogueTrees;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.DialogueTrees
{
    public class DialogueTreeEditor : EditorWindow
    {
        private DialogueTreeView _treeView;
        private InspectorView _inspectorView;

        [MenuItem("Window/UI Toolkit/DialogueTreeEditor")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<DialogueTreeEditor>();
            wnd.titleContent = new GUIContent("Dialogue Tree Editor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is not DialogueTree) return false;
            OpenWindow();
            return true;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueTrees/DialogueTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueTrees/DialogueTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            _treeView = root.Q<DialogueTreeView>();
            _inspectorView = root.Q<InspectorView>();
            _treeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var tree = Selection.activeObject as DialogueTree;

            if (!tree && Selection.activeGameObject)
            {
                var runner = Selection.activeGameObject.GetComponent<TreeRunner>();
                if (runner)
                {
                    tree = runner.tree;
                }
            }

            if (Application.isPlaying)
            {
                if (tree)
                {
                    _treeView.PopulateView(tree);
                }
            }
            else
            {
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    _treeView.PopulateView(tree);
                }
            }

        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    break;
            }
        }

        void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView.UpdateNodeSelection(nodeView);
        }
    }
}