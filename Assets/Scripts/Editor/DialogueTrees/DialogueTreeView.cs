using System;
using System.Collections.Generic;
using System.Linq;
using DialogueTrees;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = DialogueTrees.Node;

namespace Editor.DialogueTrees
{
    public class DialogueTreeView : GraphView
    {
        public Action<NodeView> OnNodeSelected;
        private DialogueTree _dialogueTree;
        public new class UxmlFactory : UxmlFactory<DialogueTreeView, UxmlTraits> { }
        public DialogueTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueTreeEditor.uss");
            styleSheets.Add(styleSheet);

        }

        NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        internal void PopulateView(DialogueTree tree)
        {
            _dialogueTree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.RootNode == null)
            {
                tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            //creates node view
            tree.Nodes.ForEach(CreateNodeView);

            //creates the edges
            tree.Nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                });
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    switch (elem)
                    {
                        case NodeView nodeView:
                            _dialogueTree.DeleteNode(nodeView.node);
                            break;
                        case Edge edge:
                        {
                            var parentView = edge.output.node as NodeView;
                            var childView = edge.input.node as NodeView;
                            _dialogueTree.RemoveChild(parentView?.node, childView?.node);
                            break;
                        }
                    }
                });
            }

            graphViewChange.edgesToCreate?.ForEach(edge => {
                var parentView = edge.output.node as NodeView;
                var childView = edge.input.node as NodeView;
                _dialogueTree.AddChild(parentView?.node, childView?.node);
            });

            return graphViewChange;
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

        private void CreateNode(Type type)
        {
            var node = _dialogueTree.CreateNode(type);
            CreateNodeView(node);
        }

        private void CreateNodeView(Node node)
        {
            var nodeView = new NodeView(node)
            {
                OnNodeSelected = OnNodeSelected
            };
            AddElement(nodeView);
        }
    }
}
