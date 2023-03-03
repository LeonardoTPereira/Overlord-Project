namespace Util
{
    public static class Enums
    {
	    public enum RoomThemeEnum
	    {
		    Purple,
		    Blue,
		    Green,
		    Yellow,
		    Red,
		    Count
	    }
	    
        public enum MovementEnum
        {
            None,
            Random,
            Follow,
            Flee,
            Random1D,
            Follow1D,
            Flee1D,
            Count
        }

        public enum ProjectileEnum
        {
            None,
            Arrow,
            Bomb,
            Count
        }
        
        public enum TileTypes
        {
            Floor,
            Block
        }
        
        public enum RoomPatterns
        {
            Empty,
            CheckerBoard,
            HorizontalLines,
            VerticalLines,
            Cross,
            Count
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
            Straight = 0,
            Sin = 1,
            Triple = 2
        }

        public enum EnemyTypeEnum
        {
            Easy = 0,
            Medium = 1,
            Hard = 2,
            Arena = 3
        }
        
        public enum GameType
        {
            TopDown,
            Platformer
        }
    }
}