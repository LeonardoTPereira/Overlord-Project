using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class CreativityQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.CREATIVITY_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> creativityQuestWeights = new Dictionary<string, Func<int, int>>();
                creativityQuestWeights.Add( Constants.EXPLORE_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.GOTO_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return creativityQuestWeights;
            } 
        }
    }
}