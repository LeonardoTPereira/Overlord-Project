using System.Collections.Generic;
using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [Serializeable]
    public class QuestEnemiesParameters
    {
        private int _nEnemies;
        private Dictionary<WeaponTypeSO, int> _totalByType;

        public Dictionary<WeaponTypeSO, int> TotalByType
        {
            get => _totalByType;
            set => _totalByType = value;
        }
        
        public int NEnemies { get => _nEnemies; set => _nEnemies = value; }

        public QuestEnemiesParameters()
        {
            NEnemies = 0;
            TotalByType = new Dictionary<WeaponTypeSO, int>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<WeaponTypeSO, int> kvp in TotalByType)
            {
                stringBuilder.Append($"Enemy = {kvp.Key}, total = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }

        public void CalculateMonsterFromQuests(QuestLine quests)
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

        private void AddEnemiesFromPairToDictionary(KeyValuePair<ItemSO, Dictionary<WeaponTypeSO, int>> dropItemData)
        {
            foreach (var enemyData in dropItemData.Value)
            {
                AddEnemiesToDictionary(enemyData);
            }
        }

        private void AddEnemiesToDictionary(KeyValuePair<WeaponTypeSO, int> enemyData)
        {
            int newEnemies = enemyData.Value;
            _nEnemies += newEnemies;
            if (TotalByType.TryGetValue(enemyData.Key, out var enemiesForItem))
            {
                TotalByType[enemyData.Key] = enemiesForItem + newEnemies;
            }
            else
            {
                TotalByType.Add(enemyData.Key, newEnemies);
            }
        }
    }
}
