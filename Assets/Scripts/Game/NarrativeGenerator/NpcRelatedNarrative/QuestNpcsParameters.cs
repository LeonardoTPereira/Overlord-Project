using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class QuestNpcsParameters
    {
        [field: SerializeField]
        public NpcAmountDictionary NpcsBySo { get; set; }
        [field: SerializeField]
        public int TotalNpcs { get; set; }

        public QuestNpcsParameters()
        {
            NpcsBySo = new NpcAmountDictionary();
        }

        
        //TODO this must receive the next quest as well.
        //Here we will need to change the talk quest to hold NPC data as well.
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
            if (NpcsBySo.TryGetValue(quest.Npc, out var npcQuestList))
            {
                npcQuestList.Quests.Add(quest);
            }
            else
            {
                NpcsBySo.Add(quest.Npc, new QuestList());
                NpcsBySo[quest.Npc].Quests.Add(quest);
            }
            TotalNpcs++;
        }

        private static bool IsTalkQuest(QuestSO quest)
        {
            return quest.GetType() == typeof(TalkQuestSO);
        }
    }
}
