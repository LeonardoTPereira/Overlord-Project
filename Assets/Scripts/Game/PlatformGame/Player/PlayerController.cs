using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Events;
using Game.GameManager;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using PlatformGame.Weapons;
using PlatformGame.Dungeon;


namespace PlatformGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        
        private Camera _camera;
        [SerializeField] private CinemachineConfiner2D _confiner;

        public void Awake()
        {
            _camera = Camera.main;
        }
        public void OnEnable()
        {
            RoomBhv.EnterRoomEventHandler += AdjustCamera;
            
        }

        public void OnDisable()
        {
           
            RoomBhv.EnterRoomEventHandler -= AdjustCamera;
            
        }
        
        private void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            PlatformRoomBhv platRoom = sender as PlatformRoomBhv;
            if (platRoom != null)
            {
                _confiner.m_BoundingShape2D = platRoom.colRoomConfiner;
            }
        }

    }
}


