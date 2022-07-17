using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
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
            foreach (var quest in quests.questLines.SelectMany(questLine => questLine.Quests))
            {
                AddNpcWhenTalkQuests(quest);
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
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var kvp in NpcsBySo)
            {
                stringBuilder.Append($"Npc = {kvp.Key.NpcName}, total Quests = {kvp.Value.Quests.Count}\n");
            }
            return stringBuilder.ToString();
        }

        public List<NpcSo> GetNpcs()
        {
            return NpcsBySo.Select(kvp => kvp.Key).ToList();
        }
    }
}
