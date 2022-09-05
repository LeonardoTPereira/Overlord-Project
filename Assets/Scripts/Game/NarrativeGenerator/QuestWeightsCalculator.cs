using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Util;

namespace Game.NarrativeGenerator
{
    public static class QuestWeightsCalculator
    {
        private const float LikertScaleMaximum = 5.0f;
        public static float GetWeightFromPreTest(int answer)
        {
            if (answer == 0)
            {
                answer++;
            }

            return answer / LikertScaleMaximum;
        }
        public static int GetCreativityWeight(int roomsEntered, int totalRooms, int locksOpened, int totalLocks)
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
        
        public static int GetImmersionWeight(int npcsInteracted, int totalNpcs)
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

        public static int GetAchievementWeight(int enemiesKilled, int totalEnemies, int treasuresCollected, int totalTreasure)
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

        public static int GetMasteryWeight(int totalDeaths, int totalAttempts, int totalLostHealth)
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
    }
}