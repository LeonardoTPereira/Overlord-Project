using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using Util;
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
        
        public GatherQuestSo GetFirstGetItemQuestWithEnemyAvailable(ItemSo itemType)
        {
            foreach (var quest in Quests)
            {
                if (quest is not GatherQuestSo gatherQuestSo) continue;
                if (!gatherQuestSo.ItemsToGatherByType.TryGetValue(itemType, out var itemCount)) continue;
                if (itemCount > 0)
                {
                    return gatherQuestSo;
                }
            }
            return null;
        }

        public ListenQuestSo GetFirstListenQuestWithNpc(NpcSo npc)
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

        public ExploreQuestSo GetFirstExploreQuestWithRoomAvailable(Coordinates roomCoordinates)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ExploreQuestSo exploreQuestSo) continue;
                if (!exploreQuestSo.CheckIfCompleted()) return exploreQuestSo;
            }
            return null;
        }

        public GiveQuestSo GetFirstGiveQuestAvailable(ItemSo itemType)
        {
            foreach (var quest in Quests)
            {
                if (quest is not GiveQuestSo giveQuestSo) continue;
                if (!giveQuestSo.HasItemToCollect(itemType)) continue;
                    return giveQuestSo;
            }
            return null;
        }

        public GiveQuestSo GetFirstGiveQuestWithNpc(NpcSo npc)
        {
            foreach (var quest in Quests)
            {
                if (quest is not GiveQuestSo giveQuestSo) continue;
                if (giveQuestSo.Npc == npc)
                {
                    return giveQuestSo;
                }
            }
            return null;        
        }

        public ExchangeQuestSo GetFirstExchangeQuestWithNpc(NpcSo npc)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ExchangeQuestSo exchangeQuestSo) continue;
                if (exchangeQuestSo.Npc == npc)
                {
                    return exchangeQuestSo;
                }
            }
            return null;        
        }

        public ExchangeQuestSo GetFirstExchangeQuestAvailable(ItemSo itemType)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ExchangeQuestSo exchangeQuestSo) continue;
                if (!exchangeQuestSo.HasItemToExchange(itemType)) continue;
                    return exchangeQuestSo;
            }
            return null;
        }

        public ReportQuestSo GetFirstReportQuestWithNpc(NpcSo npc)
        {
            foreach (var quest in Quests)
            {
                if (quest is not ReportQuestSo ReportQuestSo) continue;
                if (ReportQuestSo.Npc == npc)
                {
                    return ReportQuestSo;
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