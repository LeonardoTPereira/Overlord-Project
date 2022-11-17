using System;
using System.Collections.Generic;
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

        public void CalculateItemsFromQuests(IEnumerable<QuestLine> questLines)
        {
            foreach (var quest in questLines.SelectMany(questLine => questLine.Quests))
            {
                AddItemWhenAchievementQuest(quest);
                AddItemWhenImmersionQuest(quest);
            }
        }

        private void AddItemWhenAchievementQuest(QuestSo quest)
        {
            var achievementQuestSo = quest as AchievementQuestSo;
            if (achievementQuestSo != null)
            {
                AddAchievementItems(achievementQuestSo);
            }
        }
        
        private void AddItemWhenImmersionQuest(QuestSo quest)
        {
            var immersionQuestSo = quest as ImmersionQuestSo;
            if (immersionQuestSo != null)
            {
                AddImmersionItems(immersionQuestSo);
            }
        }

        private void AddAchievementItems(AchievementQuestSo quest)
        {
            var itemDictionary = quest.GetItemDictionary();
            foreach (var itemData in itemDictionary)
            {
                foreach (var questId in itemData.Value.QuestIds)
                {
                    ItemsByType.ItemAmountBySo.AddItemWithId(itemData.Key, questId);
                    TotalItems++;
                    TotalItemValue += itemData.Key.Value;
                }            
            }
        }
        
        private void AddImmersionItems(ImmersionQuestSo quest)
        {
            switch (quest)
            {
                case GiveQuestSo giveQuestSo:
                    ItemsByType.ItemAmountBySo.AddItemWithId(giveQuestSo.GiveQuestData.ItemToGive, giveQuestSo.Id);
                    TotalItems++;
                    TotalItemValue += giveQuestSo.GiveQuestData.ItemToGive.Value;
                    break;
                case ReadQuestSo readQuestSo:
                    ItemsByType.ItemAmountBySo.AddItemWithId(readQuestSo.ItemToRead, readQuestSo.Id);
                    TotalItems++;
                    TotalItemValue += readQuestSo.ItemToRead.Value;
                    break;
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
