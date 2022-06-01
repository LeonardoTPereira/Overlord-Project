using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [Serializable]
    public class QuestList
    {
        [field: SerializeField] public List<QuestSO> Quests {get; set; }
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }

        public QuestList()
        {
            Quests = new List<QuestSO>();
        }
        
        public QuestList(QuestList quests)
        {
            Quests = new List<QuestSO>();
            foreach (var quest in quests.Quests)
            {
                var copyQuest = ScriptableObject.CreateInstance<QuestSO>();
                copyQuest.Init(quest);
                Quests.Add(copyQuest);
            }
            NpcInCharge = quests.NpcInCharge;
        }
   

        public KillQuestSO GetFirstKillQuestWithEnemyAvailable(WeaponTypeSO weaponTypeSo)
        {
            foreach (var quest in Quests)
            {
                if (quest is not KillQuestSO killQuestSo) continue;
                var enemyCount = killQuestSo.EnemiesToKillByType.EnemiesByTypeDictionary[weaponTypeSo];
                if (enemyCount > 0)
                {
                    return killQuestSo;
                }
            }
            return null;
        }
        
        public GetQuestSo GetFirstGetItemQuestWithEnemyAvailable(ItemSo itemType)
        {
            foreach (var quest in Quests)
            {
                if (quest is not GetQuestSo getQuestSo) continue;
                var itemCount = getQuestSo.ItemsToCollectByType[itemType];
                if (itemCount > 0)
                {
                    return getQuestSo;
                }
            }
            return null;
        }

        public TalkQuestSO GetFirstTalkQuestWithNpc(NpcSo npc)
        {
            foreach (var quest in Quests)
            {
                if (quest is not TalkQuestSO talkQuestSo) continue;
                if (talkQuestSo.Npc == npc)
                {
                    return talkQuestSo;
                }
            }
            return null;        
        }

        public QuestSO GetCurrentQuest()
        {
            return CurrentQuestIndex >= Quests.Count ? null : Quests[CurrentQuestIndex];
        }
    }
}