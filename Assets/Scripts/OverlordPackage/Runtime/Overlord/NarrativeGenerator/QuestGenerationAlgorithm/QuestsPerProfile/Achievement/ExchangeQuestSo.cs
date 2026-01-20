using ScriptableObjects;
using Util;
using System;
using System.Text;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
using System.Collections.Generic;
using UnityEngine;
//using Game.GameManager;
using System.Linq;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
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

        public ExchangeQuestData ExchangeData { get; set; }
        [field: SerializeField] public ItemAmountDictionary ItemsToExchangeByType { get; set; }
        private ItemAmountDictionary OriginalItemsToExchangeByType;
        
        public NpcSo Npc { get; set; }
        public bool HasItems { get; private set; }
        public bool HasCreatedDialogue { get; set; }

        public override void Init()
        {
            base.Init();
            ItemsToExchangeByType = new ItemAmountDictionary();
            OriginalItemsToExchangeByType = (ItemAmountDictionary)ItemsToExchangeByType.Clone();
            Npc = null;
            HasItems = false;
            HasCreatedDialogue = false;
            ExchangeData = new ExchangeQuestData();
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var exchangeQuest = copiedQuest as ExchangeQuestSo;
            if (exchangeQuest != null)
            {
                Npc = exchangeQuest.Npc;
                ItemsToExchangeByType = (ItemAmountDictionary) exchangeQuest.ItemsToExchangeByType.Clone();
                OriginalItemsToExchangeByType = (ItemAmountDictionary)ItemsToExchangeByType.Clone();
                ExchangeData = exchangeQuest.ExchangeData;
                HasItems = exchangeQuest.HasItems;
                HasCreatedDialogue = exchangeQuest.HasCreatedDialogue;
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
            OriginalItemsToExchangeByType = (ItemAmountDictionary)ItemsToExchangeByType.Clone();
            ExchangeData =
                new ExchangeQuestData((ItemAmountDictionary) ItemsToExchangeByType.Clone(), receivedItem, Id);
            HasItems = false;
            HasCreatedDialogue = false;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ExchangeQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            if (questId != Id) return false;
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

        public override string GetTargetNpc()
        {
            return Npc.NpcName;
        }

        public override string GetItemAmountString(Util.Enums.Language l)
        {
            var stringBuilder = new StringBuilder();
            string spriteString;
            foreach (var itemByAmount in OriginalItemsToExchangeByType)
            {                
                spriteString = itemByAmount.Key.GetGemstoneSpriteString();
                stringBuilder.Append($"{itemByAmount.Value.QuestIds.Count} {itemByAmount.Key.ItemName}s {spriteString}, ");
            }
            if ( stringBuilder.Length > 2 )
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }

        public override string GetItemString()
        {
            string itemsToTrade = "";
            foreach (var itemsCopy in ExchangeData.CopyOfItemsToTrade)
            {
                itemsToTrade += itemsCopy.Key.ItemName;
                if ( !itemsCopy.Equals( ExchangeData.CopyOfItemsToTrade.LastOrDefault() ) &&  ExchangeData.CopyOfItemsToTrade.Count > 2 )
                {
                    itemsToTrade += ", ";
                }
                else if ( !itemsCopy.Equals( ExchangeData.CopyOfItemsToTrade.LastOrDefault() ) )
                {
                    itemsToTrade += " and ";
                }
            }
            return itemsToTrade;
        }

        public override void CreateQuestString(Language language)
        {
            var stringBuilder = new StringBuilder();

            if (language == Language.Portuguese)
            {
                stringBuilder.Append("Troque ");
            }
            else
            {
                stringBuilder.Append("Trade ");
            }

            foreach (var itemByAmount in OriginalItemsToExchangeByType)
            {                
                stringBuilder.Append($"{itemByAmount.Value.QuestIds.Count} {itemByAmount.Key.ItemName}s, ");
            }
            if (stringBuilder.Length > 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            if (language == Language.Portuguese)
            {
                stringBuilder.Append($" com {Npc.NpcName}.\n");
                stringBuilder.Append($"Você receberá dele o {ExchangeData.ReceivedItem.ItemName}");
            }
            else
            {
                stringBuilder.Append($" with {Npc.NpcName}.\n");
                stringBuilder.Append($"They'll give you a {ExchangeData.ReceivedItem.ItemName}!");
            }
            
            QuestText = stringBuilder.ToString();
        }
    }
}