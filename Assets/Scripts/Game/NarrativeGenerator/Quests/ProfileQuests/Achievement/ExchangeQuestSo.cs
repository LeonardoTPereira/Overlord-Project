using ScriptableObjects;
using Util;
using System;
using System.Text;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using System.Collections.Generic;
using Game.Dialogues;
using Game.Events;
using UnityEngine;
using Game.NPCs;
using Game.Quests;


namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExchangeQuestSo : AchievementQuestSo
    {

        public override string SymbolType => Constants.ExchangeQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }
        
        public override ItemAmountDictionary GetItemDictionary()
        {
            return ItemsToExchangeByType;
        }

        [field: SerializeField] public ItemAmountDictionary ItemsToExchangeByType { get; set; }
        private ItemAmountDictionary CopyOfItemsToTrade { get; set; }
        public ItemSo ReceivedItem { get; set; }
        public NpcSo Npc { get; set; }
        public static event ItemTradeEvent ItemTradeEventHandler;
        public bool HasItems { get; private set; }


        private void OnEnable()
        {
            TaggedDialogueHandler.StartExchangeEventHandler += TradeItems;
        }

        private void OnDisable()
        {
            TaggedDialogueHandler.StartExchangeEventHandler -= TradeItems;
        }

        public override void Init()
        {
            base.Init();
            ItemsToExchangeByType = new ItemAmountDictionary();
            ReceivedItem = null;
            Npc = null;
            HasItems = false;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var exchangeQuest = copiedQuest as ExchangeQuestSo;
            if (exchangeQuest != null)
            {
                Npc = exchangeQuest.Npc;
                ReceivedItem = exchangeQuest.ReceivedItem;
                ItemsToExchangeByType = (ItemAmountDictionary) exchangeQuest.ItemsToExchangeByType.Clone();
                CopyOfItemsToTrade = exchangeQuest.CopyOfItemsToTrade;
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
            return questElement switch
            {
                ItemSo itemSo => !IsCompleted && !HasItems && ItemsToExchangeByType.ContainsKey(itemSo),
                NpcSo npcSo => !IsCompleted && HasItems && npcSo == Npc,
                _ => false
            };
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            if (HasItems)
            {
                IsCompleted = true;
                return;
            }
            ItemsToExchangeByType.RemoveItemWithId(questElement as ItemSo, questId);
            if (ItemsToExchangeByType.Count == 0)
            {
                HasItems = true;
            }
        }

        public override void CreateQuestString()
        {
            var stringBuilder = new StringBuilder();
            CopyOfItemsToTrade = (ItemAmountDictionary) ItemsToExchangeByType.Clone();
            string spriteString;
            foreach (var itemByAmount in ItemsToExchangeByType)
            {                
                spriteString = itemByAmount.Key.GetGemstoneSpriteString();
                stringBuilder.Append($"{itemByAmount.Value.QuestIds.Count} {itemByAmount.Key.ItemName}s {spriteString}, ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append($" with {Npc.NpcName}.\n");
            
            spriteString = ReceivedItem.GetToolSpriteString();
            stringBuilder.Append($"They'll give you a {ReceivedItem.ItemName} {spriteString}!");
            
            QuestText = stringBuilder.ToString();
        }

        private void TradeItems(object sender, StartExchangeEventArgs eventArgs)
        {
            if (eventArgs.ExchangeQuestId != Id) return;
            ItemTradeEventHandler?.Invoke(this, new ItemTradeEventArgs(CopyOfItemsToTrade, ReceivedItem, Id));
            IsCompleted = true;
        }
    }
}