using System;
using System.Collections.Generic;
using Game.NPCs;
using Game.Quests;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [Serializable]
    public class QuestList
    {
        [field: SerializeReference] public List<QuestSo> Quests {get; set; }
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }
        public static event QuestCompletedEvent QuestCompletedEventHandler;

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
   
        public bool RemoveAvailableQuestWithId<T, U>(U questElement, int questId) where T : QuestSo
        {
            foreach (var quest in Quests)
            {
                if (quest is not T questSo) continue;
                if (!questSo.HasAvailableElementWithId(questElement, questId)) continue;
                questSo.RemoveElementWithId(questElement, questId);
                if (questSo.IsCompleted && questSo == GetCurrentQuest())
                {
                    CloseCurrentQuest();
                }

                return true;
            }
            return false;
        }

        private void CloseCurrentQuest()
        {
            QuestSo currentQuest;
            do
            {
                currentQuest = GetCurrentQuest();
                QuestCompletedEventHandler?.Invoke(null, new NewQuestEventArgs(currentQuest, NpcInCharge));
                currentQuest.IsClosed = true;
                CurrentQuestIndex++;
            } while (currentQuest.IsCompleted);
        }

        public QuestSo GetCurrentQuest()
        {
            return CurrentQuestIndex >= Quests.Count ? null : Quests[CurrentQuestIndex];
        }
    }
}