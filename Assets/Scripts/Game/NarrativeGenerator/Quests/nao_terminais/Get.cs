using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Util;

public class Get : NonTerminalQuest
{
    private const int chance = 1/3;
    public override string symbolType {
        get { return Constants.GET_QUEST; }
        set {}
    }
    public Get()//(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.8f;
    }

    protected override void DefineNextQuest(Manager m)
    {
        if (r > 2.8)
        {
            GetQuestSO getItemQuest = ScriptableObject.CreateInstance<GetQuestSO>();
            /*TODO initiate data for getQuest*/

            Talk t = new Talk();//(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r > 2.5 && r <= 2.8)
        {
            GetQuestSO getItemQuest = ScriptableObject.CreateInstance<GetQuestSO>();
            /*TODO initiate data for getQuest*/
        }
        if (r > 2.2 && r <= 2.5)
        {
            DropQuestSO dropItemQuest = ScriptableObject.CreateInstance<DropQuestSO>();
            /*TODO initiate data for dropQuest*/
            
            Talk t = new Talk();//(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r <= 2.2)
        {
            DropQuestSO dropItemQuest = ScriptableObject.CreateInstance<DropQuestSO>();
            /*TODO initiate data for dropQuest*/
        }
    }
}