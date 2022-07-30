using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
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
                AddItemWhenItemQuest(quest);
            }
        }

        private void AddItemWhenItemQuest(QuestSo quest)
        {
            if (quest.IsItemQuest())
            {
                AddItems((ItemQuestSo) quest);
            }
        }

        private void AddItems(ItemQuestSo quest)
        {
            foreach (var dropItemData in quest.ItemsToCollectByType)
            {
                AddItemsFromPairToDictionary(dropItemData);
            }

        }

        private void AddItemsFromPairToDictionary(KeyValuePair<ItemSo, int> itemData)
        {
            var newItems = itemData.Value;
            TotalItems += newItems;
            TotalItemValue += itemData.Key.Value;
            if (ItemsByType.ItemAmountBySo.TryGetValue(itemData.Key, out var enemiesForItem))
            {
                ItemsByType.ItemAmountBySo[itemData.Key] = enemiesForItem + newItems;
            }
            else
            {
                ItemsByType.ItemAmountBySo.Add(itemData.Key, newItems);
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
