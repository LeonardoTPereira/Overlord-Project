using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Quests
{
    public class PlaceholderQuestController : MonoBehaviour
    {
        [SerializeField] private int countableQuestElements = 0;
        [SerializeField] private int completedTasks = 0;
        [field: SerializeReference] private SelectedLevels _selectedLevels;
        private QuestLine _currentQuestLine;


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
            _currentQuestLine = _selectedLevels.GetCurrentLevel().Quests;
        }

        private void UpdateQuest(object sender, QuestElementEventArgs eventArgs)
        {
            if (eventArgs.GetType() == typeof(QuestKillEnemyEventArgs))
            {
                
            }
            else if (eventArgs.GetType() == typeof(QuestGetItemEventArgs))
            {
                
            }
        }
    }
}