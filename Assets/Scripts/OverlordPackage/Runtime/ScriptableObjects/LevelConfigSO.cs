using UnityEngine;
using static Util.Enums;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfigSO", menuName = "Overlord-Project/Levels-Generator/Util/LevelConfig")]
    public class LevelConfigSO : ScriptableObject
    {
        public DungeonSize dungeonSize;
        public DungeonLinearity dungeonLinearity;
        public string levelName;
        public EnemyDifficultyInDungeon enemyDifficulty;
        public string enemyDifficultyFile;
        public string fileName;
    }
}
