using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
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

        public QuestItemsParameters()
        {
            ItemsByType = new ItemsAmount();
            TotalItems = 0;
        }

        public void CalculateItemsFromQuests(QuestLine quests)
        {
            foreach (var quest in quests.graph)
            {
                AddItemWhenItemQuest(quest);
            }
        }

        private void AddItemWhenItemQuest(QuestSO quest)
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
            int newItems = itemData.Value;
            TotalItems += newItems;
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
