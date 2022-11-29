using System;
using System.Collections;
using System.Collections.Generic;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class DungeonPlayer : PlaceableRoomObject
    {
        public List<int> Keys { get; } = new();
        public List<int> UsedKeys { get; } = new();

        [SerializeField] private Collider2D _playerTrigger;

        private static readonly float CooldownToUse = 2f;

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
            }
        }

        public static DungeonPlayer Instance { get; private set; }

        public void OnEnable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetValues;
            RoomBhv.StartRoomEventHandler += PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler += GetKey;
            DoorBhv.ExitRoomEventHandler += ExitRoom;
        }

        public void OnDisable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetValues;
            RoomBhv.StartRoomEventHandler -= PlacePlayerInStartRoom;
            KeyBhv.KeyCollectEventHandler -= GetKey;
            DoorBhv.ExitRoomEventHandler -= ExitRoom;
        }

        private void GetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            Keys.Add(eventArgs.KeyIndex);
        }

        private void PlacePlayerInStartRoom(object sender, StartRoomEventArgs e)
        {
            Instance.transform.position = e.position;
        }

        private void ExitRoom(object sender, ExitRoomEventArgs eventArgs)
        {
            Instance.transform.position = eventArgs.EntrancePosition;
            ExitRoomEventHandler?.Invoke(this, eventArgs);
            StartCoroutine(CountToUse());
        }

        private IEnumerator CountToUse()
        {
            _playerTrigger.enabled = false;
            yield return new WaitForSeconds(CooldownToUse);
            _playerTrigger.enabled = true;
        }

        private void ResetValues(object sender, EventArgs eventArgs)
        {
            Keys.Clear();
            UsedKeys.Clear();
        }
        
    }
}
