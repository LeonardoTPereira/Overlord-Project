using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class ImmersionQuestSOs : QuestSO
    {
        public override string symbolType {
            get { return Constants.TALK_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> immersionQuestWeights = new Dictionary<string, Func<int, int>>();
                immersionQuestWeights.Add( Constants.LISTEN, Constants.FiveOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.READ, Constants.FiveOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.EXCHANGE, Constants.FiveOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.GIVE, Constants.FiveOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.REPORT, Constants.FiveOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return immersionQuestWeights;
            } 
        }
    }
}
