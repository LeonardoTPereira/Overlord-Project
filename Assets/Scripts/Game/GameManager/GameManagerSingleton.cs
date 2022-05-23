using System;
using Game.Audio;
using Game.Events;
using Game.LevelManager.DungeonLoader;
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

        public static event EventHandler GameStartEventHandler;
        
        public bool arenaMode;

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main" || scene.name == "ContentGenerator")
            {
                ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.MainMenuTheme));
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

