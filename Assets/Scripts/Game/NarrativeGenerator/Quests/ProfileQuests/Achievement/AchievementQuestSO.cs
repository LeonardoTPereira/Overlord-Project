using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class AchievmentQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.ACHIEVEMENT_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> achievmentQuestWeights = new Dictionary<string, Func<int, int>>();
                achievmentQuestWeights.Add( Constants.GATHER_QUEST, Constants.TwoOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EXCHANGE_QUEST, Constants.TwoOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EMPTY_QUEST, Constants.TwoOptionQuestEmptyWeight );
                return achievmentQuestWeights;
            } 
        }
    }
}