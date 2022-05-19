using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/DropQuest"), Serializable]
    public class DropQuestSo : ItemQuestSo
    {
        public Dictionary<EnemySO, float> DropChanceByEnemyType { get; set; }
        //TODO change this. This is confusing and is not accessed as intended.
        public Dictionary<ItemSo, EnemiesByType > ItemData { get; set; }
        public Dictionary<ItemSo, Dictionary<float, int>> ItemDataByEnemyFitness { get; set; }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get
            {
                Dictionary<string, Func<int, int>> getSymbolWeights = new Dictionary<string, Func<int, int>>();
                Debug.Log( Constants.ThreeOptionQuestLineWeight(1) );
                Debug.Log( Constants.ThreeOptionQuestEmptyWeight(1) );
                getSymbolWeights.Add( Constants.ITEM_TERMINAL, Constants.ThreeOptionQuestLineWeight );
                getSymbolWeights.Add( Constants.DROP_TERMINAL, Constants.ThreeOptionQuestLineWeight );
                getSymbolWeights.Add( Constants.GET_TERMINAL, Constants.ThreeOptionQuestLineWeight );
                getSymbolWeights.Add( Constants.EMPTY_TERMINAL, Constants.ThreeOptionQuestEmptyWeight );
                return getSymbolWeights;
            } 
            set {}
        }

        public override void Init()
        {
            base.Init();
            ItemData = new Dictionary<ItemSo, EnemiesByType >();
            ItemDataByEnemyFitness = new Dictionary<ItemSo, Dictionary<float, int>>();
        }
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSo, EnemiesByType > dropItemData)
        {
            var itemsByType = new Dictionary<ItemSo, int>();
            foreach (var itemToDrop in dropItemData)
            {
                var totalItems = itemToDrop.Value.EnemiesByTypeDictionary.Sum(itemsPerEnemy => itemsPerEnemy.Value);
                itemsByType.Add(itemToDrop.Key, totalItems);
            }
            base.Init(questName, endsStoryLine, previous, itemsByType);
            ItemData = dropItemData;
        }        
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSo, Dictionary<float, int>> dropItemData)
        {
            Dictionary<ItemSo, int> itemsByType = new Dictionary<ItemSo, int>();
            foreach (var itemToDrop in dropItemData)
            {
                int totalItems = 0;
                foreach (var itemsPerEnemy in itemToDrop.Value)
                {
                    totalItems += itemsPerEnemy.Value;
                }
                itemsByType.Add(itemToDrop.Key, totalItems);
            }
            base.Init(questName, endsStoryLine, previous, itemsByType);
            ItemDataByEnemyFitness = dropItemData;
        }
    }
}
