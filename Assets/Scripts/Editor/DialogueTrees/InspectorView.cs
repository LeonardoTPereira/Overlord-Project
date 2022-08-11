using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.DialogueTrees
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private UnityEditor.Editor _editor;
        public InspectorView()
        {

        }

        internal void UpdateNodeSelection(NodeView nodeView)
        {
            Clear();
            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(nodeView.node);

            var container = new IMGUIContainer(() => { _editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}
