using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Util;

namespace Game.NarrativeGenerator
{
    public static class QuestWeightsCalculator
    {
        public static int CalculateTotalQuestionsWeight (List<int> answers)
        {
            int totalQuestionsWeight = 0;
            int firstQuestionIndex = 2;
            int lastQuestionIndex = 12;
            for ( var i = firstQuestionIndex; i < lastQuestionIndex; i++ )
            {
                if ( i < lastQuestionIndex - 1 )
                {
                    if ( i == firstQuestionIndex )
                    {
                        totalQuestionsWeight += answers[i] -3;
                    }
                    else
                    {
                        totalQuestionsWeight += answers[i];
                    }
                }
                else
                {
                    totalQuestionsWeight -= 3*(answers[i] -3);
                }
            }
            return totalQuestionsWeight;
        }
        
        public static int GetTalkQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Talk questions = 10, 11
            int [] talkWeightQuestions = {answers[9], answers[10], -1*(answers[11]-3)};
            int talkWeight = CalculateWeightSum( talkWeightQuestions, totalQuestionsWeight );
            return talkWeight;
        }

        public static int GetGetQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Get questions = 7, 8
            int [] getWeightQuestions = {answers[7], answers[8], -1*(answers[11]-3)};
            int getWeight = CalculateWeightSum( getWeightQuestions, totalQuestionsWeight );
            return getWeight;
        }

        public static int GetExploreQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Explore questions = 5, 6
            int [] exploreWeightQuestions = {answers[5], answers[6], -1*(answers[11]-3)};
            int exploreWeight = CalculateWeightSum( exploreWeightQuestions, totalQuestionsWeight );
            return exploreWeight;
        }

        public static int GetKillQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Kill questions = 2, 3 e 4
            int [] killWeightQuestions = {answers[2]-3, answers[3], answers[4]};
            int killWeight = CalculateWeightSum( killWeightQuestions, totalQuestionsWeight );
            return killWeight;
        }

        private static int CalculateWeightSum ( int [] answers, int totalQuestionsWeight )
        {
            int weightSum = (int)(answers.Sum()/(float)totalQuestionsWeight*100);
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