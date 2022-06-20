using System;
using Microsoft.ML.Data;
using Util;

namespace Game.DataProcessing
{
    [Serializable]
    public class PlayerModel
    {
        [LoadColumn(0)] public int totalAttempts;
        [LoadColumn(1)] public float successRate;
        [LoadColumn(2)] public float npcInteractionRate;
        [LoadColumn(3)] public float enemyKillRate;
        [LoadColumn(4)] public float treasureCollectionRate;
        [LoadColumn(5)] public float averageLostHealthPerAttempt;
        [LoadColumn(6)] public int maxCombo;
        [LoadColumn(7)] public float keyCollectionRate;
        [LoadColumn(8)] public float lockOpeningRate;
        [LoadColumn(9)] public float averageRoomRevisitingRate;

        [LoadColumn(10)] public float uniqueRoomVisitingRate;

        [LoadColumn(11)] public float masteryPreference;

        [LoadColumn(12)] public float immersionPreference;

        [LoadColumn(13)] public float creativityPreference;

        [LoadColumn(14)] public float achievementPreference;

        [LoadColumn(15)] public float averageTimeToFinish;

        [LoadColumn(16)] public bool hasWonLastLevel;

        [LoadColumn(17)] public float keyCollectionRateLastLevel;

        [LoadColumn(18)] public float lockOpeningRateLastLevel;

        [LoadColumn(19)] public float npcInteractionRateLastLevel;

        [LoadColumn(20)] public float enemyKillRateLastLevel;

        [LoadColumn(21)] public float treasureCollectionRateLastLevel;

        [LoadColumn(22)] public int lostHealthLastLevel;

        [LoadColumn(23)] public int maxComboLastLevel;

        [LoadColumn(24)] public float timeToFinishLastLevel;

        [LoadColumn(25)] public float resultingPreference;

        public static PlayerModel CreateRandomData()
        {
            var randomModel = new PlayerModel
            {
                totalAttempts = RandomSingleton.GetInstance().Next(1, 10),
                successRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                npcInteractionRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                enemyKillRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                treasureCollectionRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                averageLostHealthPerAttempt = RandomSingleton.GetInstance().Next(0, 10),
                maxCombo = RandomSingleton.GetInstance().Next(0, 50),
                keyCollectionRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                lockOpeningRate = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                averageRoomRevisitingRate = RandomSingleton.GetInstance().Next(50.0f, 200.0f),
                uniqueRoomVisitingRate = RandomSingleton.GetInstance().Next(20.0f, 100.0f),
                masteryPreference = RandomSingleton.GetInstance().Next(1.0f, 10.0f),
                immersionPreference = RandomSingleton.GetInstance().Next(1.0f, 10.0f),
                creativityPreference = RandomSingleton.GetInstance().Next(1.0f, 10.0f),
                achievementPreference = RandomSingleton.GetInstance().Next(1.0f, 10.0f),
                averageTimeToFinish = RandomSingleton.GetInstance().Next(90.0f, 600.0f),
                hasWonLastLevel = RandomSingleton.GetInstance().RandomBool(),
                keyCollectionRateLastLevel = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                lockOpeningRateLastLevel = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                npcInteractionRateLastLevel = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                enemyKillRateLastLevel = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                treasureCollectionRateLastLevel = RandomSingleton.GetInstance().Next(0.0f, 100.0f),
                lostHealthLastLevel = RandomSingleton.GetInstance().Next(0, 10),
                maxComboLastLevel = RandomSingleton.GetInstance().Next(0, 50),
                timeToFinishLastLevel = RandomSingleton.GetInstance().Next(90.0f, 600.0f),
                resultingPreference = RandomSingleton.GetInstance().Next(1.0f, 10.0f)
            };
            return randomModel;
        }
    }
}