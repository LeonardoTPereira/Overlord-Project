using System.Collections.ObjectModel;
using UnityEngine;
using System.Collections.Generic;
using static Enums;

namespace ScriptableObjects
{
    [CreateAssetMenu]
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

