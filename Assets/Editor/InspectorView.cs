using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    /******** will use this when we have the tree nodes ********/

    //Editor _editor;

    //public InspectorView()
    //{

    //}

    //internal void UpdateNodeSelection(NodeView nodeView)
    //{
    //    Clear();
    //    UnityEngine.Object.DestroyImmediate(_editor);
    //    _editor = Editor.CreateEditor(nodeView.node);

    //    IMGUIContainer container = new IMGUIContainer(() => { _editor.OnInspectorGUI(); });
    //    Add(container);
    //}
}
