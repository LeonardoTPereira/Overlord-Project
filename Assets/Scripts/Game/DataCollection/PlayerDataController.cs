using System;
using Game.Dialogues;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.MenuManager;
using Game.NarrativeGenerator;
using UnityEngine;

namespace Game.DataCollection
{
    public class PlayerDataController : MonoBehaviour
    {
        public PlayerData CurrentPlayer { get; set; }
        private DungeonDataController _dungeonDataController;

        private void OnEnable()
        {
            DungeonLoader.StartMapEventHandler += OnMapStart;
            GameManagerSingleton.GameStartEventHandler += OnGameStart;
            HealthController.PlayerIsDamagedEventHandler += OnPlayerDamage;
            ProjectileController.EnemyHitEventHandler += IncrementCombo;
            ProjectileController.PlayerHitEventHandler += ResetCombo;
            BombController.PlayerHitEventHandler += ResetCombo;
            EnemyController.PlayerHitEventHandler += ResetCombo;
            TreasureController.TreasureCollectEventHandler += GetTreasure;
            KeyBhv.KeyCollectEventHandler += OnGetKey;
            EnemyController.KillEnemyEventHandler += OnKillEnemy;
            DialogueController.DialogueOpenEventHandler += OnInteractNPC;
            QuestGeneratorManager.ProfileSelectedEventHandler += OnProfileSelected;
            ExperimentController.ProfileSelectedEventHandler += OnExperimentProfileSelected;
            FormBHV.PreTestFormQuestionAnsweredEventHandler += OnPreTestFormAnswered;
            DoorBhv.KeyUsedEventHandler += OnKeyUsed;
            DungeonSceneManager.FinishMapEventHandler += OnMapComplete;
            PlayerController.PlayerDeathEventHandler += OnDeath;
            GameOverPanelBhv.ToLevelSelectEventHandler += OnFormNotAnswered;
            GameOverPanelBhv.RestartLevelEventHandler += OnFormNotAnswered;
            FormBHV.PostTestFormQuestionAnsweredEventHandler += OnPostTestFormAnswered;
        }

        private void OnDisable()
        {
            DungeonLoader.StartMapEventHandler -= OnMapStart;
            GameManagerSingleton.GameStartEventHandler -= OnGameStart;
            HealthController.PlayerIsDamagedEventHandler -= OnPlayerDamage;
            ProjectileController.EnemyHitEventHandler -= IncrementCombo;
            ProjectileController.PlayerHitEventHandler -= ResetCombo;
            BombController.PlayerHitEventHandler -= ResetCombo;
            EnemyController.PlayerHitEventHandler -= ResetCombo;
            TreasureController.TreasureCollectEventHandler -= GetTreasure;
            KeyBhv.KeyCollectEventHandler -= OnGetKey;
            FormBHV.PreTestFormQuestionAnsweredEventHandler -= OnPreTestFormAnswered;
            DoorBhv.KeyUsedEventHandler -= OnKeyUsed;
            QuestGeneratorManager.ProfileSelectedEventHandler -= OnProfileSelected;
            ExperimentController.ProfileSelectedEventHandler -= OnExperimentProfileSelected;            
            EnemyController.KillEnemyEventHandler -= OnKillEnemy;
            DialogueController.DialogueOpenEventHandler -= OnInteractNPC;
            DungeonSceneManager.FinishMapEventHandler -= OnMapComplete;
            PlayerController.PlayerDeathEventHandler -= OnDeath;
            FormBHV.PostTestFormQuestionAnsweredEventHandler -= OnPostTestFormAnswered;
        }

        private void Start()
        {
            _dungeonDataController = GetComponent<DungeonDataController>();
        }

        private void OnGameStart(object sender, EventArgs eventArgs)
        {
            CurrentPlayer = ScriptableObject.CreateInstance<PlayerData>();
            CurrentPlayer.Init();
        }
        
        private void OnMapStart(object sender, StartMapEventArgs eventArgs)
        {
            CurrentPlayer.StartDungeon(eventArgs.MapName, eventArgs.Map);
            _dungeonDataController.CurrentDungeon = CurrentPlayer.CurrentDungeon;
        }
        

        private void OnProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.PlayerProfile = eventArgs.PlayerProfile;
        }

        private void OnExperimentProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.GivenPlayerProfile = eventArgs.PlayerProfile;
        }
        
        private void ResetCombo(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.ResetCombo();
        }
        
        private void IncrementCombo(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementCombo();
        }
        
        private void OnPreTestFormAnswered(object sender, FormAnsweredEventArgs eventArgs)
        {
            CurrentPlayer.PreFormAnswers = eventArgs.AnswerValue;
        }
        
        private void OnKillEnemy(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementKills();
        }

        private void OnInteractNPC(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementInteractionsWithNpcs();
        }
        private void OnDeath(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementDeaths();
        }
        
        private void OnMapComplete(object sender, FinishMapEventArgs eventArgs)
        {
            CurrentPlayer.IncrementWins();
        }
        
        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            CurrentPlayer.AddLostHealth(eventArgs.DamageDone);
        }
        
        private void GetTreasure(object sender, TreasureCollectEventArgs eventArgs)
        {
            CurrentPlayer.AddCollectedTreasure(eventArgs.Amount);
        }
        
        private void OnGetKey(object sender, KeyCollectEventArgs eventArgs)
        {
            CurrentPlayer.IncrementCollectedKeys();
        }

        private void OnKeyUsed(object sender, KeyUsedEventArgs eventArgs)
        {
            CurrentPlayer.IncrementOpenedLocks();
        }

        private void OnFormNotAnswered(object sender, EventArgs eventArgs)
        {
#if UNITY_EDITOR
            CurrentPlayer.SaveAndRefreshAssets();
            CurrentPlayer.RefreshJson();
#endif
        }
        
        private void OnPostTestFormAnswered(object sender, FormAnsweredEventArgs eventArgs)
        {
            CurrentPlayer.AddPostTestDataToDungeon(eventArgs.AnswerValue);
        }
    }
}