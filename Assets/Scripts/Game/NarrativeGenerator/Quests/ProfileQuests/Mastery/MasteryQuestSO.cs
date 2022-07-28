using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class MasteryQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.MASTERY_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> masteryQuestWeights = new Dictionary<string, Func<int, int>>();
                masteryQuestWeights.Add( Constants.KILL_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.DAMAGE_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }
    }
}