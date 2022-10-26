using System;
using System.Collections.Generic;
using System.Linq;
using Game.LevelManager.DungeonLoader;
using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.Quests
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private int countableQuestElements;
        [SerializeField] private int completedTasks;
        [field: SerializeReference] private SelectedLevels selectedLevels;
        [SerializeField] private QuestLineList questLines;

        public QuestLineList QuestLines => questLines;

        public int CountableQuestElements
        {
            get => countableQuestElements;
            set => countableQuestElements = value;
        }

        public int CompletedTasks
        {
            get => completedTasks;
            set => completedTasks = value;
        }

        private void OnEnable()
        {
            IQuestElement.QuestElementEventHandler += UpdateQuest;
            IQuestElement.QuestCompletedEventHandler += CompleteQuest;
            DungeonSceneManager.NewLevelLoadedEventHandler += OnDungeonLoaded;
        }

        private void OnDisable()
        {
            IQuestElement.QuestElementEventHandler -= UpdateQuest;
            IQuestElement.QuestCompletedEventHandler -= CompleteQuest;
            DungeonSceneManager.NewLevelLoadedEventHandler -= OnDungeonLoaded;
        }
        
        private void OnDungeonLoaded(object sender, EventArgs eventArgs)
        {
            StartCoroutine(InitializeQuests(selectedLevels.GetCurrentLevel().QuestLines));
        }

        private IEnumerator<WaitForEndOfFrame> InitializeQuests(QuestLineList originalQuestLines)
        {
            yield return new WaitForEndOfFrame();
            questLines = ScriptableObject.CreateInstance<QuestLineList>();
            questLines.Init(originalQuestLines);
            questLines.OpenStartingQuests();
        }

        private void UpdateQuest(object sender, QuestElementEventArgs eventArgs)
        {
            switch (eventArgs)
            {
                case QuestKillEnemyEventArgs killQuestArgs:
                    UpdateKillQuest(killQuestArgs);
                    break;
                case QuestTalkEventArgs talkQuestArgs:
                    UpdateTalkQuest(talkQuestArgs);
                    break;
                case QuestGetItemEventArgs getItemEventArgs:
                    UpdateGetItemQuest(getItemEventArgs);
                    break;
                case QuestDamageEnemyEventArgs damageQuestArgs:
                    UpdateDamageQuest( damageQuestArgs );
                    break;
                case QuestExploreRoomEventArgs exploreQuestArgs:
                    UpdateExploreQuest( exploreQuestArgs );
                    break;
                case QuestReadEventArgs readQuestArgs:
                    UpdateReadQuest( readQuestArgs );
                    break;
                case QuestExchangeDialogueEventArgs exchangeDialogueEventArgs:
                    var npc = questLines.NpcSos.Find(_ => _.NpcName == exchangeDialogueEventArgs.NpcName);
                    UpdateTalkQuest(new QuestTalkEventArgs(npc, exchangeDialogueEventArgs.QuestId));
                    break;
                case QuestGiveDialogueEventArgs giveDialogueEventArgs:
                    npc = questLines.NpcSos.Find(_ => _.NpcName == giveDialogueEventArgs.NpcName);
                    UpdateTalkQuest(new QuestTalkEventArgs(npc, giveDialogueEventArgs.QuestId));
                    break;
            }
        }

        private void CompleteQuest(object sender, QuestElementEventArgs eventArgs)
        {
            foreach (var questLine in questLines.QuestLines.Where(questLine => questLine.GetCurrentQuest()?.Id == eventArgs.QuestId))
            {
                questLine.CloseCurrentQuest();
            }
        }

        private void UpdateKillQuest(QuestKillEnemyEventArgs killQuestArgs)
        {
            var enemyKilled = killQuestArgs.EnemyWeaponTypeSo;
            var questId = killQuestArgs.QuestId;
            if (questLines.QuestLines.Any(questList => 
                    questList.RemoveAvailableQuestWithId<KillQuestSo, WeaponTypeSo>(enemyKilled, questId)))
            {
                return;
            }
            Debug.LogError($"$No Kill Quests With This Enemy ({enemyKilled}) Available");
        }

        #region Damage
        private void UpdateDamageQuest ( QuestDamageEnemyEventArgs damageQuestArgs )
        {
            var enemyDamaged = damageQuestArgs.EnemyWeaponTypeSo;
            var damage = damageQuestArgs.Damage;
            var damageData = new DamageQuestData(damage, enemyDamaged);
            var questId = damageQuestArgs.QuestId;
            questLines.QuestLines.Any(questList => 
                    questList.RemoveAvailableQuestWithId<DamageQuestSo, DamageQuestData>(damageData, questId));
            //Debug.LogError($"$No damage Quests With This Enemy ({enemyDamaged}) Available");
        }
        
        #endregion

        #region Explore Quest

        private void UpdateExploreQuest(QuestExploreRoomEventArgs exploreQuestArgs)
        {
            var roomExplored = exploreQuestArgs.RoomCoordinates;
            var questId = exploreQuestArgs.QuestId;
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ExploreQuestSo, Coordinates>(roomExplored, questId)))
            {
                return;
            }

            questLines.QuestLines.Any(questList =>
                questList.RemoveAvailableQuestWithId<GotoQuestSo, Coordinates>(roomExplored, questId));
            //Debug.LogError($"$No Explore Quests With This Room ({roomExplored}) Available.");
        }

        #endregion

        #region GetItem
        //TODO check if this method is really the best to select between Get Item Quests
        private void UpdateGetItemQuest(QuestGetItemEventArgs getItemQuestArgs)
        {
            var itemCollected = getItemQuestArgs.ItemType;
            var questId = getItemQuestArgs.QuestId;
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GatherQuestSo, ItemSo>(itemCollected, questId)))
            {
                return;
            }
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GiveQuestSo, ItemSo>(itemCollected, questId)))
            {
                return;
            }
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ExchangeQuestSo, ItemSo>(itemCollected, questId)))
            {
                return;
            }

            Debug.Log($"$No Get Quests With This Item ({itemCollected}) Available.");
        }
        
        #endregion
        
        #region Listen
        private void UpdateTalkQuest(QuestTalkEventArgs talkQuestArgs)
        {
            //TODO check what is the logic for this and the Give/Exchange quests that appear both here and on item.
            var npcToTalk = talkQuestArgs.Npc;
            var questId = talkQuestArgs.QuestId;
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ListenQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ReportQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GiveQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ExchangeQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            Debug.Log($"No Talk Quests With This Npc ({npcToTalk}) Available");
        }
        #endregion

        #region Read
        private void UpdateReadQuest(QuestReadEventArgs readQuestArgs)
        {
            Debug.Log("received read quest event args");
            var itemToRead = readQuestArgs.ReadableItem;
            var questId = readQuestArgs.QuestId;
            if (questLines.QuestLines.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ReadQuestSo, ItemSo>(itemToRead, questId)))
            {
                return;
            }
            Debug.Log($"No Read Quests With This item ({itemToRead}) Available");
        }
        #endregion
    }
}