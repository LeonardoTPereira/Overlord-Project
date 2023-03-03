using System;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.SaveLoadSystem;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    public class GameManagerSingleton : MonoBehaviour, ISoundEmitter
    {
        public static GameManagerSingleton Instance { get; private set; }
        [field: SerializeField] public ProjectileTypeSO playerProjectile { get; set; }

        public bool IsLastQuestLine { get; set; }
        [field: SerializeField] private SceneReference experimentSelectorScreen;

        public static event EventHandler GameStartEventHandler;
        public static event Action LoadStateHandler;
        private bool _hasLoaded;

        public bool arenaMode;

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main" || scene.name == "ContentGenerator")
            {
                ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.MainMenuTheme));
            }

            if (scene.name == "ExperimentLevelSelector")
            {
                if (!_hasLoaded)
                {
                    //LoadStateHandler?.Invoke();
                    _hasLoaded = true;
                }
            }
        }

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
            GameStartEventHandler?.Invoke(null, EventArgs.Empty);
            if (SaveLoadManager.HasSaveFile())
            {
                SceneManager.LoadScene(experimentSelectorScreen.SceneName);
            }
        }
        
        private void OnApplicationQuit()
        {
            AnalyticsEvent.GameOver();
        }
        
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        
        public void MainMenu()
        {
            GameStartEventHandler?.Invoke(null, EventArgs.Empty);
            SceneManager.LoadScene("Main");
        }
    }
}

