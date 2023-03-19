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
            //_confiner.m_BoundingShape2D = eventArgs._roomCompositeColl;
            /*
            var cameraXPosition = eventArgs.PositionInScene.x;
            var cameraYPosition = eventArgs.PositionInScene.y;
            const float cameraZPosition = -5f;
            _camera.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
            */
        }

    }
}
