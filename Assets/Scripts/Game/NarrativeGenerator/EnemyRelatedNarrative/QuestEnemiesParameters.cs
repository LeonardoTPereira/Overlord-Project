using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EnemyGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
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
        public DifficultyLevels Difficulty { get; set; }

        public QuestEnemiesParameters()
        {
            NEnemies = 0;
            TotalByType = new EnemiesByType();
            Difficulty = DifficultyLevels.Medium;
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
                Difficulty = DifficultyLevels.Hard;
            }
            else if (MasteryIsLessPreferred(playerProfile))
            {
                Difficulty = DifficultyLevels.Easy;
            }
            else
            {
                Difficulty = DifficultyLevels.Medium;
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
            foreach (var quest in quests.questLines.SelectMany(questLine => questLine.Quests))
            {
                AddEnemiesWhenEnemyQuest(quest);
            }
        }
        
        private void AddEnemiesWhenEnemyQuest(QuestSo quest)
        {
            if (quest.IsKillQuest())
            {
                AddEnemies((KillQuestSo) quest);
            }
            else if (quest.IsDropQuest())
            {
                AddEnemies((DropQuestSo) quest);
            }
        }

        private void AddEnemies(KillQuestSo quest)
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
