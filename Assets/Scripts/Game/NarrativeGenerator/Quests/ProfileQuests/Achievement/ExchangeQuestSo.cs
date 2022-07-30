using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExchangeQuestSo : AchievementQuestSo
    {
        //No NPCSo directly. It must be only the job/race, defined using some method based on the next quest
        public NpcSo Npc { get; set; }
        [field: SerializeField] public ItemAmountDictionary ItemsToCollectByType { get; set; }

        public override string symbolType {
            get { return Constants.EXCHANGE_QUEST; }
        }
    }
}