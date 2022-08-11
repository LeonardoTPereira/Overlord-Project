using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using DialogueTrees;
using System;

public class TreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<TreeView, GraphView.UxmlTraits> { }

    DialogueTree _dialogueTree;

    public TreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueTree_2/EditorDialogueTree.uss");
        styleSheets.Add(styleSheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>(); //allows to get each type bellow action node
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    internal void PopulateView(DialogueTree tree)
    {
        _dialogueTree = tree;

        //graphViewChanged -= OnGraphViewChanged;
        //DeleteElements(graphElements);
        //graphViewChanged += OnGraphViewChanged;

        //if (tree.RootNode == null)
        //{
        //    tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
        //    EditorUtility.SetDirty(tree);
        //    AssetDatabase.SaveAssets();
        //}

        //creates node view
        tree.Nodes.ForEach(n => CreateNodeView(n));
    }

    private void CreateNodeView(Node node)
    {
        var nodeView = new NodeView(node);
        AddElement(nodeView);
    }

    private void CreateNode(Type type)
    {
        var node = _dialogueTree.CreateNode(type);
        CreateNodeView(node);
    }
}
