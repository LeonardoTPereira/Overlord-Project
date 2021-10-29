using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/NPCs")]
    public class QuestNpcsSO : ScriptableObject
    {
        private int numNpcs = 0;
        private List<NpcSO> npcs;

        public int NumNpcs { get => numNpcs; set => numNpcs = value; }
        
        public void CalculateNpcsFromQuests(QuestList quests)
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
            NumNpcs++;
            quest.npc
            /*TODO create or select NpcSO*/
            /*TODO set quest to Npc*/
        }

        private static bool IsTalkQuest(QuestSO quest)
        {
            return quest.GetType() == typeof(TalkQuestSO);
        }
    }
}
