﻿using System;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.NarrativeGenerator.ItemRelatedNarrative
{
    [Serializable]
    public class QuestItemsParameters
    {
        [field: SerializeField]
        public ItemsAmount ItemsByType { get; set; }
        [field: SerializeField]
        public int TotalItems { get; set; }
        public int TotalItemValue { get; set; }

        public QuestItemsParameters()
        {
            ItemsByType = new ItemsAmount();
            TotalItems = 0;
            TotalItemValue = 0;
        }

        public void CalculateItemsFromQuests(QuestLine quests)
        {
            foreach (var quest in quests.questLines.SelectMany(questLine => questLine.Quests))
            {
                AddItemWhenAchievementQuest(quest);
            }
        }

        private void AddItemWhenAchievementQuest(QuestSo quest)
        {
            var achievementQuestSo = quest as AchievementQuestSo;
            if (achievementQuestSo != null)
            {
                AddItems(achievementQuestSo);
            }
        }

        private void AddItems(AchievementQuestSo quest)
        {
            var itemDictionary = quest.GetItemDictionary();
            foreach (var dropItemData in itemDictionary)
            {
                foreach (var questId in dropItemData.Value)
                {
                    ItemsByType.ItemAmountBySo.AddItemWithId(dropItemData.Key, questId);
                }            
            }
        }

        public void DebugPrint()
        {
            Debug.Log("Item parameters:");
            foreach (var itemAmountPair in ItemsByType.ItemAmountBySo)
            {
                Debug.Log($"Item={itemAmountPair.Key.name}, Amount={itemAmountPair.Value}");
            }
        }
    }
}
