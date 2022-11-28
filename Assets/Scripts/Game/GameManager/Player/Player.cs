using System;
using System.Collections.Generic;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class Player : PlaceableRoomObject, ISoundEmitter
    {
        public List<int> Keys { get; } = new();
        public List<int> UsedKeys { get; } = new();
        private Camera _camera;
        private PlayerController _playerController;
        public static event ExitRoomEvent ExitRoomEventHandler;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _camera = Camera.main;
                _playerController = GetComponent<PlayerController>();
            }
        }

        public static Player Instance { get; private set; }

        public void OnEnable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetValues;
            RoomBhv.StartRoomEventHandler += PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler += GetKey;
            RoomBhv.EnterRoomEventHandler += AdjustCamera;
            DoorBhv.ExitRoomEventHandler += ExitRoom;
            EnemyController.PlayerHitEventHandler += HurtPlayer;
            ProjectileController.PlayerHitEventHandler += HurtPlayer;
            BombController.PlayerHitEventHandler += HurtPlayer;
            PlayerController.PlayerDeathEventHandler += KillPlayer;
        }

        public void OnDisable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetValues;
            RoomBhv.StartRoomEventHandler -= PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler -= GetKey;
            RoomBhv.EnterRoomEventHandler -= AdjustCamera;
            DoorBhv.ExitRoomEventHandler -= ExitRoom;
            EnemyController.PlayerHitEventHandler -= HurtPlayer;
            ProjectileController.PlayerHitEventHandler -= HurtPlayer;
            BombController.PlayerHitEventHandler -= HurtPlayer;
            PlayerController.PlayerDeathEventHandler -= KillPlayer;
        }

        private void GetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.ItemPickup));
            Keys.Add(eventArgs.KeyIndex);
        }

        private void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            var cameraXPosition = eventArgs.PositionInScene.x;
            var cameraYPosition = eventArgs.PositionInScene.y;
            const float cameraZPosition = -5f;
            _camera.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
        }

        private void PlacePlayerInStartRoom(object sender, StartRoomEventArgs e)
        {
            Instance.transform.position = e.position;
        }

        private void ExitRoom(object sender, ExitRoomEventArgs eventArgs)
        {
            Instance.transform.position = eventArgs.EntrancePosition;
            eventArgs.PlayerHealthWhenExiting = _playerController.GetHealth();
            ExitRoomEventHandler?.Invoke(this, eventArgs);
        }

        private void ResetValues(object sender, EventArgs eventArgs)
        {
            Keys.Clear();
            UsedKeys.Clear();
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
