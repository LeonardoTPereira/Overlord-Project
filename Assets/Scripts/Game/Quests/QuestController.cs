using System.Collections.Generic;
using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Quests
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private int countableQuestElements;
        [SerializeField] private int completedTasks;
        [field: SerializeReference] private SelectedLevels selectedLevels;
        [SerializeField] private List<QuestList> questLists;
        public static event QuestCompletedEvent QuestCompletedEventHandler;
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
            //TODO move this processing inside the QuestSo and their children
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not KillQuestSo killQuestSo) continue;
                if (!killQuestSo.HasEnemyToKill(enemyKilled)) continue;
                UpdateValidKillQuest(questList, killQuestSo, enemyKilled);
                return;
            }

            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetFirstKillQuestWithEnemyAvailable(enemyKilled);
                if (currentQuest == null) continue;
                UpdateValidKillQuest(questList, currentQuest, enemyKilled);
                return;
            }
            Debug.Log($"$No Kill Quests With This Enemy ({enemyKilled}) Available");
        }

        private void UpdateValidKillQuest(QuestList questList, KillQuestSo killQuestSo, WeaponTypeSO enemyKilled)
        {
            killQuestSo.SubtractEnemy(enemyKilled);
            if (!killQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, killQuestSo);
        }

        #region Damage
        private void UpdateDamageQuest ( QuestDamageEnemyEventArgs damageQuestArgs )
        {
            var enemyDamaged = damageQuestArgs.EnemyWeaponTypeSo;
            var damage = damageQuestArgs.Damage;
            var damageQuestSo = DamageQuestSo.GetValidDamageQuest( damageQuestArgs, questLists );
            var questList = questLists.Find( x => x.Quests.Contains(damageQuestSo) );
            if ( damageQuestSo == null )
            {
                Debug.Log($"$No damage Quests With This Enemy ({enemyDamaged}) Available");
            }
            UpdateValidDamageQuest(questList, damageQuestSo, enemyDamaged, damage);
        }

        private void UpdateValidDamageQuest(QuestList questList, DamageQuestSo damageQuestSo, WeaponTypeSO enemyDamaged, int damage)
        {
            damageQuestSo.SubtractDamage(enemyDamaged, damage);
            if (!damageQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, damageQuestSo);
        }
        #endregion

        #region Explore Quest
        private void UpdateExploreQuest ( QuestExploreRoomEventArgs exploreQuestArgs )
        {
            var roomExplored = exploreQuestArgs.RoomCoordinates;
            var exploreQuestSo = ExploreQuestSo.GetValidExploreQuest( exploreQuestArgs, questLists );
            var questList = questLists.Find( x => x.Quests.Contains(exploreQuestSo) );
            if ( exploreQuestSo == null )
            {
                Debug.Log($"$No Explore Quests With This Room ({roomExplored}) Available.");
            }
            exploreQuestSo.ExploreRoom( roomExplored );
            if (!exploreQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, exploreQuestSo);
        }
        #endregion

        #region GetItem
        private void UpdateGetItemQuest(QuestGetItemEventArgs getItemQuestArgs)
        {
            var itemCollected = getItemQuestArgs.ItemType;
            var gatherQuestSo = GatherQuestSo.GetValidGatherQuest( getItemQuestArgs, questLists );
            if ( gatherQuestSo != null )
            {
                var questList = questLists.Find( x => x.Quests.Contains(gatherQuestSo) );
                UpdateValidGatherQuest(questList, gatherQuestSo, itemCollected);
                return;
            }

            var giveQuestSo = GiveQuestSo.GetValidGiveQuest( getItemQuestArgs, questLists );
            if ( giveQuestSo != null )
            {
                var questList = questLists.Find( x => x.Quests.Contains(giveQuestSo) );
                UpdateValidGiveQuest(questList, giveQuestSo, itemCollected);
                return;
            }

            var exchangeQuestSo = ExchangeQuestSo.GetValidExchangeQuest( getItemQuestArgs, questLists);
            if ( exchangeQuestSo != null )
            {
                var questList = questLists.Find( x => x.Quests.Contains(exchangeQuestSo) );
                UpdateValidExchangeQuest(questList, exchangeQuestSo, itemCollected);
                return;
            }

            Debug.Log($"$No Get Quests With This Item ({itemCollected}) Available.");
        }

        private void UpdateValidExchangeQuest(QuestList questList, ExchangeQuestSo exchangeQuestSo, ItemSo itemCollected)
        {
            exchangeQuestSo.SubtractItem( itemCollected );
            //Quest is only completed when talking to npc
        }

        private void UpdateValidGiveQuest(QuestList questList, GiveQuestSo giveQuestSo, ItemSo itemCollected)
        {
            giveQuestSo.CollectItem( itemCollected );
            //Quest is only completed when talking to npc
        }
        
        private void UpdateValidGatherQuest(QuestList questList, GatherQuestSo getQuestSo, ItemSo itemCollected)
        {
            getQuestSo.SubtractItem(itemCollected);
            if (!getQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, getQuestSo);
        }
        #endregion
        
        #region Listen
        private void UpdateTalkQuest(QuestTalkEventArgs talkQuestArgs)
        {
            var npcToTalk = talkQuestArgs.Npc;
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not ListenQuestSo ListenQuestSo) continue;
                if (!(ListenQuestSo.Npc == npcToTalk)) continue;
                CompleteQuestAndRemoveFromOngoing(questList, currentQuest);
                return;
            }

            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetFirstTalkQuestWithNpc(npcToTalk);
                if (currentQuest == null) continue;
                CompleteQuestAndRemoveFromOngoing(questList, currentQuest);
                return;
            }
            Debug.Log($"No Talk Quests With This Npc ({npcToTalk}) Available");
        }
        #endregion

        private void CompleteQuestAndRemoveFromOngoing(QuestList questList, QuestSo completedQuest)
        {
            completedQuest.IsCompleted = true;
            CheckCompletionAndComplete(questList);
        }

        private void CheckCompletionAndComplete(QuestList questList)
        {
            var currentQuest = questList.GetCurrentQuest();
            if (currentQuest == null) return;
            if (!currentQuest.IsCompleted) return;
            QuestCompletedEventHandler?.Invoke(null, new NewQuestEventArgs(currentQuest, questList.NpcInCharge));
            questList.CurrentQuestIndex++;
            CheckCompletionAndComplete(questList);
        }
    }
}