using System;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    [Serializable]
    public enum DifficultyLevels
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard
    }

    public static class EnemyDifficultyFactor
    {
        public const float veryEasyDifficultyFactor = 11.0f;
        public const float easyDifficultyFactor = 13.0f;
        public const float mediumDifficultyFactor = 15.0f;
        public const float hardDifficultyFactor = 17f;
        public const float veryHardDifficultyFactor = 19f;

        public static float GetDifficultyFactor(DifficultyLevels difficulty)
        {
            switch (difficulty)
            {
                case DifficultyLevels.VeryEasy:
                    return veryEasyDifficultyFactor;
                case DifficultyLevels.Easy:
                    return easyDifficultyFactor;
                case DifficultyLevels.Medium:
                    return mediumDifficultyFactor;
                case DifficultyLevels.Hard:
                    return hardDifficultyFactor;
                case DifficultyLevels.VeryHard:
                    return veryHardDifficultyFactor;
                default:
                    UnityEngine.Debug.LogWarning("Difficulty not set, defaulting to Medium.");
                    return mediumDifficultyFactor;
            }
        }
    }
}