using System;
using Game.Dialogues;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.MenuManager;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using UnityEngine;

namespace Game.DataCollection
{
    public class PlayerDataController : MonoBehaviour
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
            _gameplayData.SendProfileToServer(CurrentPlayer);
        }

        private void OnQuestEvent(object sender, NewQuestEventArgs eventArgs)
        {
            switch (eventArgs.Quest)
            {
                case AchievementQuestSo achievementQuest:
                    CurrentPlayer.CompletedAchievementQuests++;
                    GetAchievementTerminalAndUpdate(achievementQuest);
                    break;
                case CreativityQuestSo creativityQuest:
                    CurrentPlayer.CompletedCreativityQuests++;
                    GetCreativityTerminalAndUpdate(creativityQuest);
                    break;
                case ImmersionQuestSo immersionQuest:
                    CurrentPlayer.CompletedImmersionQuests++;
                    GetImmersionTerminalAndUpdate(immersionQuest);
                    break;
                case MasteryQuestSo masteryQuest:
                    CurrentPlayer.CompletedMasteryQuests++;
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
                    CurrentPlayer.CompletedExchangeQuests++;
                    break;
                case GatherQuestSo:
                    CurrentPlayer.CompletedGatherQuests++;
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
                    CurrentPlayer.CompletedExploreQuests++;
                    break;
                case GotoQuestSo:
                    CurrentPlayer.CompletedGoToQuests++;
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
                    CurrentPlayer.CompletedGiveQuests++;
                    break;
                case ListenQuestSo:
                    CurrentPlayer.CompletedListenQuests++;
                    break;
                case ReadQuestSo:
                    CurrentPlayer.CompletedReadQuests++;
                    break;
                case ReportQuestSo:
                    CurrentPlayer.CompletedReportQuests++;
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
                    CurrentPlayer.CompletedDamageQuests++;
                    break;
                case KillQuestSo:
                    CurrentPlayer.CompletedDamageQuests++;
                    break;
                default:
                    Debug.LogError("This mastery quest type does not exist!");
                    break;
            }        
        }
    }
}