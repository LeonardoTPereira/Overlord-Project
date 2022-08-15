using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.NPCs;
using MyBox;
using Game.NarrativeGenerator.ItemRelatedNarrative;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class AchievementQuestSo : QuestSo
    {
        public override string SymbolType => Constants.AchievementQuest;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get {
                if ( nextSymbolChances != null )
                    return nextSymbolChances;
                    
                var achievementQuestWeights = new Dictionary<string, Func<int, int>>
                {
                    {Constants.GATHER_QUEST, Constants.TwoOptionQuestLineWeight},
                    {Constants.EXCHANGE_QUEST, Constants.TwoOptionQuestLineWeight},
                    {Constants.EMPTY_QUEST, Constants.TwoOptionQuestEmptyWeight}
                };
                return achievementQuestWeights;
            } 
        }

        public override void DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( SymbolType )
            {
                case Constants.GATHER_QUEST:
                    CreateAndSaveGatherQuestSo(questSos, possibleItems);
                break;
                case Constants.EXCHANGE_QUEST:
                    CreateAndSaveExchangeQuestSo(questSos, possibleNpcSos, possibleItems);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        private static void CreateAndSaveGatherQuestSo( List<QuestSo> questSos, TreasureRuntimeSetSO possibleItems)
        {
            var getItemQuest = CreateInstance<GatherQuestSo>();
            var selectedItems = new ItemAmountDictionary();
            var questId = getItemQuest.GetInstanceID();
            var selectedItem = possibleItems.GetRandomItem();
            var nItemsToCollect = RandomSingleton.GetInstance().Random.Next(5) + 5;
            for (var i = 0; i < nItemsToCollect; i++)
            {
                selectedItems.AddItemWithId(selectedItem, questId);
            }
            getItemQuest.Init(ItemsToString(selectedItems), false, questSos.Count > 0 
                ? questSos[^1] : null, selectedItems);
            if (questSos.Count > 0)
            {
                questSos[^1].Next = getItemQuest;
            }
            
            questSos.Add(getItemQuest);
        }

        private static void CreateAndSaveExchangeQuestSo( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems)
        {
            var exchangeQuest = CreateInstance<ExchangeQuestSo>();
            var exchangedItems = new ItemAmountDictionary();
            var selectedItem = possibleItems.GetRandomItem();
            exchangedItems.AddItemWithId(selectedItem, exchangeQuest.Id);
            var receivedItem = possibleItems.GetRandomItem();
            var selectedNpc = possibleNpcSos.GetRandom();

            exchangeQuest.Init($"Exchange {selectedItem} with {selectedNpc} for a reward!", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, exchangedItems, receivedItem);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = exchangeQuest;
            }

            questSos.Add(exchangeQuest);
        }

        private static string ItemsToString(ItemAmountDictionary selectedItems)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedItems.Count; i++)
            {
                var itemAmountPair = selectedItems.ElementAt(i);
                stringBuilder.Append($"$Gather {itemAmountPair.Value} {itemAmountPair.Key}");
                if (itemAmountPair.Value.QuestIds.Count > 1)
                {
                    stringBuilder.Append("s");
                }
                
                if (i < (selectedItems.Count - 1))
                {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }

        public virtual ItemAmountDictionary GetItemDictionary()
        {
            throw new NotImplementedException();
        }
    }
}