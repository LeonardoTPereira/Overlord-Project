using ScriptableObjects;
using Util;
using System;
using System.Text;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;
using static Util.Enums;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GatherQuestSo : AchievementQuestSo
    {
        [field: SerializeField] public ItemAmountDictionary ItemsToGatherByType { get; set; }
        private ItemAmountDictionary OriginalItemsToGatherByType;
        public override string SymbolType => Constants.GatherQuest;

        public override ItemAmountDictionary GetItemDictionary()
        {
            return ItemsToGatherByType;
        }

        public override void Init()
        {
            base.Init();
            ItemsToGatherByType = new ItemAmountDictionary();
            OriginalItemsToGatherByType = (ItemAmountDictionary)ItemsToGatherByType.Clone();
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var getQuest = copiedQuest as GatherQuestSo;
            if (getQuest != null)
            {
                ItemsToGatherByType = (ItemAmountDictionary) getQuest.ItemsToGatherByType.Clone();
                OriginalItemsToGatherByType = (ItemAmountDictionary)ItemsToGatherByType.Clone();
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
            OriginalItemsToGatherByType = (ItemAmountDictionary)ItemsToGatherByType.Clone();
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GatherQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override string GetItemAmountString(Language language)
        {
            var stringBuilder = new StringBuilder();
            if (language == Language.Portuguese)
            {
                stringBuilder.Append("Colete um(a)");
            }
            else
            {
                stringBuilder.Append("Collect a");
            }
            
            foreach (var itemByAmount in OriginalItemsToGatherByType)
            {
                var spriteString = itemByAmount.Key.GetGemstoneSpriteString();
                stringBuilder.Append($" {itemByAmount.Key.ItemName} {spriteString}, ");
            }
            if ( stringBuilder.Length > 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            return stringBuilder.ToString();
        }

        public override string GetItemString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var itemByAmount in OriginalItemsToGatherByType)
            {
                var spriteString = itemByAmount.Key.GetGemstoneSpriteString();
                stringBuilder.Append($"{itemByAmount.Key.ItemName}s {spriteString}, ");
            }
            if ( stringBuilder.Length > 2 )
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            return stringBuilder.ToString();
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            if (questId != Id) return false;
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

        public override void CreateQuestString(Util.Enums.Language l)
        {
            QuestText = this.GetItemAmountString(l);
        }
    }
}