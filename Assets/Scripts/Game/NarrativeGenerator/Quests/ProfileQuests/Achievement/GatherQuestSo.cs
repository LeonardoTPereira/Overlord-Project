using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;
using System.Linq;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GatherQuestSo : AchievementQuestSo
    {
        [field: SerializeField] public ItemAmountDictionary ItemsToGatherByType { get; set; }
        public override string symbolType {
            get { return Constants.GATHER_QUEST; }
        }

        public override void Init()
        {
            base.Init();
            ItemsToGatherByType = new ItemAmountDictionary();
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            ItemsToGatherByType = new ItemAmountDictionary();
            foreach (var itemByAmount in (copiedQuest as GatherQuestSo).ItemsToGatherByType)
            {
                ItemsToGatherByType.Add(itemByAmount.Key, itemByAmount.Value);
            }
        }

        public void Init(string name, bool endsStoryLine, QuestSo previous, ItemAmountDictionary itemsByType)
        {
            base.Init(name, endsStoryLine, previous);
            ItemsToGatherByType = itemsByType;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ItemQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public void AddItem(ItemSo item, int amount)
        {
            if (ItemsToGatherByType.TryGetValue(item, out var currentAmount))
            {
                ItemsToGatherByType[item] = currentAmount + amount;
            }
            else
            {
                ItemsToGatherByType.Add(item, amount);
            }
        }
        
        public void SubtractItem(ItemSo itemSo)
        {
            ItemsToGatherByType[itemSo]--;
        }

        public bool CheckIfCompleted()
        {
            return ItemsToGatherByType.All(itemToGather => itemToGather.Value == 0);
        }
        
        public bool HasItemToGather(ItemSo itemSo)
        {
            if (!ItemsToGatherByType.TryGetValue(itemSo, out var itemsLeft)) return false;
            return itemsLeft > 0;
        }
    }
}