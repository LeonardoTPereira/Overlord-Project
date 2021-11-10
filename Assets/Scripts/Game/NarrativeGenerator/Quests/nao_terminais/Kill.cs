using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using UnityEngine;

public class Kill : NonTerminalQuest
{
    public Kill(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.5f;
    }

    protected override void DefineNextQuest(Manager m)
    {
        KillQuestSO killQuest = ScriptableObject.CreateInstance<KillQuestSO>();

        if (r <= 2.3)
        {
            /*TODO initiate data for killQuest*/
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        else
        {
            /*TODO initiate data for killQuest*/
        }
    }
}