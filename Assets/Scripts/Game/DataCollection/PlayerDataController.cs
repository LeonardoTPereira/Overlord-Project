using Overlord.NarrativeGenerator.Quests;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.LevelSelection;
using Game.MenuManager;
using Topdown.Overlord.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using Game.SaveLoadSystem;
using Game.NPCs;
using System;
using UnityEngine;
using Topdown.Overlord.ProfileAnalyst;

namespace Game.DataCollection
{
    public class PlayerDataController : MonoBehaviour, ISaveable
    {
        public ExperimentController ExperimentController;
        public PlayerData CurrentPlayer { get; private set; }
        private DungeonDataController _dungeonDataController;
        private GameplayData _gameplayData;

        private void OnEnable()
        {
            DungeonLoader.StartMapEventHandler += OnMapStart;
            // GameManagerSingleton.GameStartEventHandler += OnGameStart;
            HealthController.PlayerIsDamagedEventHandler += OnPlayerDamage;
            PlayerController.InitializePlayerHealthEventHandler += OnPlayerHealthInitialize;
            ProjectileController.EnemyHitEventHandler += IncrementCombo;
            ProjectileController.PlayerHitEventHandler += ResetCombo;
            BombController.PlayerHitEventHandler += ResetCombo;
            EnemyController.PlayerHitEventHandler += ResetCombo;
            TreasureController.TreasureCollectEventHandler += CollectItem;
            ReadableItemController.ReadableItemInteraction += ReadItem;
            KeyBhv.KeyCollectEventHandler += OnGetKey;
            NpcController.KeyCollectEventHandler += OnGetKey;
            EnemyController.KillEnemyEventHandler += OnKillEnemy;
            NpcController.NpcInteraction += OnInteractNPC;
            TopdownQuestGeneratorManager.ProfileSelectedEventHandler += OnProfileSelected;
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
            QuestLine.QuestLineOpenedEventHandler += OnQuestlineOpenedEvent;
            TopdownPlayerProfileManager.GameplayProfileSelectedEventHandler += OnPlayerProfileUpdated;
        }

        private void OnDisable()
        {
            DungeonLoader.StartMapEventHandler -= OnMapStart;
            // GameManagerSingleton.GameStartEventHandler -= OnGameStart;
            HealthController.PlayerIsDamagedEventHandler -= OnPlayerDamage;
            PlayerController.InitializePlayerHealthEventHandler -= OnPlayerHealthInitialize;
            ProjectileController.EnemyHitEventHandler -= IncrementCombo;
            ProjectileController.PlayerHitEventHandler -= ResetCombo;
            BombController.PlayerHitEventHandler -= ResetCombo;
            EnemyController.PlayerHitEventHandler -= ResetCombo;
            TreasureController.TreasureCollectEventHandler -= CollectItem;
            ReadableItemController.ReadableItemInteraction -= ReadItem;
            KeyBhv.KeyCollectEventHandler -= OnGetKey;
            NpcController.KeyCollectEventHandler += OnGetKey;
            FormBhv.PreTestFormQuestionAnsweredEventHandler -= OnPreTestFormAnswered;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler -= OnPreTestFormAnswered;
            DoorBhv.KeyUsedEventHandler -= OnKeyUsed;
            TopdownQuestGeneratorManager.ProfileSelectedEventHandler -= OnProfileSelected;
            ExperimentController.ProfileSelectedEventHandler -= OnExperimentProfileSelected;
            EnemyController.KillEnemyEventHandler -= OnKillEnemy;
            NpcController.NpcInteraction -= OnInteractNPC;
            TriforceBhv.GotTriforceEventHandler -= OnMapComplete;
            PlayerController.PlayerDeathEventHandler -= OnDeath;
            FormBhv.PostTestFormQuestionAnsweredEventHandler -= OnPostTestFormAnswered;
            QuestLine.QuestCompletedEventHandler -= OnQuestEvent;
            QuestLine.QuestLineOpenedEventHandler -= OnQuestlineOpenedEvent;
            TopdownPlayerProfileManager.GameplayProfileSelectedEventHandler -= OnPlayerProfileUpdated;
        }

        private void Awake()
        {
            _gameplayData = new GameplayData();
        }

        private void Start()
        {
            _dungeonDataController = GetComponent<DungeonDataController>();
            OnGameStart(null, null);
        }

        private void OnGameStart(object sender, EventArgs eventArgs)
        {
            CurrentPlayer = ScriptableObject.CreateInstance<PlayerData>();
            CurrentPlayer.Init( ExperimentController.UseFixedProfile );
        }

        private void OnMapStart(object sender, StartMapEventArgs eventArgs)
        {
            CurrentPlayer.StartDungeon(eventArgs.MapName, eventArgs.Map);
            _dungeonDataController.CurrentDungeon = CurrentPlayer.CurrentDungeon;
            _dungeonDataController.SetDungeonParameters();
        }


