using System.Collections.Generic;
using UnityEngine;

//namespace DialogueTrees
//{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> _children = new List<Node>();

        public override Node Clone()
        {
            var node = Instantiate(this);
            node._children = _children.ConvertAll(c => c.Clone());
            return node;
        }
    }
//}
