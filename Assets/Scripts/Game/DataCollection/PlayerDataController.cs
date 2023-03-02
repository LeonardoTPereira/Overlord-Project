using System;
using Game.Dialogues;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.LevelSelection;
using Game.MenuManager;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using Game.SaveLoadSystem;
using UnityEngine;

namespace Game.DataCollection
{
    public class PlayerDataController : MonoBehaviour, ISaveable
    {
        public PlayerData CurrentPlayer { get; private set; }
        private DungeonDataController _dungeonDataController;
        private GameplayData _gameplayData;

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
            FormBhv.PreTestFormQuestionAnsweredEventHandler += OnPreTestFormAnswered;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler += OnPreTestFormAnswered;
            DoorBhv.KeyUsedEventHandler += OnKeyUsed;
            TriforceBhv.GotTriforceEventHandler += OnMapComplete;
            PlayerController.PlayerDeathEventHandler += OnDeath;
            GameOverPanelBhv.ToLevelSelectEventHandler += OnFormNotAnswered;
            GameOverPanelBhv.RestartLevelEventHandler += OnFormNotAnswered;
            FormBhv.PostTestFormQuestionAnsweredEventHandler += OnPostTestFormAnswered;
            QuestLine.QuestCompletedEventHandler += OnQuestEvent;
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
            FormBhv.PreTestFormQuestionAnsweredEventHandler -= OnPreTestFormAnswered;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler -= OnPreTestFormAnswered;
            DoorBhv.KeyUsedEventHandler -= OnKeyUsed;
            QuestGeneratorManager.ProfileSelectedEventHandler -= OnProfileSelected;
            ExperimentController.ProfileSelectedEventHandler -= OnExperimentProfileSelected;
            EnemyController.KillEnemyEventHandler -= OnKillEnemy;
            DialogueController.DialogueOpenEventHandler -= OnInteractNPC;
            TriforceBhv.GotTriforceEventHandler -= OnMapComplete;
            PlayerController.PlayerDeathEventHandler -= OnDeath;
            FormBhv.PostTestFormQuestionAnsweredEventHandler -= OnPostTestFormAnswered;
            QuestLine.QuestCompletedEventHandler -= OnQuestEvent;
        }

        private void Awake()
        {
            _gameplayData = new GameplayData();
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
            Debug.Log("Map Started");
            CurrentPlayer.StartDungeon(eventArgs.MapName, eventArgs.Map);
            _dungeonDataController.CurrentDungeon = CurrentPlayer.CurrentDungeon;
            _dungeonDataController.SetDungeonParameters();
        }


        private void OnProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.SerializedData.PlayerProfile = eventArgs.PlayerProfile;
        }

        private void OnExperimentProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.SerializedData.GivenPlayerProfile = eventArgs.PlayerProfile;
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
            CurrentPlayer.SerializedData.PreFormAnswers = eventArgs.AnswerValue;
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

        private void OnMapComplete(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementWins();
        }

        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            CurrentPlayer.AddLostHealth(eventArgs.DamageDone);
        }

        private void GetTreasure(object sender, TreasureCollectEventArgs eventArgs)
        {
            CurrentPlayer.AddCollectedTreasure(eventArgs.QuestId);
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
            CurrentPlayer.AddPostTestDataToDungeon(null);
            _gameplayData.SendProfileToServer(CurrentPlayer);
        }

        private void OnPostTestFormAnswered(object sender, FormAnsweredEventArgs eventArgs)
        {
            CurrentPlayer.AddPostTestDataToDungeon(eventArgs.AnswerValue);
            _gameplayData.SendProfileToServer(CurrentPlayer);
        }

        private void OnQuestEvent(object sender, NewQuestEventArgs eventArgs)
        {
            switch (eventArgs.Quest)
            {
                case AchievementQuestSo achievementQuest:
                    CurrentPlayer.SerializedData.CompletedAchievementQuests++;
                    GetAchievementTerminalAndUpdate(achievementQuest);
                    break;
                case CreativityQuestSo creativityQuest:
                    CurrentPlayer.SerializedData.CompletedCreativityQuests++;
                    GetCreativityTerminalAndUpdate(creativityQuest);
                    break;
                case ImmersionQuestSo immersionQuest:
                    CurrentPlayer.SerializedData.CompletedImmersionQuests++;
                    GetImmersionTerminalAndUpdate(immersionQuest);
                    break;
                case MasteryQuestSo masteryQuest:
                    CurrentPlayer.SerializedData.CompletedMasteryQuests++;
                    GetMasteryTerminalAndUpdate(masteryQuest);
                    break;
                default:
                    Debug.LogError("This Quest non-terminal is non-existent!");
                    break;
            }
        }


        private void GetAchievementTerminalAndUpdate(AchievementQuestSo achievementQuest)
        {
            switch (achievementQuest)
            {
                case ExchangeQuestSo:
                    CurrentPlayer.SerializedData.CompletedExchangeQuests++;
                    break;
                case GatherQuestSo:
                    CurrentPlayer.SerializedData.CompletedGatherQuests++;
                    break;
                default:
                    Debug.LogError("This achievement quest type does not exist!");
                    break;
            }
        }
        
        private void GetCreativityTerminalAndUpdate(CreativityQuestSo creativityQuest)
        {
            switch (creativityQuest)
            {
                case ExploreQuestSo:
                    CurrentPlayer.SerializedData.CompletedExploreQuests++;
                    break;
                case GotoQuestSo:
                    CurrentPlayer.SerializedData.CompletedGoToQuests++;
                    break;
                default:
                    Debug.LogError("This creativity quest type does not exist!");
                    break;
            }
        }
        
        private void GetImmersionTerminalAndUpdate(ImmersionQuestSo immersionQuest)
        {
            switch (immersionQuest)
            {
                case GiveQuestSo:
                    CurrentPlayer.SerializedData.CompletedGiveQuests++;
                    break;
                case ListenQuestSo:
                    CurrentPlayer.SerializedData.CompletedListenQuests++;
                    break;
                case ReadQuestSo:
                    CurrentPlayer.SerializedData.CompletedReadQuests++;
                    break;
                case ReportQuestSo:
                    CurrentPlayer.SerializedData.CompletedReportQuests++;
                    break;
                default:
                    Debug.LogError("This immersion quest type does not exist!");
                    break;
            }
        }
        
        private void GetMasteryTerminalAndUpdate(MasteryQuestSo masteryQuest)
        {
            switch (masteryQuest)
            {
                case DamageQuestSo:
                    CurrentPlayer.SerializedData.CompletedDamageQuests++;
                    break;
                case KillQuestSo:
                    CurrentPlayer.SerializedData.CompletedKillQuests++;
                    break;
                default:
                    Debug.LogError("This mastery quest type does not exist!");
                    break;
            }        
        }

        public object SaveState()
        {
	        return CurrentPlayer.SaveState();
        }

        public void LoadState(object state)
        {
	        CurrentPlayer.LoadState(state);
        }
    }
}