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
        [field:SerializeField] public QuestLine Quests { get; set; }
        [field:SerializeField] public DungeonFileSo Dungeon { get; set; }
        [SerializeField] private bool _completed;
        [SerializeField] private bool _surrendered;

        public void Init(QuestLine quests, DungeonFileSo dungeon)
        {
            Quests = quests;
            Dungeon = dungeon;
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