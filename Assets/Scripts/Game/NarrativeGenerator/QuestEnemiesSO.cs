using System.Collections.Generic;
using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Enemies")]
    public class QuestEnemiesSO : ScriptableObject
    {
        private int nEnemies;
        private SortedList<EnemySO, int> totalByType;

        public int NEnemies { get => nEnemies; set => nEnemies = value; }

        public QuestEnemiesSO()
        {
            NEnemies = 0;
            totalByType = new SortedList<EnemySO, int>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<EnemySO, int> kvp in totalByType)
            {
                stringBuilder.Append($"Enemy = {kvp.Key}, total = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }

        public void CalculateMonsterFromQuests(QuestList quests)
        {
            foreach (var quest in quests.graph)
            {
                AddEnemiesWhenEnemyQuest(quest);
            }
        }
        
        private void AddEnemiesWhenEnemyQuest(QuestSO quest)
        {
            if (quest.IsKillQuest())
            {
                AddEnemies((KillQuestSO) quest);
            }
            else if (quest.IsDropQuest())
            {
                AddEnemies((DropQuestSO) quest);
            }
        }

        private void AddEnemies(KillQuestSO quest)
        {
            foreach (var enemyAmountPair in quest.EnemiesToKillByType)
            {
                AddEnemiesToDictionary(enemyAmountPair);
            }
        }
        
        /*
         * TODO the enemies on drop quests could be the same from the killEnemies quest. We can try to check overlaps
         * and avoid creating more from these quests if possible
         */
        private void AddEnemies(DropQuestSO quest)
        {
            foreach (var dropItemData in quest.ItemData)
            {
                AddEnemiesFromPairToDictionary(dropItemData);
            }
        }

        private void AddEnemiesFromPairToDictionary(KeyValuePair<ItemSO, Dictionary<EnemySO, int>> dropItemData)
        {
            foreach (var enemyData in dropItemData.Value)
            {
                AddEnemiesToDictionary(enemyData);
            }
        }

        private void AddEnemiesToDictionary(KeyValuePair<EnemySO, int> enemyData)
        {
            int newEnemies = enemyData.Value;
            nEnemies += newEnemies;
            if (totalByType.TryGetValue(enemyData.Key, out var enemiesForItem))
            {
                totalByType[enemyData.Key] = enemiesForItem + newEnemies;
            }
            else
            {
                totalByType.Add(enemyData.Key, newEnemies);
            }
        }
    }
}
