using System;
using System.Collections.Generic;
using Game.LevelManager;
using UnityEngine;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable, CreateAssetMenu]
    public class DungeonFileSo : ScriptableObject
    {
        [SerializeField]
        public Dimensions dimensions;
        [SerializeField]
        public List<SORoom> rooms;
        public float fitness;

        private int _currentIndex = 0;

        public void ResetIndex()
        {
            _currentIndex = 0;
        }

        public DungeonPart GetNextPart()
        {
            if (_currentIndex < rooms.Count)
                return DungeonPartFactory.CreateDungeonPartFromDungeonFileSO(rooms[_currentIndex++]);
            return null;
        }
    }
}