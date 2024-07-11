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
        public override string symbolType {
            get { return Constants.EXPLORE_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            
            get {
                Dictionary<string, Func<int, int>> creativityQuestWeights = new Dictionary<string, Func<int, int>>();
                creativityQuestWeights.Add( Constants.STEALTH, Constants.FourOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.SPY, Constants.FourOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.ESCORT, Constants.FourOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.GOTO, Constants.FourOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return creativityQuestWeights;
            } 
        }
    }
}
