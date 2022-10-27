using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class IndividualJsonList
    {
        private const string filePath = @"E:\Doutorado\Resultados\RealTimeQuestGenerator\DungeonGeneration";
        private string dungeonSetting;
        [Serializable]
        private struct IndividualData
        {
            [SerializeField] public int generation;
            [SerializeField] public float distance;
            [SerializeField] public float usage;
            [SerializeField] public float sparsity;
            [SerializeField] public float enemyStandardDeviation;
            [SerializeField] public float result;
            [SerializeField] public float normalizedDistance;
            [SerializeField] public float normalizedUsage;
            [SerializeField] public float normalizedSparsity;
            [SerializeField] public float normalizedEnemyStandardDeviation;
            [SerializeField] public int enemies;
            [SerializeField] public int rooms;
            [SerializeField] public int keys;
            [SerializeField] public int locks;
            [SerializeField] public float neededRooms;
            [SerializeField] public float neededLocks;
            [SerializeField] public float linearity;
            [SerializeField] public int items;
            [SerializeField] public int desiredEnemies;
            [SerializeField] public int desiredRooms;
            [SerializeField] public int desiredKeys;
            [SerializeField] public int desiredLocks;
            [SerializeField] public float desiredLinearity;
            [SerializeField] public int quests;
            [SerializeField] public int achievementQuests;
            [SerializeField] public int creativityQuests;
            [SerializeField] public int immersionQuests;
            [SerializeField] public int masteryQuests;
            [SerializeField] public int achievementWeight;
            [SerializeField] public int creativityWeight;
            [SerializeField] public int immersionWeight;
            [SerializeField] public int masteryWeight;
            
            public IndividualData(Individual individual)
            {
                generation = individual.generation;
                distance = individual.Fitness.Distance;
                usage = individual.Fitness.Usage;
                sparsity = individual.Fitness.EnemySparsity;
                enemyStandardDeviation = individual.Fitness.EnemyStandardDeviation;
                normalizedDistance = individual.Fitness.NormalizedDistance;
                normalizedUsage = individual.Fitness.NormalizedUsage;
                normalizedSparsity = individual.Fitness.NormalizedEnemySparsity;
                normalizedEnemyStandardDeviation = individual.Fitness.NormalizedEnemyStandardDeviation;
                result = individual.Fitness.NormalizedResult;
                enemies = individual.dungeon.GetNumberOfEnemies();
                rooms = individual.dungeon.Rooms.Count;
                keys = individual.dungeon.KeyIds.Count;
                locks = individual.dungeon.LockIds.Count;
                neededRooms = individual.neededRooms;
                neededLocks = individual.neededLocks;
                linearity = individual.linearCoefficient;
                items = individual.Fitness.DesiredInput.DesiredItems;
                desiredEnemies = individual.Fitness.DesiredInput.DesiredEnemies;
                desiredRooms = individual.Fitness.DesiredInput.DesiredRooms;
                desiredKeys = individual.Fitness.DesiredInput.DesiredKeys;
                desiredLocks = individual.Fitness.DesiredInput.DesiredLocks;
                desiredLinearity = individual.Fitness.DesiredInput.DesiredLinearity;
                achievementQuests = 0;
                creativityQuests = 0;
                immersionQuests = 0;
                masteryQuests = 0;
                foreach (var quest in individual.Fitness.DesiredInput.QuestLines.SelectMany(questLine => questLine.Quests))
                {
                    switch (quest)
                    {
                        case AchievementQuestSo:
                            achievementQuests++;
                            break;
                        case CreativityQuestSo:
                            creativityQuests++;
                            break;
                        case ImmersionQuestSo:
                            immersionQuests++;
                            break;
                        case MasteryQuestSo:
                            masteryQuests++;
                            break;
                        default:
                            Debug.LogError($"No quest type for this quest {quest.GetType()} " +
                                           "was found to create dialogue");
                            break;
                    }
                }
                quests = creativityQuests + masteryQuests + immersionQuests + achievementQuests;
                achievementWeight = (int) individual.Fitness.DesiredInput.PlayerProfile.AchievementPreference;
                creativityWeight = (int) individual.Fitness.DesiredInput.PlayerProfile.CreativityPreference;
                immersionWeight = (int) individual.Fitness.DesiredInput.PlayerProfile.ImmersionPreference;
                masteryWeight = (int) individual.Fitness.DesiredInput.PlayerProfile.MasteryPreference;
            }
        }
        [SerializeField] private List<IndividualData> data;

        public IndividualJsonList()
        {
            data = new List<IndividualData>();
        }

        public void Add(Individual individual)
        {
            data.Add(new IndividualData(individual));
        }

        public void SaveJson()
        {
            dungeonSetting =
                $"A{data[0].achievementWeight}-C{data[0].creativityWeight}-I{data[0].immersionWeight}-M{data[0].masteryWeight}";
            var finalPath = filePath + dungeonSetting + ".json";
            if (!File.Exists(finalPath))
            {
                File.Create(finalPath).Dispose();
            }

            using var sw = File.AppendText(finalPath);
            sw.Write(JsonUtility.ToJson(this));
        }
    }
}