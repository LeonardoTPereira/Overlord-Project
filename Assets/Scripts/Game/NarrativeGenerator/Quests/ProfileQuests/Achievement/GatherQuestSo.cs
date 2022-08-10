using ScriptableObjects;
using Util;
using System;
using System.Text;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GatherQuestSo : AchievementQuestSo
    {
        [field: SerializeField] public ItemAmountDictionary ItemsToGatherByType { get; set; }
        public override string SymbolType {
            get { return Constants.GATHER_QUEST; }
        }

        public override ItemAmountDictionary GetItemDictionary()
        {
            return ItemsToGatherByType;
        }

        public override void Init()
        {
            base.Init();
            ItemsToGatherByType = new ItemAmountDictionary();
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var getQuest = copiedQuest as GatherQuestSo;
            if (getQuest != null)
            {
                ItemsToGatherByType = getQuest.ItemsToGatherByType.Clone();
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ExchangeQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, ItemAmountDictionary itemsByType)
        {
            base.Init(questName, endsStoryLine, previous);
            ItemsToGatherByType = itemsByType;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GatherQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }


        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   && ItemsToGatherByType.ContainsKey(questElement as ItemSo ?? throw new InvalidOperationException());
        }
        
        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            ItemsToGatherByType.RemoveItemWithId(questElement as ItemSo, questId);
            if (ItemsToGatherByType.Count == 0)
            {
                IsCompleted = true;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var itemByAmount in ItemsToGatherByType)
            {
                stringBuilder.Append($"{itemByAmount.Value.Count} {itemByAmount.Key.ItemName}s, ");
            }
            stringBuilder.Remove(stringBuilder.Length - 3, 2);
            return stringBuilder.ToString();
        }
    }
}