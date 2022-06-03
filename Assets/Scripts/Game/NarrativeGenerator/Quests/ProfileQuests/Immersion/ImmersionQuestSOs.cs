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
            get { return Constants.EXPLORE_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            
            get {
                Dictionary<string, Func<int, int>> immersionQuestWeights = new Dictionary<string, Func<int, int>>();
                immersionQuestWeights.Add( Constants.STEALTH, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.SPY, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.ESCORT, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.GOTO, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return immersionQuestWeights;
            } 
        }
    }
}
