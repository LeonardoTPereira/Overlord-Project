using System;
using System.Collections.Generic;
using Game.Events;
using Game.LevelManager;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class Player : PlaceableRoomObject
    {
        private static readonly int GET_KEY = 0;
        private static readonly int HIT_PLAYER = 1;
        private static readonly int PLAYER_DEATH = 2;

        private static Player instance = null;
        public List<int> keys = new List<int>();
        public List<int> usedKeys = new List<int>();
        public int x { private set; get; }
        public int y { private set; get; }
        public Camera cam;
        public Camera minimap;
        private AudioSource[] audioSrcs;
        private PlayerController playerController;
        public static event EnterRoomEvent EnterRoomEventHandler;
        public static event ExitRoomEvent ExitRoomEventHandler;

        public void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                cam = Camera.main;
                audioSrcs = GetComponents<AudioSource>();
                playerController = GetComponent<PlayerController>();
            }
        }

        public static Player Instance { get { return instance; } }

        public void OnEnable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler += ResetValues;
            RoomBhv.StartRoomEventHandler += PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler += GetKey;
            GameManagerSingleton.EnterRoomEventHandler += GetHealth;
            GameManagerSingleton.EnterRoomEventHandler += AdjustCamera;
            RoomBhv.EnterRoomEventHandler += GetHealth;
            RoomBhv.EnterRoomEventHandler += AdjustCamera;
            DoorBhv.ExitRoomEventHandler += ExitRoom;
            EnemyController.playerHitEventHandler += HurtPlayer;
            ProjectileController.playerHitEventHandler += HurtPlayer;
            BombController.PlayerHitEventHandler += HurtPlayer;
            PlayerController.PlayerDeathEventHandler += KillPlayer;
        }

        public void OnDisable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler -= ResetValues;
            RoomBhv.StartRoomEventHandler -= PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler -= GetKey;
            GameManagerSingleton.EnterRoomEventHandler -= GetHealth;
            GameManagerSingleton.EnterRoomEventHandler -= AdjustCamera;
            RoomBhv.EnterRoomEventHandler -= GetHealth;
            RoomBhv.EnterRoomEventHandler -= AdjustCamera;
            DoorBhv.ExitRoomEventHandler -= ExitRoom;
            EnemyController.playerHitEventHandler -= HurtPlayer;
            ProjectileController.playerHitEventHandler -= HurtPlayer;
            BombController.PlayerHitEventHandler -= HurtPlayer;
            PlayerController.PlayerDeathEventHandler -= KillPlayer;
        }

        private void GetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            audioSrcs[GET_KEY].PlayOneShot(audioSrcs[GET_KEY].clip, 0.6f);
            keys.Add(eventArgs.KeyIndex);
        }

        public void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            var roomWidth = eventArgs.RoomDimensions.Width;
            var cameraXPosition = eventArgs.RoomPosition.x + roomWidth / 3.5f;
            var cameraYPosition = eventArgs.RoomPosition.y;
            var cameraZPosition = -5f;
            cam.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
            //minimap.transform.position = new Vector3(roomTransf.position.x, roomTransf.position.y, -5f);
        }

        private void PlacePlayerInStartRoom(object sender, StartRoomEventArgs e)
        {
            instance.transform.position = e.position;
        }

        private void ExitRoom(object sender, ExitRoomEventArgs eventArgs)
        {
            Instance.transform.position = eventArgs.EntrancePosition;
            eventArgs.PlayerHealthWhenExiting = playerController.GetHealth();
            ExitRoomEventHandler?.Invoke(this, eventArgs);
        }

        private void ResetValues(object sender, EventArgs eventArgs)
        {
            keys.Clear();
            usedKeys.Clear();
            playerController.ResetHealth();
        }
        private void GetHealth(object sender, EnterRoomEventArgs eventArgs)
        {
            int health = playerController.GetHealth();
            eventArgs.PlayerHealthWhenEntering = health;
            EnterRoomEventHandler(this, eventArgs);
        }

        private void HurtPlayer(object sender, EventArgs eventArgs)
        {
            if (playerController.GetHealth() > 0 && !playerController.IsInvincible())
            {
                audioSrcs[HIT_PLAYER].PlayOneShot(audioSrcs[HIT_PLAYER].clip, 1.0f);
            }
        }

        private void KillPlayer(object sender, EventArgs eventArgs)
        {
            audioSrcs[PLAYER_DEATH].PlayOneShot(audioSrcs[PLAYER_DEATH].clip, 1.0f);
        }
    }
}
