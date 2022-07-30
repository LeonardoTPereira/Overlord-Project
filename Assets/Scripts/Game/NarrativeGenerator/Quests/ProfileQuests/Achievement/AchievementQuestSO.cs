using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.NPCs;
using MyBox;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class AchievementQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.ACHIEVEMENT_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> achievmentQuestWeights = new Dictionary<string, Func<int, int>>();
                achievmentQuestWeights.Add( Constants.GATHER_QUEST, Constants.TwoOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EXCHANGE_QUEST, Constants.TwoOptionQuestLineWeight );
                achievmentQuestWeights.Add( Constants.EMPTY_QUEST, Constants.TwoOptionQuestEmptyWeight );
                return achievmentQuestWeights;
            } 
        }

        public override void DefineQuestSO ( MarkovChain chain, List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( chain.GetLastSymbol().symbolType )
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

        public static void CreateAndSaveGatherQuestSo( List<QuestSO> questSos, TreasureRuntimeSetSO possibleItems )
        {
            var getItemQuest = ScriptableObject.CreateInstance<GatherQuestSO>();
            var selectedItems = new ItemAmountDictionary();
            //TODO select more items
            var selectedItem = possibleItems.GetRandomItem();
            selectedItems.Add(selectedItem, 1);
            getItemQuest.Init(ItemsToString(selectedItems), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedItems);
            //getItemQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = getItemQuest;
            }
            
            questSos.Add(getItemQuest);
        }

        public static void CreateAndSaveExchangeQuestSo( List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems)
        {
            var secretRoomQuest = ScriptableObject.CreateInstance<ExchangeQuestSO>();
            secretRoomQuest.Init("Explore Room", false, questSos.Count > 0 ? questSos[^1] : null);
            //TODO initiate data for secretRoomQuest
            if (questSos.Count > 0)
            {
                questSos[^1].Next = secretRoomQuest;
            }

            questSos.Add(secretRoomQuest);
        }

        private static string ItemsToString(ItemAmountDictionary selectedItems)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedItems.Count; i++)
            {
                var itemAmountPair = selectedItems.ElementAt(i);
                stringBuilder.Append($"$Get {itemAmountPair.Value} {itemAmountPair.Key}");
                if (itemAmountPair.Value > 1)
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
    }
}