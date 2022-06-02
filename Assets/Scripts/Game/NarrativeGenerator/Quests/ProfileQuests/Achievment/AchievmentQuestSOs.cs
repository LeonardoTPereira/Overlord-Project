using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class AchievmentQuestSOs : QuestSO
    {
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> achievmentQuestWeights = new Dictionary<string, Func<int, int>>();
                achievmentQuestWeights.Add( Constants.GATHER, Constants.FiveOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EXPERIMENT, Constants.FiveOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.REPAIR, Constants.FiveOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.TAKE, Constants.FiveOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.USE, Constants.FiveOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return achievmentQuestWeights;
            } 
        }
    }
}
