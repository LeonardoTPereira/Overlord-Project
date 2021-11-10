using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

public class Explore : NonTerminalQuest
{
    public Explore(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.6f;
    }

    protected override void DefineNextQuest(Manager m)
    {

        if (r > 2.6)
        {
            SecretRoomQuestSO secretRoomQuest = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
            /*TODO initiate data for secretRoomQuest
             secretRoomQuest.Init(...);
             */
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        else
        {
            SecretRoomQuestSO secretRoomQuest = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
            /*TODO initiate data for secretRoomQuest
             secretRoomQuest.Init(...);
             */
        }
    }
}