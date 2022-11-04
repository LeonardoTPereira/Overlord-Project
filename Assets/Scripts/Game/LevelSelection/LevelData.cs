using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelSelection
{
    [Serializable]
    [CreateAssetMenu(fileName = "LevelData", menuName = "Overlord-Project/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field:SerializeField] public QuestLineList QuestLines { get; set; }
        [field:SerializeField] public DungeonFileSo Dungeon { get; set; }
        private bool _completed;
        private bool _surrendered;

        public void Init(QuestLineList questLines, DungeonFileSo dungeon)
        {
            QuestLines = CreateInstance<QuestLineList>();
            QuestLines.Init(questLines);
            Dungeon = dungeon;
            QuestLines.ConvertDataForCurrentDungeon(Dungeon.Rooms);
            _completed = false;
            _surrendered = false;
        }

        public void CompleteLevel()
        {
            _completed = true;
        }

        public void GiveUpLevel()
        {
            _surrendered = true;
        }

        public bool HasCompleted()
        {
            return _completed || _surrendered;
        }
    }
}