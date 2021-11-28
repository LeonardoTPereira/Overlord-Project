﻿using System;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class QuestNpcsParameters
    {
        [SerializeField] private int totalNpcs;
        public int TotalNpcs => totalNpcs;

        [field: SerializeField]
        private NpcAmountDictionary NpcsByType { get; }

        public QuestNpcsParameters()
        {
            totalNpcs = 0;
            NpcsByType = new NpcAmountDictionary();
        }

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
            Debug.Log("Quest: " + quest.name + " NPC: "+ quest.npc);
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