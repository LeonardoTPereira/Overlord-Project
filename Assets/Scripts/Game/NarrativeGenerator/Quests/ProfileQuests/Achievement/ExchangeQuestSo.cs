using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Game.NPCs;
using Game.Quests;
using System.Linq;


namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExchangeQuestSo : AchievementQuestSo
    {

        public override string symbolType {
            get { return Constants.EXCHANGE_QUEST; }
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
            Npc = (copiedQuest as ExchangeQuestSo).Npc;
            ReceivedItem = (copiedQuest as ExchangeQuestSo).ReceivedItem;
            ItemsToExchangeByType = new ItemAmountDictionary();
            foreach (var itemByAmount in (copiedQuest as ExchangeQuestSo).ItemsToExchangeByType)
            {
                ItemsToExchangeByType.Add(itemByAmount.Key, itemByAmount.Value);
            }
        }

        public void Init( string name, bool endsStoryLine, QuestSo previous, NpcSo npc, ItemAmountDictionary exchangedItems, ItemSo receivedItem )
        {
            base.Init(name, endsStoryLine, previous);
            Npc = npc;
            ItemsToExchangeByType = exchangedItems;
            ReceivedItem = receivedItem;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ItemQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public static ExchangeQuestSo GetValidExchangeQuest ( QuestGetItemEventArgs getItemQuestArgs, List<QuestList> questLists )
        {
            var itemCollected = getItemQuestArgs.ItemType;
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not ExchangeQuestSo exchangeQuestSo) continue;
                if (exchangeQuestSo.HasItemToExchange(itemCollected)) return exchangeQuestSo;
            }

            foreach (var questList in questLists)
            {
                var exchangeQuestSo = questList.GetFirstExchangeQuestAvailable(itemCollected);
                if (exchangeQuestSo == null) return exchangeQuestSo;
            }

            return null;
        }

        public static ExchangeQuestSo GetValidExchangeQuest ( QuestTalkEventArgs talkQuestArgs, List<QuestList> questLists )
        {
            var npc = talkQuestArgs.Npc;
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not ExchangeQuestSo exchangeQuestSo) continue;
                if (exchangeQuestSo.Npc == npc) return exchangeQuestSo;
            }

            foreach (var questList in questLists)
            {
                var exchangeQuestSo = questList.GetFirstExchangeQuestWithNpc(npc);
                if (exchangeQuestSo == null) return exchangeQuestSo;
            }

            return null;
        }

        public void AddItem(ItemSo item, int amount)
        {
            if (ItemsToExchangeByType.TryGetValue(item, out var currentAmount))
            {
                ItemsToExchangeByType[item] = currentAmount + amount;
            }
            else
            {
                ItemsToExchangeByType.Add(item, amount);
            }
        }
        
        public void SubtractItem(ItemSo itemSo)
        {
            ItemsToExchangeByType[itemSo]--;
        }

        public bool HasItemToExchange(ItemSo itemSo)
        {
            if (!ItemsToExchangeByType.TryGetValue(itemSo, out var itemsLeft)) return false;
            return itemsLeft > 0;
        }

        public bool CheckIfCanComplete()
        {
            return ItemsToExchangeByType.All(itemToExchange => itemToExchange.Value == 0);
        }
    }
}