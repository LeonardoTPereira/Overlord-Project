using System.Collections.Generic;
using System.Linq;
using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
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
        private List<QuestList> _questLists;
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
            if (scene.name is not ("Level" or "LevelWithEnemies")) return;
            InitializeQuests(selectedLevels.GetCurrentLevel().Quests.questLines);
        }

        private void InitializeQuests(List<QuestList> originalQuestLines)
        {
            _questLists = new List<QuestList>();
            foreach (var questList in originalQuestLines)
            {
                _questLists.Add(new QuestList(questList));
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
            }
        }

        private void UpdateKillQuest(QuestKillEnemyEventArgs killQuestArgs)
        {
            var enemyKilled = killQuestArgs.EnemyWeaponTypeSo;
            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not KillQuestSO killQuestSo) continue;
                if (!killQuestSo.HasEnemyToKill(enemyKilled)) continue;
                UpdateValidKillQuest(questList, killQuestSo, enemyKilled);
                return;
            }

            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetFirstKillQuestWithEnemyAvailable(enemyKilled);
                if (currentQuest != null) continue;
                UpdateValidKillQuest(questList, currentQuest, enemyKilled);
                return;
            }
            Debug.Log("No Kill Quests With This Enemy Available");
        }

        private void UpdateValidKillQuest(QuestList questList, KillQuestSO killQuestSo, WeaponTypeSO enemyKilled)
        {
            killQuestSo.SubtractEnemy(enemyKilled);
            if (!killQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, killQuestSo);
        }
        
        private void UpdateGetItemQuest(QuestGetItemEventArgs getItemQuestArgs)
        {
            var itemCollected = getItemQuestArgs.ItemType;
            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not GetQuestSo getItemQuestSo) continue;
                if (!getItemQuestSo.HasItemToCollect(itemCollected)) continue;
                UpdateValidGetItemQuest(questList, getItemQuestSo, itemCollected);
                return;
            }

            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetFirstGetItemQuestWithEnemyAvailable(itemCollected);
                if (currentQuest != null) continue;
                UpdateValidGetItemQuest(questList, currentQuest, itemCollected);
                return;
            }
            Debug.Log("No Kill Quests With This Enemy Available");
        }
        
        private void UpdateValidGetItemQuest(QuestList questList, GetQuestSo getQuestSo, ItemSo itemCollected)
        {
            getQuestSo.SubtractItem(itemCollected);
            if (!getQuestSo.CheckIfCompleted()) return;
            CompleteQuestAndRemoveFromOngoing(questList, getQuestSo);
        }

        
        private void UpdateTalkQuest(QuestTalkEventArgs talkQuestArgs)
        {
            var npcToTalk = talkQuestArgs.Npc;
            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not TalkQuestSO talkQuestSo) continue;
                if (!(talkQuestSo.Npc == npcToTalk)) continue;
                CompleteQuestAndRemoveFromOngoing(questList, currentQuest);
                return;
            }

            foreach (var questList in _questLists)
            {
                var currentQuest = questList.GetFirstTalkQuestWithNpc(npcToTalk);
                if (currentQuest != null) continue;
                CompleteQuestAndRemoveFromOngoing(questList, currentQuest);
                return;
            }
            Debug.Log("No Talk Quests With This Npc Available");
        }

        private void CompleteQuestAndRemoveFromOngoing(QuestList questList, QuestSO completedQuest)
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