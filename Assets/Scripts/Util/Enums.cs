using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Util
{
    public class Enums
    {
        public enum MovementEnum
        {
            None,
            Random,
            Follow,
            Flee,
            Random1D,
            Follow1D,
            Flee1D,
            COUNT
        }

        public enum ProjectileEnum
        {
            None,
            Arrow,
            Bomb,
            COUNT
        }

        public enum FormEnum
        {
            PreTestForm,
            PostTestForm
        }

        public enum QuestWeights
        {
            Hated = 1,
            Disliked = 3,
            Liked = 5,
            Loved = 7
        }

        public enum EnemyDifficultyInDungeon
        {
            VeryEasy = 11,
            Easy = 13,
            Medium = 15,
            Hard = 17,
            VeryHard = 19
        }

        public enum DungeonSize
        {
            VerySmall = 16,
            Small = 20,
            Medium = 24,
            Large = 28,
            VeryLarge = 32
        }

        public enum DungeonLinearity
        {
            VeryLinear,
            Linear,
            Medium,
            Branched,
            VeryBranched
        }

        public enum DungeonKeys
        {
            AFewKeys = 3,
            SomeKeys = 4,
            SeveralKeys = 5,
            ManyKeys = 6,
            LotsOfKeys = 7
        }

        public enum PlayerProjectileEnum
        {
            STRAIGHT = 0,
            SIN = 1,
            TRIPLE = 2
        }

        public enum EnemyTypeEnum
        {
            EASY = 0,
            MEDIUM = 1,
            HARD = 2,
            ARENA = 3
        }
    }

    public static class DungeonLinearityConverter
    {
        public static readonly ReadOnlyDictionary<Enums.DungeonLinearity, float> DungeonLinearityEnumToFloat
            = new ReadOnlyDictionary<Enums.DungeonLinearity, float>(new Dictionary<Enums.DungeonLinearity, float>
            {
                { Enums.DungeonLinearity.VeryLinear, 1.0f},
                { Enums.DungeonLinearity.Linear, 1.2f},
                { Enums.DungeonLinearity.Medium, 1.4f},
                { Enums.DungeonLinearity.Branched, 1.6f},
                { Enums.DungeonLinearity.VeryBranched, 1.8f},
            });

        public static float ToFloat(this Enums.DungeonLinearity dungeonLinearity)
        {
            return DungeonLinearityEnumToFloat[dungeonLinearity];
        }

    }
}