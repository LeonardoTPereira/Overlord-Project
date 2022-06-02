using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class Spy : ImmersionQuestSOs
    {
        public override string symbolType {
            get { return Constants.SPY; }
        }
    }
}
