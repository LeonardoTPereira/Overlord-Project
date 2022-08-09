using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
public class DialogueTreeView : GraphView
{
    //required for viewing in the editor
    public new class UxmlFactory : UxmlFactory<DialogueTreeView, GraphView.UxmlTraits> { } 

    public DialogueTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueTree.uss");
        styleSheets.Add(styleSheet);
    }
}