        private void OnProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.SerializedData.GivenPlayerProfile = eventArgs.PlayerProfile;
        }

        private void OnPlayerProfileUpdated(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.SerializedData.PreviousPlayerProfiles.Add( CurrentPlayer.SerializedData.GivenPlayerProfile );
            CurrentPlayer.SerializedData.PlayerProfile = eventArgs.PlayerProfile;
        }

        private void OnExperimentProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
        {
            CurrentPlayer.SerializedData.PlayerProfile = eventArgs.PlayerProfile;
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

        private void OnKillEnemy(object sender, KillEnemyEventArgs eventArgs)
        {
            CurrentPlayer.IncrementKills(eventArgs.EnemyTypeString);
        }

        private void OnInteractNPC(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementNpcInteractions();
        }

        private void OnDeath(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementDeaths();
        }

        private void OnMapComplete(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.IncrementWins();
        }

        private void OnPlayerHealthInitialize(object sender, InitializePlayerHealthEventArgs eventArgs )
        {
            CurrentPlayer.InitializeHealth(eventArgs.PlayerHealth);
        }

        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            CurrentPlayer.AddLostHealth(eventArgs.DamageDone);
        }

        private void CollectItem(object sender, TreasureCollectEventArgs eventArgs)
        {
            CurrentPlayer.AddCollectedItem(eventArgs.Amount);
        }

        private void ReadItem(object sender, EventArgs eventArgs)
        {
            CurrentPlayer.AddReadItem(1);
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

        private void OnQuestlineOpenedEvent(object sender, NewQuestLineEventArgs eventArgs)
        {
            foreach (QuestSo quest in eventArgs.QuestLine.Quests)
            {
                switch (quest)
                {
                    case AchievementQuestSo achievementQuest:
                        CurrentPlayer.SerializedData.TotalAchievementQuests++;
                        GetAchievementTerminalAndUpdateTotal(achievementQuest);
                        break;
                    case CreativityQuestSo creativityQuest:
                        CurrentPlayer.SerializedData.TotalCreativityQuests++;
                        GetCreativityTerminalAndUpdateTotal(creativityQuest);
                        break;
                    case ImmersionQuestSo immersionQuest:
                        CurrentPlayer.SerializedData.TotalImmersionQuests++;
                        GetImmersionTerminalAndUpdateTotal(immersionQuest);
                        break;
                    case MasteryQuestSo masteryQuest:
                        CurrentPlayer.SerializedData.TotalMasteryQuests++;
                        GetMasteryTerminalAndUpdateTotal(masteryQuest);
                        break;
                    default:
                        Debug.LogError("This Quest non-terminal is non-existent!");
                        break;
                }
            }
        }


        private void GetAchievementTerminalAndUpdateTotal(AchievementQuestSo achievementQuest)
        {
            switch (achievementQuest)
            {
                case ExchangeQuestSo:
                    CurrentPlayer.SerializedData.TotalExchangeQuests++;
                    break;
                case GatherQuestSo:
                    CurrentPlayer.SerializedData.TotalGatherQuests++;
                    break;
                default:
                    Debug.LogError("This achievement quest type does not exist!");
                    break;
            }
        }
        
        private void GetCreativityTerminalAndUpdateTotal(CreativityQuestSo creativityQuest)
        {
            switch (creativityQuest)
            {
                case ExploreQuestSo:
                    CurrentPlayer.SerializedData.TotalExploreQuests++;
                    break;
                case GotoQuestSo:
                    CurrentPlayer.SerializedData.TotalGoToQuests++;
                    break;
                default:
                    Debug.LogError("This creativity quest type does not exist!");
                    break;
            }
        }
        
        private void GetImmersionTerminalAndUpdateTotal(ImmersionQuestSo immersionQuest)
        {
            switch (immersionQuest)
            {
                case GiveQuestSo:
                    CurrentPlayer.SerializedData.TotalGiveQuests++;
                    break;
                case ListenQuestSo:
                    CurrentPlayer.SerializedData.TotalListenQuests++;
                    break;
                case ReadQuestSo:
                    CurrentPlayer.SerializedData.TotalReadQuests++;
                    break;
                case ReportQuestSo:
                    CurrentPlayer.SerializedData.TotalReportQuests++;
                    break;
                default:
                    Debug.LogError("This immersion quest type does not exist!");
                    break;
            }
        }
        
        private void GetMasteryTerminalAndUpdateTotal(MasteryQuestSo masteryQuest)
        {
            switch (masteryQuest)
            {
                case DamageQuestSo:
                    CurrentPlayer.SerializedData.TotalDamageQuests++;
                    break;
                case KillQuestSo:
                    CurrentPlayer.SerializedData.TotalKillQuests++;
                    break;
                default:
                    Debug.LogError("This mastery quest type does not exist!");
                    break;
            }        
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