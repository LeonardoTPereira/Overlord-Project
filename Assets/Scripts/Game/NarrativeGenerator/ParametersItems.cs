using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Items")]
    public class ParametersItems : ScriptableObject
    {
        public Dictionary<ItemSo, int> ItemsByType { get; }
        public int NumItens;

        public void ConversorItems(QuestLine quests)
        {
            for (var i = 0; i < quests.graph.Count; i++)
            {
                AddItemWhenItemQuest(quests.graph[i]);
            }
        }

        private void AddItemWhenItemQuest(QuestSO quest)
        {
            if (IsItemQuest(quest))
            {
                AddItems((ItemQuestSo) quest);
            }
        }

        private void AddItems(ItemQuestSo quest)
        {
            
            /*TODO create or select ItemSO*/
        }

        private static bool IsItemQuest(QuestSO quest)
        {
            return quest.GetType().IsAssignableFrom(typeof(ItemQuestSo));
        }
    }


}
