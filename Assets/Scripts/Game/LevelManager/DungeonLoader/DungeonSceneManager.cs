using System;
using Game.Audio;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonManager;
using Game.LevelSelection;
using Game.MenuManager;
using Game.NarrativeGenerator.Quests;
using Game.SaveLoadSystem;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LevelManager.DungeonLoader
{
    public class DungeonSceneManager : MonoBehaviour, ISoundEmitter
    {
        public GameObject gameOverScreen, victoryScreen;
        [field: Scene] public string GameUI { get; set; } = "GameUI";
        [field: Scene] public string MinimapUI { get; set; } = "MinimapUI";
        public static event EventHandler NewLevelLoadedEventHandler;
        public QuestLineList currentQuestLines;
        public int maxTreasure;
        private DungeonFileSo _currentDungeonSo;
        [field: SerializeReference] protected SelectedLevels selectedLevels;
        private DungeonLoader _dungeonLoader;
        protected void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBhv.GotTriforceEventHandler += LevelComplete;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        protected void Start()
        {
            _dungeonLoader = GetComponent<DungeonLoader>();
            EnemyLoader.LoadEnemies(currentQuestLines.EnemySos);
            _dungeonLoader.LoadNewLevel(_currentDungeonSo, currentQuestLines);
            PlayBackgroundMusic();
            SetGameOverCurrentLevel();
            LoadSecondaryScenes();
            StartCoroutine(_dungeonLoader.OnStartMap(_currentDungeonSo.BiomeName));
        }

        protected virtual void PlayBackgroundMusic()
        {
            PlayBgm(AudioManager.BgmTracks.DungeonTheme);
        }

        protected virtual void SetGameOverCurrentLevel()
        {
            gameOverScreen.GetComponent<GameOverPanelBhv>().currentLevel = selectedLevels.GetCurrentLevel();
        }

        protected virtual void LoadSecondaryScenes()
        {
            SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(MinimapUI, LoadSceneMode.Additive);
        }

        protected void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBhv.GotTriforceEventHandler -= LevelComplete;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (VerifySceneName(scene)) return;
            _currentDungeonSo = selectedLevels.GetCurrentLevel().Dungeon;
            currentQuestLines = selectedLevels.GetCurrentLevel().QuestLines;
            maxTreasure = currentQuestLines.ItemParametersForQuestLines.TotalItems;
            OnLevelLoadedEvents();
        }

        protected virtual bool VerifySceneName(Scene scene)
        {
            return scene.name is not ("LevelWithEnemies");
        }

        protected virtual void PlayBgm(AudioManager.BgmTracks bgmTrack)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(bgmTrack));
        }

        //TODO display something about the player losing and call a continue screen os something like this.
        protected virtual void GameOver(object sender, EventArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(GameUI);
            SceneManager.UnloadSceneAsync(MinimapUI);
            gameOverScreen.SetActive(true);
        }
        
        protected virtual void LevelComplete(object sender, EventArgs eventArgs)
        {
            PlayBgm(AudioManager.BgmTracks.VictoryTheme);            
            SceneManager.UnloadSceneAsync(GameUI);
            SceneManager.UnloadSceneAsync(MinimapUI);

            SetComplete();
            victoryScreen.SetActive(true);
        }

        protected void SetComplete()
        {
            selectedLevels.GetCurrentLevel().CompleteLevel();
        }
        
        public void OnLevelLoadedEvents()
        {
            NewLevelLoadedEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void SetCurrentLevelQuestLine(object sender, LevelLoadEventArgs args)
        {
            currentQuestLines = args.LevelQuestLines;
        }
    }
}