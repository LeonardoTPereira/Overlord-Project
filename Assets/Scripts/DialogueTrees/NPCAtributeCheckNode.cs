using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCAtributeCheckNode : CompositeNode
{
    public string attribute;
    public int attributeFactor;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //check the atribute and its factor to choose the next node
        //the next node might check another attribute or be an action node to return a string 
        if(attributeFactor < 4)                         _children.ElementAt(0).Update();
        if(attributeFactor >= 4 && attributeFactor < 8) _children.ElementAt(1).Update();
        if(attributeFactor >= 8)                        _children.ElementAt(2).Update();
        return State.Success;
        
    }
}
