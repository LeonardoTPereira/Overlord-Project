using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    /******** will use this when we have the tree nodes ********/

    //Editor editor;
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

    //internal void UpdateEdgeSelection(ConditionEdge edgeView)
    //{
    //    Clear();
    //    UnityEngine.Object.DestroyImmediate(editor);
    //    editor = Editor.CreateEditor(edgeView);

    //    IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
    //    Add(container);
    //}
}
