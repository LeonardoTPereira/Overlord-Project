using System;
using System.Linq;
using System.Text;
using Game.EnemyGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
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
        }

        private void AddEnemies(KillQuestSo quest)
        {
            foreach (var enemyAmountPair in quest.EnemiesToKillByType.EnemiesByTypeDictionary)
            {
                foreach (var questId in enemyAmountPair.Value)
                {
                    TotalByType.EnemiesByTypeDictionary.AddItemWithId(enemyAmountPair.Key, questId);
                }
            }
        }
    }
}
