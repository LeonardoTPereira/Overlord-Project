using Game.DataCollection;
using Game.EnemyManager;
using Game.LevelManager;
using System;
using UnityEngine;
using Util;
using Game.GameManager.Player;

namespace Game.Events
{
    public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
    public class EnterRoomEventArgs : EventArgs
    {
        public RoomData RoomData { get; set; }
        public Vector3 PositionInScene { get; set; }
<<<<<<< HEAD
        public int PlayerHealthWhenEntering { get; set; }        
=======
>>>>>>> Develop

        public EnterRoomEventArgs(Coordinates roomCoordinates, Dimensions roomDimensions, EnemyByAmountDictionary enemiesInRoom, Vector3 roomPosition)
        {
            PositionInScene = roomPosition;
            PlayerHealthWhenEntering = DungeonPlayer.Instance.GetComponent<PlayerController>().GetHealth();
            var enterTime = Time.realtimeSinceStartup;
            RoomData = ScriptableObject.CreateInstance<RoomData>();
            RoomData.Init(roomCoordinates, roomDimensions, enemiesInRoom, enterTime);
        }
    }
}