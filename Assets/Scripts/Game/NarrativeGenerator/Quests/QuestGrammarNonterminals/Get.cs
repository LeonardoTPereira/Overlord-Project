using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public class Get : NonTerminalQuest
    { 
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> getSymbolWeights = new Dictionary<string, Func<int, int>>();
                getSymbolWeights.Add( Constants.GET_TERMINAL, x => (int)Mathf.Clamp( 1/(x*0.25f), 0, 100) );
                getSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => (int)Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 100));
                return getSymbolWeights;
            } 
            set {}
        }
        public void DefineQuestSO ( MarkovChain chain, List<QuestSO> questSos, List<NpcSO> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( chain.GetLastSymbol().symbolType )
            {
                case Constants.GET_TERMINAL:
                    CreateAndSaveGetQuestSo(questSos, possibleItems);
                break;
                case Constants.DROP_TERMINAL:
                    CreateAndSaveDropQuestSo(questSos, possibleItems, enemyTypes);
                break;
            }
        }

        private static void CreateAndSaveGetQuestSo(List<QuestSO> questSos,
            TreasureRuntimeSetSO possibleItems)
        {
            var getItemQuest = ScriptableObject.CreateInstance<ItemQuestSo>();
            var selectedItems = new Dictionary<ItemSo, int>();
            //TODO select more items
            var selectedItem = possibleItems.GetRandomItem();
            selectedItems.Add(selectedItem, 1);
            getItemQuest.Init(ItemsToString(selectedItems), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedItems);
            getItemQuest.SaveAsAsset();
            questSos.Add(getItemQuest);

        }

        private static void CreateAndSaveDropQuestSo(List<QuestSO> questSos,
            TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var dropQuest = ScriptableObject.CreateInstance<DropQuestSo>();

            var dropItemData = new Dictionary<ItemSo, EnemiesByType >();

            //TODO select more items
            var selectedItem = possibleItems.GetRandomItem();
            var selectedEnemyTypes = new EnemiesByType ();
            //TODO select more enemies
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(7) + 5;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            dropItemData.Add(selectedItem, selectedEnemyTypes);
            dropQuest.Init(DropItemsToString(dropItemData), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, dropItemData);
            dropQuest.SaveAsAsset();
            questSos.Add(dropQuest);
        }

        private static string ItemsToString(Dictionary<ItemSo, int> selectedItems)
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
    
        private static string DropItemsToString(Dictionary<ItemSo, EnemiesByType > dropItems)
        {
            var stringBuilder = new StringBuilder();
            foreach (var itemToDrop in dropItems)
            {
                stringBuilder.Append($"$Get {itemToDrop.Key} From ");
                foreach (var itemsPerEnemy in itemToDrop.Value.EnemiesByTypeDictionary)
                {
                    stringBuilder.Append($"{itemsPerEnemy.Value} {itemsPerEnemy.Key}");
                }
            }
            return stringBuilder.ToString();
        }
    }
}