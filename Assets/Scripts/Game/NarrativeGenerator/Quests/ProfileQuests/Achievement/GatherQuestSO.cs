using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GatherQuestSO : AchievementQuestSO
    {
        public override string symbolType {
            get { return Constants.GATHER_QUEST; }
        }

        public void Init(string name, bool endsStoryLine, QuestSO previous, ItemAmountDictionary itemsByType)
        {
            base.Init(name, endsStoryLine, previous);
            // ItemsToCollectByType = itemsByType;
        }
    }
}