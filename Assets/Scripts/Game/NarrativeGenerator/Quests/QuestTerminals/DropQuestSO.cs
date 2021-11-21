using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/DropQuest"), Serializable]
    class DropQuestSO : ItemQuestSO
    {
        public Dictionary<EnemySO, float> DropChanceByEnemyType { get; set; }
        public Dictionary<ItemSO, Dictionary<EnemySO, int>> ItemData { get; set; }
        public Dictionary<ItemSO, Dictionary<float, int>> ItemDataByEnemyFitness { get; set; }
        
        public DropQuestSO ()
        {
            symbolType = Constants.DROP_TERMINAL;
            //canDrawNext = true;
        }

        public override void Init()
        {
            base.Init();
            ItemData = new Dictionary<ItemSO, Dictionary<EnemySO, int>>();
            ItemDataByEnemyFitness = new Dictionary<ItemSO, Dictionary<float, int>>();
        }
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSO, Dictionary<EnemySO, int>> dropItemData)
        {
            Dictionary<ItemSO, int> itemsByType = new Dictionary<ItemSO, int>();
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
            ItemData = dropItemData;
        }        
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSO, Dictionary<float, int>> dropItemData)
        {
            Dictionary<ItemSO, int> itemsByType = new Dictionary<ItemSO, int>();
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
