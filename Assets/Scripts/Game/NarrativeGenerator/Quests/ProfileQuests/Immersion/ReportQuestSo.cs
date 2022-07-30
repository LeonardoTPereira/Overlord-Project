using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReportQuestSo : ImmersionQuestSo
    {
        public override string symbolType {
            get { return Constants.REPORT_QUEST; }
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            // Npc = npc;
        }
    }
}