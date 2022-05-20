using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class TalkQuestSO : QuestSO
    {
        public NpcSO npc { get; set; }
        public override string symbolType { 
            get { return Constants.TALK_TERMINAL;} 
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> talkQuestWeights = new Dictionary<string, Func<int, int>>();
                talkQuestWeights.Add( Constants.TALK_TERMINAL, Constants.OneOptionQuestLineWeight );
                talkQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return talkQuestWeights;
            } 
        }

        public override void Init()
        {
            base.Init();
            npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, NpcSO npc)
        {
            base.Init(questName, endsStoryLine, previous);
            this.npc = npc;
        }
        
        public void AddNpc(NpcSO npc)
        {
            this.npc = npc;
        }
    }
}
