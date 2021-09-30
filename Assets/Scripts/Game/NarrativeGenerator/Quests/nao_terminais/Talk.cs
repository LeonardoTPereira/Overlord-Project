using System.Collections.Generic;
using System.Dynamic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

public class Talk : NonTerminalQuest
{

    public Talk(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.4f;
    }
    
    protected override void DefineNextQuest(Manager m)
    {
        TalkQuestSO talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
        if (r > 2.7)
        {
            /*TODO initiate data for talkQuest*/
            Option(m);
        }
        else
        {
            /*TODO initiate data for talkQuest*/
        }
    }
}