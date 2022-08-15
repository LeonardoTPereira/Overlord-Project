using ScriptableObjects;
using Util;
using System;
using System.Text;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using UnityEngine;
using Game.NPCs;


namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExchangeQuestSo : AchievementQuestSo
    {

        public override string SymbolType => Constants.EXCHANGE_QUEST;
        public override ItemAmountDictionary GetItemDictionary()
        {
            return ItemsToExchangeByType;
        }

        [field: SerializeField] public ItemAmountDictionary ItemsToExchangeByType { get; set; }
        public ItemSo ReceivedItem { get; set; }
        public NpcSo Npc { get; set; }

        public override void Init()
        {
            base.Init();
            ItemsToExchangeByType = new ItemAmountDictionary();
            ReceivedItem = null;
            Npc = null;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var exchangeQuest = copiedQuest as ExchangeQuestSo;
            if (exchangeQuest != null)
            {
                Npc = exchangeQuest.Npc;
                ReceivedItem = exchangeQuest.ReceivedItem;
                ItemsToExchangeByType = new ItemAmountDictionary();
                foreach (var itemByAmount in exchangeQuest.ItemsToExchangeByType)
                {
                    ItemsToExchangeByType.Add(itemByAmount.Key, itemByAmount.Value);
                }
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ExchangeQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }

        public void Init( string questName, bool endsStoryLine, QuestSo previous, NpcSo npc, ItemAmountDictionary exchangedItems, ItemSo receivedItem )
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
            ItemsToExchangeByType = exchangedItems;
            ReceivedItem = receivedItem;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ExchangeQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   && ItemsToExchangeByType.ContainsKey(questElement as ItemSo ?? throw new InvalidOperationException());
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            ItemsToExchangeByType.RemoveItemWithId(questElement as ItemSo, questId);
            if (ItemsToExchangeByType.Count == 0)
            {
                IsCompleted = true;
            }
        }
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var itemByAmount in ItemsToExchangeByType)
            {
                stringBuilder.Append($"{itemByAmount.Value.QuestIds.Count} {itemByAmount.Key.ItemName}s, ");
            }
            stringBuilder.Remove(stringBuilder.Length - 3, 2);
            stringBuilder.Append($" with {Npc.NpcName}.\n");
            return stringBuilder.ToString();
        }
    }
}