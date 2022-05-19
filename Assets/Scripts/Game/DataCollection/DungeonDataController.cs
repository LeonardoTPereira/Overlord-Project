using System;
using Game.Dialogues;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonManager;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Game.DataCollection
{
    public class DungeonDataController : MonoBehaviour
    {
        public DungeonData CurrentDungeon { get; set; }
        private const string PostDataURL = "http://damicore.icmc.usp.br/pag/data/upload.php?";

        private void OnEnable()
        {
            HealthController.PlayerIsDamagedEventHandler += OnPlayerDamage;
            ProjectileController.EnemyHitEventHandler += IncrementCombo;
            ProjectileController.PlayerHitEventHandler += ResetCombo;
            BombController.PlayerHitEventHandler += ResetCombo;
            EnemyController.PlayerHitEventHandler += ResetCombo;
            TreasureController.TreasureCollectEventHandler += GetTreasure;
            KeyBhv.KeyCollectEventHandler += OnGetKey;
            EnemyController.KillEnemyEventHandler += OnKillEnemy;
            DialogueController.DialogueOpenEventHandler += OnInteractNPC;
            DoorBhv.KeyUsedEventHandler += OnKeyUsed;
            RoomBhv.EnterRoomEventHandler += OnRoomEnter;            
            TriforceBhv.GotTriforceEventHandler += OnMapComplete;
            PlayerController.PlayerDeathEventHandler += OnDeath;
            Player.ExitRoomEventHandler += OnRoomExit;
        }

        private void OnDisable()
        {
            HealthController.PlayerIsDamagedEventHandler -= OnPlayerDamage;
            ProjectileController.EnemyHitEventHandler -= IncrementCombo;
            ProjectileController.PlayerHitEventHandler -= ResetCombo;
            BombController.PlayerHitEventHandler -= ResetCombo;
            EnemyController.PlayerHitEventHandler -= ResetCombo;
            TreasureController.TreasureCollectEventHandler -= GetTreasure;
            KeyBhv.KeyCollectEventHandler -= OnGetKey;
            DoorBhv.KeyUsedEventHandler -= OnKeyUsed;
            EnemyController.KillEnemyEventHandler -= OnKillEnemy;
            DialogueController.DialogueOpenEventHandler -= OnInteractNPC;
            RoomBhv.EnterRoomEventHandler -= OnRoomEnter;
            TriforceBhv.GotTriforceEventHandler -= OnMapComplete;
            PlayerController.PlayerDeathEventHandler -= OnDeath;
            Player.ExitRoomEventHandler -= OnRoomExit;
        }
        


        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            CurrentDungeon.AddLostHealth(eventArgs.DamageDone);        
        }

        private void ResetCombo(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.ResetCombo();
        }
        
        private void IncrementCombo(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.IncrementCombo();
        }

        private void GetTreasure(object sender, TreasureCollectEventArgs eventArgs)
        {
            CurrentDungeon.AddCollectedTreasure(eventArgs.Amount);

        }

        private void OnGetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            CurrentDungeon.IncrementCollectedKeys();

        }

        private void OnKeyUsed(object sender, KeyUsedEventArgs eventArgs)
        {
            CurrentDungeon.IncrementOpenedLocks();

        }
        
        private void OnKillEnemy(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.IncrementKills();
        }

        private void OnInteractNPC(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.IncrementInteractionsWithNpcs();
        }
        
        private void OnRoomEnter(object sender, EnterRoomEventArgs eventArgs)
        {
            CurrentDungeon.OnRoomEnter(eventArgs.RoomData);
        }
        
        private void OnRoomExit(object sender, ExitRoomEventArgs eventArgs)
        {
            CurrentDungeon.OnRoomExit();
        }
        
        private void OnDeath(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.OnPlayerDeath();
        }
        
        
        
        private void OnMapComplete(object sender, EventArgs eventArgs)
        {
            CurrentDungeon.OnPlayerVictory();
        }
#if UNITY_WEBGL
        public void SendJsonToServer()
        {
            StartCoroutine(PostData());
        }
        
        private IEnumerator PostData()
        {
            //TODO post Room Data
            var data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(CurrentDungeon));
            var form = new WWWForm();
            form.AddField("name", CurrentDungeon.LevelName);
            form.AddBinaryData("level", data, CurrentDungeon.LevelName + "-Dungeon" + ".json", "application/json");
            using var www = UnityWebRequest.Post(PostDataURL, form);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
        }
#endif
    }
}