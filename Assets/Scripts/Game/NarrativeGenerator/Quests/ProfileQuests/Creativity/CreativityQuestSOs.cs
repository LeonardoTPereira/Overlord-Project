using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class CreativityQuestSOs : QuestSO
    {
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> creativityQuestWeights = new Dictionary<string, Func<int, int>>();
                creativityQuestWeights.Add( Constants.LISTEN, Constants.FiveOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.READ, Constants.FiveOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EXCHANGE, Constants.FiveOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.GIVE, Constants.FiveOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.REPORT, Constants.FiveOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return creativityQuestWeights;
            } 
        }
    }
}
