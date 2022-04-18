using System;
using Game.Events;
using Game.GameManager.DungeonManager;
using Game.LevelSelection;
using Game.NarrativeGenerator;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    public class GameManagerSingleton : MonoBehaviour
    {
        public static GameManagerSingleton Instance { get; private set; }
        
        public bool IsLastQuestLine { get; set; }

        public static event EventHandler GameStartEventHandler;
        
        public bool arenaMode;
        
        public void Awake()
        {
            //Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        private void Start()
        {
            GameStartEventHandler(null, EventArgs.Empty);
        }
        
        private void OnApplicationQuit()
        {
            AnalyticsEvent.GameOver();
        }
        
        void OnEnable()
        {

            DungeonSceneLoader.LoadLevelEventHandler += CheckIfLastAvailableQuestline;
            LevelSelectManager.LoadLevelEventHandler += CheckIfLastAvailableQuestline;
        }
        void OnDisable()
        {
            DungeonSceneLoader.LoadLevelEventHandler -= CheckIfLastAvailableQuestline;
            LevelSelectManager.LoadLevelEventHandler -= CheckIfLastAvailableQuestline;
        }
        
        public void MainMenu()
        {
            GameStartEventHandler(null, EventArgs.Empty);
            SceneManager.LoadScene("Main");
        }

        public void CheckIfLastAvailableQuestline(object sender, LevelLoadEventArgs args)
        {
            IsLastQuestLine = args.IsLastQuestLine;
        }
    }
}

