using System;

namespace Game.EnemyGenerator
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

    public static class EnemyUtil
    {
        public const float veryEasyDifficultyFactor = 11.0f;
        public const float easyDifficultyFactor = 13.0f;
        public const float mediumDifficultyFactor = 15.0f;
        public const float hardDifficultyFactor = 17f;
        public const float veryHardDifficultyFactor = 19f;
    }
}