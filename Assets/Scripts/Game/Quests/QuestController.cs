using System.Collections.Generic;
using System.Linq;
using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.Quests
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private int countableQuestElements;
        [SerializeField] private int completedTasks;
        [field: SerializeReference] private SelectedLevels selectedLevels;
        [SerializeField] private List<QuestList> questLists;
        public static event  QuestOpenedEvent QuestOpenedEventHandler;

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
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            IQuestElement.QuestElementEventHandler -= UpdateQuest;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name is not "LevelWithEnemies") return;
            StartCoroutine(InitializeQuests(selectedLevels.GetCurrentLevel().Quests.questLines));
        }

        private IEnumerator<WaitForEndOfFrame> InitializeQuests(List<QuestList> originalQuestLines)
        {
            yield return new WaitForEndOfFrame();
            questLists = new List<QuestList>();
            foreach (var questList in originalQuestLines)
            {
                questLists.Add(new QuestList(questList));
                QuestOpenedEventHandler?.Invoke(null, new NewQuestEventArgs(questList.GetCurrentQuest(), questList.NpcInCharge));
            }
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
            }
        }

        private void UpdateKillQuest(QuestKillEnemyEventArgs killQuestArgs)
        {
            var enemyKilled = killQuestArgs.EnemyWeaponTypeSo;
            var questId = killQuestArgs.QuestId;
            if (questLists.Any(questList => 
                    questList.RemoveAvailableQuestWithId<KillQuestSo, WeaponTypeSO>(enemyKilled, questId)))
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
            if (questLists.Any(questList => 
                    questList.RemoveAvailableQuestWithId<DamageQuestSo, DamageQuestData>(damageData, questId)))
            {
                return;
            }
            Debug.LogError($"$No damage Quests With This Enemy ({enemyDamaged}) Available");
        }
        
        #endregion

        #region Explore Quest

        private void UpdateExploreQuest(QuestExploreRoomEventArgs exploreQuestArgs)
        {
            var roomExplored = exploreQuestArgs.RoomCoordinates;
            var questId = exploreQuestArgs.QuestId;
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ExploreQuestSo, Coordinates>(roomExplored, questId)))
            {
                return;
            }

            Debug.LogError($"$No Explore Quests With This Room ({roomExplored}) Available.");
        }

        #endregion

        #region GetItem
        //TODO check if this method is really the best to select between Get Item Quests
        private void UpdateGetItemQuest(QuestGetItemEventArgs getItemQuestArgs)
        {
            var itemCollected = getItemQuestArgs.ItemType;
            var questId = getItemQuestArgs.QuestId;
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GatherQuestSo, ItemSo>(itemCollected, questId)))
            {
                return;
            }
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GiveQuestSo, ItemSo>(itemCollected, questId)))
            {
                return;
            }
            if (questLists.Any(questList =>
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
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ListenQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ReportQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<GiveQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            if (questLists.Any(questList =>
                    questList.RemoveAvailableQuestWithId<ExchangeQuestSo, NpcSo>(npcToTalk, questId)))
            {
                return;
            }
            Debug.Log($"No Talk Quests With This Npc ({npcToTalk}) Available");
        }
        #endregion
    }
}