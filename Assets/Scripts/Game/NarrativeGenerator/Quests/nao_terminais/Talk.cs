using System.Collections.Generic;
using System.Dynamic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Util;

public class Talk : NonTerminalQuest
{
    public override string symbolType {
        get { return Constants.TALK_QUEST; }
        set {}
    }
    public Talk()//(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.4f;
    }
    
    protected override void DefineNextQuest(Manager m)
    {
        TalkQuestSO talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
        if (r > 2.7)
        {
            /*TODO initiate data for talkQuest*/
            talkQuest.Init();
            talkQuest.SaveAsAsset("QUEST_NPC_1");
            Option(m);
        }
        else
        {
            /*TODO initiate data for talkQuest*/
        }
    }
}