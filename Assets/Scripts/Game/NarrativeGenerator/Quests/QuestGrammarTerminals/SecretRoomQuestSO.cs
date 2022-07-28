using System.Collections.Generic;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class SecretRoomQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.SECRET_QUEST; }
        }
        public Dictionary<EnemySO, int> EnemiesToKillByType { get; set; }
        private Dictionary<ItemSo, int> ItemsToCollectByType { get; set; }
        private NpcSo Npc { get; set; }

        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new Dictionary<EnemySO, int>();
            ItemsToCollectByType = new Dictionary<ItemSo, int>();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<EnemySO, int> enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
        }
        
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<ItemSo, int> itemsByType)
        {
            base.Init(questName, endsStoryLine, previous);
            ItemsToCollectByType = itemsByType;
        }
        
        public void Init(string questName, bool endsStoryLine, QuestSO previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
        }
        
        public void AddEnemy(EnemySO enemy, int amount)
        {
            if (EnemiesToKillByType.TryGetValue(enemy, out var currentAmount))
            {
                EnemiesToKillByType[enemy] = currentAmount + amount;
            }
            else
            {
                EnemiesToKillByType.Add(enemy, amount);
            }
        }
        public void AddItem(ItemSo item, int amount)
        {
            if (ItemsToCollectByType.TryGetValue(item, out var currentAmount))
            {
                ItemsToCollectByType[item] = currentAmount + amount;
            }
            else
            {
                ItemsToCollectByType.Add(item, amount);
            }
        }
        
        public void SetNpc(NpcSo npc)
        {
            Npc = npc;
        }
        
        public override void Init(QuestSO copiedQuest)
        {
            base.Init(copiedQuest);
            Debug.Log("Init Not Implemented for SecretRoomQuestSO");
        }
        
        public override QuestSO Clone()
        {
            var cloneQuest = CreateInstance<SecretRoomQuestSO>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}
