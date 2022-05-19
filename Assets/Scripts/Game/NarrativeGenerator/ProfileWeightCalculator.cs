using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    public static class ProfileWeightCalculator
    {
        public class QuestWeight
        {
            public string Quest { get; set; }
            public int Weight { get; set; }

            public QuestWeight(string quest, int weight)
            {
                Quest = quest;
                Weight = weight;
            }
        }
        private static readonly int[] weights = {1, 3, 5, 7};
        //pesos[0] = 3; //peso talk
        //pesos[1] = 7; //peso get
        //pesos[2] = 1; //peso kill
        //pesos[3] = 5; //peso explore
        public static Dictionary<string, int> CalculateProfileFromGameplayData(PlayerData playerData, DungeonData dungeonData)
        {
            var questWeightsByType = new Dictionary<string, int>
            {
                {PlayerProfile.PlayerProfileCategory.Immersion.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Achievement.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Mastery.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Creativity.ToString(), 0}
            };

            questWeightsByType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()] = 
                GetMasteryWeight(playerData.TotalDeaths, playerData.TotalAttempts, playerData.TotalLostHealth);
            questWeightsByType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()] = 
                GetAchievementWeight(playerData.EnemiesKilled, playerData.TotalEnemies, playerData.TreasuresCollected, playerData.TotalTreasure);
            questWeightsByType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()] = 
                GetImmersionWeight(playerData.NpcsInteracted, playerData.TotalNpcs);
            questWeightsByType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()] = 
                GetCreativityWeight(playerData.UniqueRoomsEntered, playerData.TotalRooms, playerData.LocksOpened, playerData.TotalLocks);

            return questWeightsByType;
        }
        
        private static int GetCreativityWeight(int roomsEntered, int totalRooms, int locksOpened, int totalLocks)
        {
            var roomVisitedRatio = roomsEntered / (float) totalRooms;
            var locksOpenedRatio = locksOpened / (float) totalLocks;
            
            switch (locksOpenedRatio)
            {
                case < 0.25f when roomVisitedRatio < 0.25f:
                    return 1;
                case < 0.5f when roomVisitedRatio < 0.5f:
                    return 3;
                case < 0.5f:
                    return 5;
                case < 0.75f when roomVisitedRatio < 0.75f:
                    return 7;
                default:
                    return 9;
            }
        }
        
        private static int GetImmersionWeight(int npcsInteracted, int totalNpcs)
        {
            var npcInteractedRatio = npcsInteracted / (float) totalNpcs;
            switch (npcInteractedRatio)
            {
                case < 0.2f:
                    return 1;
                case < 0.4f:
                    return 3;
                case < 0.6f:
                    return 5;
                case < 0.8f:
                    return 7;
                default:
                    return 9;
            }
        }

        private static int GetAchievementWeight(int enemiesKilled, int totalEnemies, int treasuresCollected, int totalTreasure)
        {
            var enemyKillRatio = enemiesKilled / (float) totalEnemies;
            var treasureCollectedRatio = treasuresCollected / (float) totalTreasure;
            
            switch (treasureCollectedRatio)
            {
                case < 0.25f when enemyKillRatio < 0.25f:
                    return 1;
                case < 0.5f when enemyKillRatio < 0.5f:
                    return 3;
                case < 0.5f:
                    return 5;
                case < 0.75f when enemyKillRatio < 0.75f:
                    return 7;
                default:
                    return 9;
            }
        }

        private static int GetMasteryWeight(int totalDeaths, int totalAttempts, int totalLostHealth)
        {
            var deathByAttemptRatio = totalDeaths / (float) totalAttempts;
            var healthLostByAttemptRatio = totalLostHealth / (float) totalAttempts;

            switch (deathByAttemptRatio)
            {
                case > 1 when healthLostByAttemptRatio > 5:
                     return 1;
                case > 1:
                    return 3;
                case > 0.5f:
                    return 5;
                case > 0.25f:
                    return 7;
                default:
                    return 9;
            }
        }

        public static Dictionary<string, int> CalculateProfileWeights(List<int> answers)
        {
            var pesos = new int[4];

            for (var i = 2; i < 12; i++)
            {
                switch (i)
                {
                    case 2:
                        pesos[2] += answers[i]-3;
                        break;
                    case 3:
                    case 4:
                        pesos[2] += answers[i];
                        break;
                    case 5:
                    case 6:
                        pesos[3] += answers[i];
                        break;
                    case 7:
                    case 8:
                        pesos[1] += answers[i];
                        break;
                    case 9:
                    case 10:
                        pesos[0] += answers[i];
                        break;
                    default:
                        pesos[3] -= answers[i]-3;
                        pesos[1] -= answers[i]-3;
                        pesos[0] -= answers[i]-3;
                        break;
                }
            }

            List<QuestWeight> questWeights = new List<QuestWeight>
            {
                new (Constants.TALK_QUEST, pesos[0]),
                new (Constants.GET_QUEST, pesos[1]),
                new (Constants.KILL_QUEST, pesos[2]),
                new (Constants.EXPLORE_QUEST, pesos[3])
            };

            questWeights = questWeights.OrderBy(x => x.Weight).ToList();

            for (var i = 0; i < questWeights.Count; ++i)
            {
                questWeights[i].Weight = weights[i];
                Debug.Log($"Quest Weight [{i}]: {questWeights[i].Weight}");
            }

            pesos[0] = questWeights.Find(x => x.Quest == Constants.TALK_QUEST).Weight;
            pesos[1] = questWeights.Find(x => x.Quest == Constants.GET_QUEST).Weight;
            pesos[2] = questWeights.Find(x => x.Quest == Constants.KILL_QUEST).Weight;
            pesos[3] = questWeights.Find(x => x.Quest == Constants.EXPLORE_QUEST).Weight;

            var questWeightsByType = new Dictionary<string, int>
            {
                {PlayerProfile.PlayerProfileCategory.Immersion.ToString(), pesos[0]},
                {PlayerProfile.PlayerProfileCategory.Achievement.ToString(), pesos[1]},
                {PlayerProfile.PlayerProfileCategory.Mastery.ToString(), pesos[2]},
                {PlayerProfile.PlayerProfileCategory.Creativity.ToString(), pesos[3]}
            };

            return questWeightsByType;
        }
    }
}