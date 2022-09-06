using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreetingsDecisionNode : NPCAtributeCheckNode
{
    protected override State OnUpdate()
    {
        //if(<get npc attribute> < 3) _children[0]
        //if(<get npc attribute> > 3 && <get npc attribute> < 5) _children[1]
        //if(<get npc attribute> > 5 && <get npc attribute> < 8) _children[2]
        //if(<get npc attribute> > 8) _children[3]
        //the next node might check another attribute or be an action node to return a string 
        return State.Running;
    }
}
