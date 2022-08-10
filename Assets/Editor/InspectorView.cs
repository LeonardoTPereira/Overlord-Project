using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    //Editor editor; // "Editor é um namespace, mas está sendo usado como um tipo" why is this happening?????
    //public InspectorView()
    //{

    //}

    //internal void UpdateNodeSelection(NodeView nodeView)
    //{
    //    Clear();
    //    UnityEngine.Object.DestroyImmediate(editor);
    //    editor = Editor.CreateEditor(nodeView.node);

    //    IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
    //    Add(container);
    //}
}
