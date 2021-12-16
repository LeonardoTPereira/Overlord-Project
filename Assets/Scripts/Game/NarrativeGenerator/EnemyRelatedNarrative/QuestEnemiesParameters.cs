using System;
using System.Collections.Generic;
using System.Text;
using Game.EnemyGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.EnemyRelatedNarrative
{
    [Serializable]
    public class QuestEnemiesParameters
    {
        [field: SerializeField]
        public EnemiesByType TotalByType { get; set; }

        [field: SerializeField]
        public int NEnemies { get; set; }

        [field: SerializeField]
        public DifficultyEnum Difficulty { get; set; }

        public QuestEnemiesParameters()
        {
            NEnemies = 0;
            TotalByType = new EnemiesByType();
            Difficulty = DifficultyEnum.Medium;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var kvp in TotalByType.EnemiesByTypeDictionary)
            {
                stringBuilder.Append($"Enemy = {kvp.Key}, total = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }

        public void CalculateDifficultyFromProfile(PlayerProfile playerProfile)
        {
            if (playerProfile.PlayerProfileEnum == PlayerProfile.PlayerProfileCategory.Mastery)
            {
                Difficulty = DifficultyEnum.Hard;
            }
            else if (MasteryIsLessPreferred(playerProfile))
            {
                Difficulty = DifficultyEnum.Easy;
            }
            else
            {
                Difficulty = DifficultyEnum.Medium;
            }
        }

        private bool MasteryIsLessPreferred(PlayerProfile playerProfile)
        {
            if (playerProfile.MasteryPreference > playerProfile.AchievementPreference)
            {
                return false;
            }
            if (playerProfile.MasteryPreference > playerProfile.CreativityPreference)
            {
                return false;
            }
            if (playerProfile.MasteryPreference > playerProfile.ImmersionPreference)
            {
                return false;
            }
            return true;
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
                AddEnemies((DropQuestSo) quest);
            }
        }

        private void AddEnemies(KillQuestSO quest)
        {
            foreach (var enemyAmountPair in quest.EnemiesToKillByType.EnemiesByTypeDictionary)
            {
                AddEnemiesToDictionary(enemyAmountPair);
            }
        }
        
        /*
         * TODO the enemies on drop quests could be the same from the killEnemies quest. We can try to check overlaps
         * and avoid creating more from these quests if possible
         */
        private void AddEnemies(DropQuestSo quest)
        {
            foreach (var dropItemData in quest.ItemData)
            {
                AddEnemiesFromPairToDictionary(dropItemData);
            }
        }

        private void AddEnemiesFromPairToDictionary(KeyValuePair<ItemSo, EnemiesByType> dropItemData)
        {
            foreach (var enemyData in dropItemData.Value.EnemiesByTypeDictionary)
            {
                AddEnemiesToDictionary(enemyData);
            }
        }

        private void AddEnemiesToDictionary(KeyValuePair<WeaponTypeSO, int> enemyData)
        {
            int newEnemies = enemyData.Value;
            NEnemies += newEnemies;
            if (TotalByType.EnemiesByTypeDictionary.TryGetValue(enemyData.Key, out var enemiesForItem))
            {
                TotalByType.EnemiesByTypeDictionary[enemyData.Key] = enemiesForItem + newEnemies;
            }
            else
            {
                TotalByType.EnemiesByTypeDictionary.Add(enemyData.Key, newEnemies);
            }
        }
    }
}
