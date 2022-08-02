using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [Serializable]
    public class QuestList
    {
        [field: SerializeReference] public List<QuestSo> Quests {get; set; }
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }

        public QuestList()
        {
            Quests = new List<QuestSo>();
        }
        
        public QuestList(QuestList quests)
        {
            Quests = new List<QuestSo>();
            foreach (var quest in quests.Quests)
            {
                var copyQuest = quest.Clone();
                if (Quests.Count > 0)
                {
                    Quests[^1].Next = copyQuest;
                    copyQuest.Previous = Quests[^1];
                }
                Quests.Add(copyQuest);
            }
            NpcInCharge = quests.NpcInCharge;
        }
   

        public KillQuestSo GetFirstKillQuestWithEnemyAvailable(WeaponTypeSO weaponTypeSo)
        {
            foreach (var quest in Quests)
            {
                if (quest is not KillQuestSo killQuestSo) continue;
                if (!killQuestSo.EnemiesToKillByType.EnemiesByTypeDictionary.TryGetValue(weaponTypeSo,
                        out var enemyCount)) continue;
                if (enemyCount > 0)
                {
                    return killQuestSo;
                }
            }
            return null;
        }
        
        public ItemQuestSo GetFirstGetItemQuestWithEnemyAvailable(ItemSo itemType)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ItemQuestSo getQuestSo) continue;
                if (!getQuestSo.ItemsToCollectByType.TryGetValue(itemType, out var itemCount)) continue;
                if (itemCount > 0)
                {
                    return getQuestSo;
                }
            }
            return null;
        }

        public ListenQuestSo GetFirstTalkQuestWithNpc(NpcSo npc)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ListenQuestSo ListenQuestSo) continue;
                if (ListenQuestSo.Npc == npc)
                {
                    return ListenQuestSo;
                }
            }
            return null;        
        }

        public DamageQuestSo GetFirstDamageQuestWithEnemyAvailable(WeaponTypeSO weaponTypeSo)
        {
            foreach (var quest in Quests)
            {
                if (quest is not DamageQuestSo damageQuestSo) continue;
                if (!damageQuestSo.EnemiesToDamageByType.EnemiesByTypeDictionary.TryGetValue(weaponTypeSo,
                        out var enemyCount)) continue;
                if (enemyCount > 0)
                {
                    return damageQuestSo;
                }
            }
            return null;
        }

        public QuestSo GetCurrentQuest()
        {
            return CurrentQuestIndex >= Quests.Count ? null : Quests[CurrentQuestIndex];
        }
    }
}