using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueTrees
{
    [CreateAssetMenu]
    public class DialogueTree : ScriptableObject
    {
        [field: SerializeField] public Node RootNode { get; set; }
        [field: SerializeField] public Node.State TreeState { get; set; }

        //nodes can be detached and then attached to the tree, so we create a simple list
        [field: SerializeField] public List<Node> Nodes { get; set; }

        public Node.State Update()
        {
            if (RootNode.state == Node.State.Running)
            {
                TreeState = RootNode.Update();
            }
            return TreeState;
        }

        public Node CreateNode(System.Type type)
        {
            var node = CreateInstance(type) as Node;
            if (node == null) return null;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            Nodes.Add(node);
            TreeState = Node.State.Running;
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.child = child;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.children.Add(child);
            }

            var root = parent as RootNode;
            if (root != null)
            {
                root.child = child;
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.child = null;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.children.Remove(child);
            }

            var root = parent as RootNode;
            if (root != null)
            {
                root.child = null;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            var children = new List<Node>();

            var decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }

            var composite = parent as CompositeNode;
            if (composite)
            {
                return composite.children;
            }

            var root = parent as RootNode;
            if (root && root.child != null)
            {
                children.Add(root.child);
            }

            return children;
        }

        //clone the list of nodes
        public void Traverse(Node node, System.Action<Node> visiter)
        {
            if (!node) return;
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }

        //clone the tree so if there is more than one game object with the same tree, each one works independently
        public DialogueTree Clone()
        {
            var tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            tree.Nodes = new List<Node>();
            Traverse(tree.RootNode, (n) =>
            {
                tree.Nodes.Add(n);
            });
            return tree;
        }
    }
}
