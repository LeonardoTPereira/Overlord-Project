using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ImmersionQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.IMMERSION_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> immersionQuestWeights = new Dictionary<string, Func<int, int>>();
                immersionQuestWeights.Add( Constants.LISTEN_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.READ_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.GIVE_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.REPORT_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return immersionQuestWeights;
            } 
        }
    }
}