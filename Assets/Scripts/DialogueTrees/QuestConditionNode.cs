using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestConditionNode : CompositeNode
{
    //this script is a prototype

    public string questItem; //wich item
    public int questItemCount; //amount of the item required

    protected override void OnStart()
    {
        //if the player has the qust item and the desired quantity, right up on the start the quest is compleated
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if the player has the qust item and the desired quantity, return success
        //else return running
        return State.Running;
    }
}
