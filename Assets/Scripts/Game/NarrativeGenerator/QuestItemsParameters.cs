using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [Serializable]
    public class QuestItemsParameters
    {
        [field: SerializeField]
        public Dictionary<ItemSo, int> ItemsByType { get; }
        [field: SerializeField]
        public int TotalItems { get; set; }

        public QuestItemsParameters()
        {
            ItemsByType = new Dictionary<ItemSo, int>();
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
            if (IsItemQuest(quest))
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
            if (ItemsByType.TryGetValue(itemData.Key, out var enemiesForItem))
            {
                ItemsByType[itemData.Key] = enemiesForItem + newItems;
            }
            else
            {
                ItemsByType.Add(itemData.Key, newItems);
            }
        }
        
        private static bool IsItemQuest(QuestSO quest)
        {
            return quest.GetType().IsAssignableFrom(typeof(ItemQuestSo));
        }
    }


}
