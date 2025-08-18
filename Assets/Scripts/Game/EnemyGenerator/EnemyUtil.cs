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
        public const float veryEasyDifficulty = 11.0f;
        public const float easyDifficulty = 13.0f;
        public const float mediumDifficulty = 15.0f;
        public const float hardDifficulty = 17f;
        public const float veryHardDifficulty = 19f;
    }
}