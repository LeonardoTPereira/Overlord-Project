using Game.DataCollection;
using Game.EnemyManager;
using Game.LevelManager;
using System;
using UnityEngine;
using Util;
using Game.GameManager.Player;
using Game.GameManager;
using PlatformGame.Player;
using System.Collections;
using Overlord.ProfileAnalyst;

namespace Game.Events
{
    public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
    public class EnterRoomEventArgs : EventArgs
    {
        public RoomData RoomData { get; set; }
        public Vector3 PositionInScene { get; set; }
        public int PlayerHealthWhenEntering { get; set; }        

        public EnterRoomEventArgs(Coordinates roomCoordinates, Dimensions roomDimensions, EnemyByAmountDictionary enemiesInRoom, Vector3 roomPosition)
        {
            var gameType = GameManagerSingleton.Instance.GameType;
            PositionInScene = roomPosition;

            if (gameType == Enums.GameType.TopDown)
                PlayerHealthWhenEntering = DungeonPlayer.Instance.GetComponent<Game.GameManager.Player.PlayerController>().GetHealth();
            else if (gameType == Enums.GameType.Platformer)
            {
                PlayerHealthWhenEntering = GameObject.FindWithTag("Player").GetComponent<PlatformGame.Player.PlayerHealth>().GetHealth();
            }

            var enterTime = Time.realtimeSinceStartup;
            RoomData = ScriptableObject.CreateInstance<RoomData>();
            RoomData.Init(roomCoordinates, roomDimensions, enemiesInRoom, enterTime);
        }
    }
}