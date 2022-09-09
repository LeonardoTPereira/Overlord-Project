using System;
using System.Collections.Generic;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using MyBox;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class Player : PlaceableRoomObject, ISoundEmitter
    {
        public List<int> keys = new ();
        public List<int> usedKeys = new ();
        public int X { private set; get; }
        public int Y { private set; get; }
        public Camera cam;
        public Camera minimap;
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
                cam = Camera.main;
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
            keys.Add(eventArgs.KeyIndex);
        }

        public void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            var roomWidth = eventArgs.RoomData.RoomDimensions.Width;
            var cameraXPosition = eventArgs.PositionInScene.x;
            var cameraYPosition = eventArgs.PositionInScene.y;
            const float cameraZPosition = -5f;
            cam.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
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
            keys.Clear();
            usedKeys.Clear();
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
