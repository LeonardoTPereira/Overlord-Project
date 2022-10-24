using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using Game.SaveLoadSystem;
using UnityEngine;

namespace Game.LevelSelection
{
    [Serializable]
    [CreateAssetMenu(fileName = "LevelData", menuName = "Overlord-Project/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field:SerializeField] public QuestLineList QuestLines { get; set; }
        [field:SerializeField] public DungeonFileSo Dungeon { get; set; }
        [field:SerializeField] protected bool IsCompleted { get; set; }
        [field:SerializeField] protected bool HasSurrendered { get; set; }
        protected bool HasDataBeenLoaded { get; set; }

        public void Init(QuestLineList questLines, DungeonFileSo dungeon)
        {
            QuestLines = CreateInstance<QuestLineList>();
            QuestLines.Init(questLines);
            Dungeon = dungeon;
            QuestLines.ConvertDataForCurrentDungeon(Dungeon.Parts);
            if (HasDataBeenLoaded) return;
            IsCompleted = false;
            HasSurrendered = false;
        }

        public void CompleteLevel()
        {
            IsCompleted = true;
        }

        public void GiveUpLevel()
        {
            HasSurrendered = true;
        }

        public bool HasCompleted()
        {
            return IsCompleted || HasSurrendered;
        }
    }
}