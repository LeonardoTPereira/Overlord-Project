using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.ExperimentControllers;
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
                if ( _nextSymbolChances != null )
                    return _nextSymbolChances;
                    
                var achievementQuestWeights = new Dictionary<string, Func<int, int>>
                {
                    {Constants.GATHER_QUEST, Constants.OneOptionQuestLineWeight},
                    //{Constants.EXCHANGE_QUEST, Constants.TwoOptionQuestLineWeight},
                    {Constants.EMPTY_QUEST, Constants.TwoOptionQuestEmptyWeight}
                };
                return achievementQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, in GeneratorSettings generatorSettings)
        {
            switch ( SymbolType )
            {
                case Constants.GATHER_QUEST:
                    return CreateAndSaveGatherQuestSo(questSos, generatorSettings.PlaceholderItems);
                case Constants.EXCHANGE_QUEST:
                    return CreateAndSaveExchangeQuestSo(questSos, generatorSettings.PlaceholderNpcs, generatorSettings.PlaceholderItems);
                default:
                    Debug.LogError("help something went wrong! - Achievement doesn't contain symbol: "+SymbolType);
                break;
            }

            return null;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void CreateQuestString()
        {
            throw new NotImplementedException();
        }

        private static GatherQuestSo CreateAndSaveGatherQuestSo( List<QuestSo> questSos, TreasureRuntimeSetSO possibleItems)
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
            return getItemQuest;
        }

        private static ExchangeQuestSo CreateAndSaveExchangeQuestSo( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems)
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
            return exchangeQuest;

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