using System;
using System.Collections.Generic;
using Game.EnemyGenerator;
using Game.LevelManager;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.Events
{
    public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
    public class EnterRoomEventArgs : EventArgs
    {
        private Coordinates _roomCoordinates;
        private bool _roomHasEnemies;
        private Dictionary<EnemySO, int> _enemiesInRoom;
        private int _playerHealthWhenEntering;
        private Vector3 _roomPosition;
        private Dimensions _roomDimensions;

        public EnterRoomEventArgs(Coordinates roomCoordinates, bool roomHasEnemies, Dictionary<EnemySO, int> enemiesInEnemiesInRoom, int playerHealthWhenEntering, Vector3 roomPosition, Dimensions roomDimensions)
        {
            RoomCoordinates = roomCoordinates;
            RoomHasEnemies = roomHasEnemies;
            EnemiesInRoom = enemiesInEnemiesInRoom;
            PlayerHealthWhenEntering = playerHealthWhenEntering;
            RoomPosition = roomPosition;
            RoomDimensions = roomDimensions;
        }

        public Coordinates RoomCoordinates { get => _roomCoordinates; set => _roomCoordinates = value; }
        public bool RoomHasEnemies { get => _roomHasEnemies; set => _roomHasEnemies = value; }
        public  Dictionary<EnemySO, int> EnemiesInRoom { get => _enemiesInRoom; set => _enemiesInRoom = value; }
        public int PlayerHealthWhenEntering { get => _playerHealthWhenEntering; set => _playerHealthWhenEntering = value; }
        public Vector3 RoomPosition { get => _roomPosition; set => _roomPosition = value; }
        public Dimensions RoomDimensions { get => _roomDimensions; set => _roomDimensions = value; }
    }
}