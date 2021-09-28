using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/NPCs")]
    public class ParametersNpcs : ScriptableObject
    {
        private int totalNpcs = 0;
        private List<NpcSO> npcs;

        public int NumNpcs { get => totalNpcs; set => totalNpcs = value; }
        
        private void ConversorNpcs(QuestList quests)
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
                AddNpcs();
            }
        }

        private void AddNpcs()
        {
            NumNpcs++;
            /*TODO create or select NpcSO*/
            /*TODO set quest to Npc*/
        }

        private static bool IsTalkQuest(QuestSO quest)
        {
            return quest.GetType() == typeof(TalkQuestSO);
        }
    }
}
