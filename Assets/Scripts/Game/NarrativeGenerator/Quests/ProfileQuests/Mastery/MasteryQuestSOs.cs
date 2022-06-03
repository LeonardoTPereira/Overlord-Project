using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class MasteryQuestSOs : QuestSO
    {
        public override string symbolType {
            get { return Constants.KILL_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> masteryQuestWeights = new Dictionary<string, Func<int, int>>();
                masteryQuestWeights.Add( Constants.DEFEND, Constants.FourOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.KILL, Constants.FourOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.CAPTURE, Constants.FourOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.DAMAGE, Constants.FourOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }
    }
}
