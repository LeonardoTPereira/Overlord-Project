using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    public class QuestNpcsParameters
    {
        public int totalNpcs;
        public Dictionary<NpcSO, int> NpcsByType { get; set; }
        
        public void CalculateNpcsFromQuests(QuestLine quests)
        {
            for (var i = 0; i < quests.graph.Count; i++)
            {
                AddNpcWhenTalkQuests(quests.graph[i]);
            }
        }

        private void AddNpcWhenTalkQuests(QuestSO quest)
        {
            if (IsTalkQuest(quest))
            {
                AddNpcs((TalkQuestSO) quest);
            }
        }

        private void AddNpcs(TalkQuestSO quest)
        {
            if (NpcsByType.TryGetValue(quest.npc, out var currentNpcCounter))
            {
                NpcsByType[quest.npc] = currentNpcCounter+1;
            }
            else
            {
                NpcsByType.Add(quest.npc, 1);
            }
        }

        private static bool IsTalkQuest(QuestSO quest)
        {
            return quest.GetType() == typeof(TalkQuestSO);
        }
    }
}
