namespace Game.EnemyGenerator
{
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
        //The population size of the EA
        public const float veryEasyDifficulty = 10.0f;
		public const float easyDifficulty = 14.0f;
		public const float mediumDifficulty = 18.0f;
		public const float hardDifficulty = 22f;
		public const float veryHardDifficulty = 26f;
		public const int nBestEnemies = 20;
		public const int minDamage = 1;
		public const int maxDamage = 4;
		public const int minHealth = 1;
		public const int maxHealth = 6;
		public const float minAtkSpeed = 0.75f;
		public const float maxAtkSpeed = 4.0f;
		public const float minMoveSpeed = 0.8f;
		public const float maxMoveSpeed = 3.2f;
		public const float minActivetime = 1.5f;
		public const float maxActiveTime = 10;
		public const float minResttime = 0.3f;
		public const float maxRestTime = 1.5f;
		public const float minProjectileSpeed = 1;
		public const float maxProjectileSpeed = 4;
		

    }
}
