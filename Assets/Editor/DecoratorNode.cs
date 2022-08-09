using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
