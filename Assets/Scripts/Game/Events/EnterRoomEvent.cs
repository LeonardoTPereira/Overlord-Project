using System;
using Game.DataCollection;
using Game.EnemyManager;
using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.Events
{
    public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
    public class EnterRoomEventArgs : EventArgs
    {
        public RoomData RoomData { get; set; }
        public Vector3 PositionInScene { get; set; }
        
        public EnterRoomEventArgs(Coordinates roomCoordinates, Dimensions roomDimensions, EnemyByAmountDictionary enemiesInRoom, Vector3 roomPosition)
        {
            PositionInScene = roomPosition;
            var enterTime = Time.realtimeSinceStartup;
            RoomData = ScriptableObject.CreateInstance<RoomData>();
            RoomData.Init(roomCoordinates, roomDimensions, enemiesInRoom, enterTime);
        }
    }
}