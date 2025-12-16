using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyBox;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
//using Game.GameManager;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class AchievementQuestSo : QuestSo
    {
        public override string SymbolType => Constants.AchievementQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get {
                if ( _nextSymbolChances != null )
                    return _nextSymbolChances;
                    
                var achievementQuestWeights = new Dictionary<string, Func<int, float>>
                {
                    {Constants.GatherQuest, Constants.TwoOptionQuestLineWeight},
                    {Constants.ExchangeQuest, Constants.TwoOptionQuestLineWeight},
                    {Constants.EmptyQuest, Constants.TwoOptionQuestEmptyWeight}
                };
                return achievementQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, NpcSo npcInCharge, in NarrativeSettings narrativeSettings, Language language)
        {
            switch ( SymbolType )
            {
                case Constants.GatherQuest:
                    return CreateAndSaveGatherQuestSo(questSos, narrativeSettings.Gemstones, narrativeSettings.ItemsToGather, npcInCharge, language);
                case Constants.ExchangeQuest:
                    return CreateAndSaveExchangeQuestSo(questSos, narrativeSettings.PlaceholderNpcs, narrativeSettings.Gemstones, narrativeSettings.Tools, npcInCharge, language);
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

        public override void CreateQuestString(Language l)
        {
            throw new NotImplementedException();
        }

        private static GatherQuestSo CreateAndSaveGatherQuestSo( List<QuestSo> questSos, TreasureRuntimeSetSo possibleItems, RangedInt itemRange, NpcSo npcInCharge, Language language)
        {
            var getItemQuest = CreateInstance<GatherQuestSo>();
            var selectedItems = new ItemAmountDictionary();
            var questId = getItemQuest.GetInstanceID();
            var selectedItem = possibleItems.GetRandomItem();
            var nItemsToCollect = RandomSingleton.GetInstance().Random.Next(itemRange.Max - itemRange.Min) + itemRange.Min;
            for (var i = 0; i < nItemsToCollect; i++)
            {
                selectedItems.AddItemWithId(selectedItem, questId);
            }
            getItemQuest.Init(ItemsToString(selectedItems, language), false, questSos.Count > 0 
                ? questSos[^1] : null, selectedItems);
            getItemQuest.NpcInCharge = npcInCharge;            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = getItemQuest;
            }
            questSos.Add(getItemQuest);
            return getItemQuest;
        }

        private static ExchangeQuestSo CreateAndSaveExchangeQuestSo( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, 
            TreasureRuntimeSetSo itemsToGive, TreasureRuntimeSetSo itemsToReceive, NpcSo npcInCharge, Language language)
        {
            var exchangeQuest = CreateInstance<ExchangeQuestSo>();
            var exchangedItems = new ItemAmountDictionary();
            var questId = exchangeQuest.GetInstanceID();
            var selectedItem = itemsToGive.GetRandomItem();
            exchangedItems.AddItemWithId(selectedItem, questId);
            var receivedItem = itemsToReceive.GetRandomItem();

            var npcCopy = new List<NpcSo>();
            npcCopy.AddRange( possibleNpcSos );
            npcCopy.Remove(npcInCharge);

            var selectedNpc = npcCopy.GetRandom();
                        
            if (language == Language.Portuguese)
                exchangeQuest.Init($"Troque o item {selectedItem} com {selectedNpc} para receber uma recompensa!", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, exchangedItems, receivedItem);
            else if (language == Language.English)
                exchangeQuest.Init($"Exchange {selectedItem} with {selectedNpc} for a reward!", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, exchangedItems, receivedItem);
            exchangeQuest.NpcInCharge = npcInCharge;

            if (questSos.Count > 0)
            {
                questSos[^1].Next = exchangeQuest;
            }

            questSos.Add(exchangeQuest);
            return exchangeQuest;

        }

        private static string ItemsToString(ItemAmountDictionary selectedItems, Language language)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedItems.Count; i++)
            {
                var itemAmountPair = selectedItems.ElementAt(i);

                if (language == Language.Portuguese)
                {
                    stringBuilder.Append($"Junte {itemAmountPair.Value} {itemAmountPair.Key}");
                    if (itemAmountPair.Value.QuestIds.Count > 1)
                    {
                        stringBuilder.Append("s");
                    }

                    if (i < (selectedItems.Count - 1))
                    {
                        stringBuilder.Append(" e ");
                    }
                }
                else if (language == Language.English)
                {
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
            }
            return stringBuilder.ToString();
        }

        public virtual ItemAmountDictionary GetItemDictionary()
        {
            throw new NotImplementedException();
        }
    }
}