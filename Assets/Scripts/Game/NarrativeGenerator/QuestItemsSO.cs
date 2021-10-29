using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Items")]
    public class QuestItemsSO : ScriptableObject
    {
        public Dictionary<ItemSO, int> ItemsByType { get; }
        public int NItems { get; set; }

        public void CalculateItemsFromQuests(QuestList quests)
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
                AddItems((ItemQuestSO) quest);
            }
        }

        private void AddItems(ItemQuestSO quest)
        {
            foreach (var dropItemData in quest.ItemsToCollectByType)
            {
                AddItemsFromPairToDictionary(dropItemData);
            }

        }

        private void AddItemsFromPairToDictionary(KeyValuePair<ItemSO, int> itemData)
        {
            int newItems = itemData.Value;
            NItems += newItems;
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
            return quest.GetType().IsAssignableFrom(typeof(ItemQuestSO));
        }
    }


}
