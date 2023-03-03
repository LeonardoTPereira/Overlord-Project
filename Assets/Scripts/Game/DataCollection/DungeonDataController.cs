using System;
using Game.Dialogues;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using UnityEngine;

namespace Game.DataCollection
{
    public class DungeonDataController : MonoBehaviour
    {
        public DungeonData CurrentDungeon { get; set; }
        private PlayerProfile _inputProfile;

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
            DungeonPlayer.ExitRoomEventHandler += OnRoomExit;
            QuestLine.QuestCompletedEventHandler += OnQuestEvent;
            QuestGeneratorManager.FixedLevelProfileEventHandler += OnLevelWithFixedProfileCreated;
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
            DungeonPlayer.ExitRoomEventHandler -= OnRoomExit;
            QuestLine.QuestCompletedEventHandler -= OnQuestEvent;
            QuestGeneratorManager.FixedLevelProfileEventHandler -= OnLevelWithFixedProfileCreated;
        }
        


        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            CurrentDungeon.AddLostHealth(eventArgs.DamageDone);        
        }

        private void OnLevelWithFixedProfileCreated(object sender, ProfileSelectedEventArgs eventArgs)
        {
            _inputProfile = eventArgs.PlayerProfile;
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
            CurrentDungeon.AddCollectedTreasure(eventArgs.QuestId);

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
        
        private void OnQuestEvent(object sender, NewQuestEventArgs eventArgs)
        {
            switch (eventArgs.Quest)
            {
                case AchievementQuestSo achievementQuest:
                    CurrentDungeon.CompletedAchievementQuests++;
                    GetAchievementTerminalAndUpdate(achievementQuest);
                    break;
                case CreativityQuestSo creativityQuest:
                    CurrentDungeon.CompletedCreativityQuests++;
                    GetCreativityTerminalAndUpdate(creativityQuest);
                    break;
                case ImmersionQuestSo immersionQuest:
                    CurrentDungeon.CompletedImmersionQuests++;
                    GetImmersionTerminalAndUpdate(immersionQuest);
                    break;
                case MasteryQuestSo masteryQuest:
                    CurrentDungeon.CompletedMasteryQuests++;
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
                    CurrentDungeon.CompletedExchangeQuests++;
                    break;
                case GatherQuestSo:
                    CurrentDungeon.CompletedGatherQuests++;
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
                    CurrentDungeon.CompletedExploreQuests++;
                    break;
                case GotoQuestSo:
                    CurrentDungeon.CompletedGoToQuests++;
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
                    CurrentDungeon.CompletedGiveQuests++;
                    break;
                case ListenQuestSo:
                    CurrentDungeon.CompletedListenQuests++;
                    break;
                case ReadQuestSo:
                    CurrentDungeon.CompletedReadQuests++;
                    break;
                case ReportQuestSo:
                    CurrentDungeon.CompletedReportQuests++;
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
                    CurrentDungeon.CompletedDamageQuests++;
                    break;
                case KillQuestSo:
                    CurrentDungeon.CompletedDamageQuests++;
                    break;
                default:
                    Debug.LogError("This mastery quest type does not exist!");
                    break;
            }        
        }

        public void SetDungeonParameters()
        {
            CurrentDungeon.InputProfile = _inputProfile;
        }
    }
}