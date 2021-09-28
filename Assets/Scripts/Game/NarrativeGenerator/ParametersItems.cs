using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Items")]
    public class ParametersItems : ScriptableObject
    {
        public Dictionary<ItemSO, int> ItemsByType { get; }

        private void ConversorItems(QuestList quests)
        {
            for (var i = 0; i < quests.graph.Count; i++)
            {
                AddItemWhenItemQuest(quests.graph[i]);
            }
        }

        private void AddNpcInTalkQuests(QuestSO quest)
        {
            if (IsItemQuest(quest))
            {
                AddItems(quest);
            }
        }

        private void AddItems(ItemQuestSO quest)
        {
            quest.TotalTreasure;
            /*TODO create or select NpcSO*/
            /*TODO set quest to Npc*/
        }

        private static bool IsItemQuest(QuestSO quest)
        {
            return quest.GetType().IsAssignableFrom(typeof(ItemQuestSO));
        }
    }


}
