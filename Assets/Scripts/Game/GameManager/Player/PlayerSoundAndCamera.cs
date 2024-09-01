using System;
using System.Collections.Generic;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class PlayerSoundAndCamera : MonoBehaviour, ISoundEmitter
    {
        private Camera _camera;
        private PlayerController _playerController;
        
        public void Awake()
        {
            _camera = Camera.main;
            _playerController = GetComponent<PlayerController>();
        }

        public void OnEnable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetValues;
            RoomBhv.EnterRoomEventHandler += AdjustCamera;
            DoorBhv.ExitRoomEventHandler += ExitRoom;
            EnemyController.PlayerHitEventHandler += HurtPlayer;
            ProjectileController.PlayerHitEventHandler += HurtPlayer;
            BombController.PlayerHitEventHandler += HurtPlayer;
            PlayerController.PlayerDeathEventHandler += KillPlayer;
            KeyBhv.KeyCollectEventHandler += GetKey;
        }

        public void OnDisable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetValues;
            RoomBhv.EnterRoomEventHandler -= AdjustCamera;
            DoorBhv.ExitRoomEventHandler -= ExitRoom;
            EnemyController.PlayerHitEventHandler -= HurtPlayer;
            ProjectileController.PlayerHitEventHandler -= HurtPlayer;
            BombController.PlayerHitEventHandler -= HurtPlayer;
            PlayerController.PlayerDeathEventHandler -= KillPlayer;
            KeyBhv.KeyCollectEventHandler -= GetKey;
        }

        private void GetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.ItemPickup));
        }

        private void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            var cameraXPosition = eventArgs.PositionInScene.x+eventArgs.RoomData.RoomDimensions.Width/2f;
            var cameraYPosition = eventArgs.PositionInScene.y+eventArgs.RoomData.RoomDimensions.Height/2f;
            const float cameraZPosition = -5f;
            _camera.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
        }

        private void ExitRoom(object sender, ExitRoomEventArgs eventArgs)
        {
            eventArgs.PlayerHealthWhenExiting = _playerController.GetHealth();
        }

        private void ResetValues(object sender, EventArgs eventArgs)
        {
            _playerController.ResetHealth();
        }

        private void HurtPlayer(object sender, EventArgs eventArgs)
        {
            if (_playerController.GetHealth() > 0 && !_playerController.IsInvincible())
            {
                ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.PlayerHit));
            }
        }

        private void KillPlayer(object sender, EventArgs eventArgs)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.PlayerDeath));
        }
    }
}
