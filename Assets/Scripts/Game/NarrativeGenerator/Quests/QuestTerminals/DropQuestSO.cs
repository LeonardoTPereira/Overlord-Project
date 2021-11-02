using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/DropQuest"), Serializable]
    class DropQuestSO : ItemQuestSO
    {
        public Dictionary<EnemySO, float> DropChanceByEnemyType { get; set; }
        
        public DropQuestSO ()
        {
            symbolType = SymbolType.drop;
            //canDrawNext = true;
        }

        public override void Init()
        {
            base.Init();
            DropChanceByEnemyType = new Dictionary<EnemySO, float>();
        }
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSO, int> itemsByType, Dictionary<EnemySO, float> dropChanceByEnemyType)
        {
            base.Init(questName, endsStoryLine, previous, itemsByType);
            DropChanceByEnemyType = dropChanceByEnemyType;
        }
    }
}
