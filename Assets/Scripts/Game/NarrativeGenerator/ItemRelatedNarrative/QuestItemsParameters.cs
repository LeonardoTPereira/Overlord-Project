using System;
using System.Linq;
using Game.NarrativeGenerator.Quests;
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
                foreach (var questId in dropItemData.Value)
                {
                    dropItemData.AddItemWithId(dropItemData.Key, questId);
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
