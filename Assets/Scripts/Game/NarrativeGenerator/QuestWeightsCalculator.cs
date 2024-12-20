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
    // ponderação em inputs da sala + dados do jogador -> rebalancear para 1, 2, 3, 4

    // fator da perfil -> 1 = .25, 2 = ..., 4 = 1.00
    // fator do perfil - dados do jogador > 0.2 muda, senão mantem
    // clamp no 1 e no 4

    
    // setar limite para taxa de revisitação para 100 (2 -> 100)
    // compleção do mapa, lock used => explorer
        public static float GetCreativityWeight(float previousCreativityWeight, int roomsEntered, int totalRooms, int locksOpened, int totalLocks)
        {
            float initialWeight = (1/(float)Enums.QuestWeights.Loved) * previousCreativityWeight;

            var roomVisitedRatio = roomsEntered / (float) totalRooms;
            var locksOpenedRatio = locksOpened / (float) totalLocks;

            float dataAverage = (roomVisitedRatio) / 3;
            

            float newWeight = GetAdjustedValue( initialWeight, dataAverage );
            return GetClampedValue(newWeight);
        }
        
        // valor de imersão => % de compleção de quests de imerção
        public static float GetImmersionWeight(float previousImmersionWeight, int completedImmersionQuests, int totalImmersionQuests)
        {
            //TODO: Make sure the previous weight is in terms of 1, 2, 3, 4

            float immersionQuestCompletionRatio = completedImmersionQuests / (float) totalImmersionQuests;
            float initialWeight = (1/(float)Enums.QuestWeights.Loved) * previousImmersionWeight;

            float newWeight = GetAdjustedValue( initialWeight, immersionQuestCompletionRatio );
            return GetClampedValue(newWeight);
        }

        // todos os dados juntos/ponderação ( enemy kill rate+ revist rate+ %items coletados + completude do mapa) => achiever
        public static float GetAchievementWeight( float previousAchievementWeight, int enemiesKilled, int totalEnemies, int treasuresCollected, int totalTreasure, int roomsEntered, int totalRooms )
        {
            float initialWeight = ( 1/(float)Enums.QuestWeights.Loved) * previousAchievementWeight;

            float enemyKillRatio = enemiesKilled / (float) totalEnemies;
            float treasureCollectedRatio = treasuresCollected / (float) totalTreasure;
            float roomVisitedRatio = roomsEntered / (float) totalRooms;

            float dataAverage = (enemyKillRatio + treasureCollectedRatio + roomVisitedRatio) / 3;

            float newWeight = GetAdjustedValue( initialWeight, dataAverage );
            return GetClampedValue(newWeight);
        }

        // combinação/ponderação entre (1 - %vida perdida, quantos inimigos matou) => mastery
        public static float GetMasteryWeight(float previousMasteryWeight, int enemiesKilled, int totalEnemies, int totalLostHealth, int totalHealth)
        {
            float initialWeight = ( 1/(float)Enums.QuestWeights.Loved) * previousMasteryWeight;

            float enemyKillRatio = enemiesKilled / (float) totalEnemies;
            float invertedHealthLostRatio = 1 - (totalLostHealth / totalHealth);

            float dataAverage = (enemyKillRatio + invertedHealthLostRatio) / 2;

            float newWeight = GetAdjustedValue( initialWeight, dataAverage );
            return GetClampedValue(newWeight);
        }

        private static float GetAdjustedValue( float initialValue, float currentValue )
        {
            float adjustedValue = initialValue;
            if ( initialValue - currentValue > .2f )
            {
                adjustedValue -= .2f;
            }
            else if ( initialValue - currentValue < - .2f )
            {
                adjustedValue += .2f;
            }
            return adjustedValue;
        }

        private static float GetClampedValue( float unclampedValue )
        {
            float clampedValue = Mathf.Min( unclampedValue, (float)Enums.QuestWeights.Loved );
            clampedValue = Mathf.Max( clampedValue, (float)Enums.QuestWeights.Hated );
            return clampedValue;
        }
    }
}