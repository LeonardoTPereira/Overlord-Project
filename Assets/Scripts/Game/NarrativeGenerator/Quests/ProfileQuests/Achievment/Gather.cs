using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class Gather : AchievmentQuestSOs
    {
        public override string symbolType {
            get { return Constants.GATHER; }
        }
    }
}
