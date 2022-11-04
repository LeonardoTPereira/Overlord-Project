using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Util;

namespace Game.NarrativeGenerator
{
    public static class QuestWeightsCalculator
    {        
        private const float minWeightPercentage = 0.2f;
        private const float percentageDivider = 10;
        private const float killPercentageDivider = 15;

        public static float GetTalkQuestWeight ( List<int> answers )
        {
            // Talk questions = 10, 11
            float [] talkWeightQuestions = {answers[9], answers[10]};
            float talkWeight = CalculateWeightSum( talkWeightQuestions )/percentageDivider;
            if ( talkWeight > 0) return talkWeight;
            return minWeightPercentage;
        }

        public static float GetGetQuestWeight ( List<int> answers )
        {
            // Get questions = 7, 8
            float [] getWeightQuestions = {answers[7], answers[8]};
            float getWeight = CalculateWeightSum( getWeightQuestions )/percentageDivider;
            if ( getWeight > 0) return getWeight;
            return minWeightPercentage;
        }

        public static float GetExploreQuestWeight ( List<int> answers )
        {
            // Explore questions = 5, 6
            float [] exploreWeightQuestions = {answers[5], answers[6]};
            float exploreWeight = CalculateWeightSum( exploreWeightQuestions )/percentageDivider;
            if ( exploreWeight > 0) return exploreWeight;
            return minWeightPercentage;
        }

        public static float GetKillQuestWeight ( List<int> answers )
        {
            // Kill questions = 2, 3 e 4
            float [] killWeightQuestions = {answers[2], answers[3], answers[4]};
            float killWeight = CalculateWeightSum( killWeightQuestions )/killPercentageDivider;
            if ( killWeight > 0) return killWeight;
            return minWeightPercentage;
        }

        private static float CalculateWeightSum ( float [] answers )
        {
            float weightSum = (float)(answers.Sum());
            return weightSum;
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