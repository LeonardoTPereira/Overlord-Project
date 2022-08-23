using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAtributeCheckNode : CompositeNode
{
    public string attribute;
    public string attributeFactor;
    private int next;

    protected override void OnStart()
    {
        next = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //check the atribute and its factor to choose the next node
        //the next node might check another attribute or be an action node to return a string 
        return State.Running;
    }
}
