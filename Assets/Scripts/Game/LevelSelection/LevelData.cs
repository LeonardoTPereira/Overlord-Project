using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelSelection
{
    [Serializable]
    public class LevelData
    {
        [field:SerializeField] public QuestLine Quests { get; set; }
        [field:SerializeField] public DungeonFileSo Dungeon { get; set; }

        public LevelData(QuestLine quests, DungeonFileSo dungeon)
        {
            Quests = quests;
            Dungeon = dungeon;
        }
    }
}