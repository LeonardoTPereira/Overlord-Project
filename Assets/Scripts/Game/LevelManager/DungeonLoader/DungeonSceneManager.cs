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
        [field: SerializeReference] private SelectedLevels selectedLevels;
        private DungeonLoader _dungeonLoader;
        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBhv.GotTriforceEventHandler += LevelComplete;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void Start()
        {
            _dungeonLoader = GetComponent<DungeonLoader>();
            EnemyLoader.LoadEnemies(currentQuestLines.EnemySos);
            _dungeonLoader.LoadNewLevel(_currentDungeonSo, currentQuestLines);
            PlayBgm(AudioManager.BgmTracks.DungeonTheme);
            gameOverScreen.GetComponent<GameOverPanelBhv>().currentLevel = selectedLevels.GetCurrentLevel();
            SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(MinimapUI, LoadSceneMode.Additive);
            StartCoroutine(_dungeonLoader.OnStartMap(_currentDungeonSo.BiomeName));
        }

        private void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBhv.GotTriforceEventHandler -= LevelComplete;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name is not ("LevelWithEnemies")) return;
            _currentDungeonSo = selectedLevels.GetCurrentLevel().Dungeon;
            currentQuestLines = selectedLevels.GetCurrentLevel().QuestLines;
            maxTreasure = currentQuestLines.ItemParametersForQuestLines.TotalItems;
            OnLevelLoadedEvents();
        }

        private void PlayBgm(AudioManager.BgmTracks bgmTrack)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(bgmTrack));
        }

        //TODO display something about the player losing and call a continue screen os something like this.
        private void GameOver(object sender, EventArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(GameUI);
            SceneManager.UnloadSceneAsync(MinimapUI);
            gameOverScreen.SetActive(true);
        }
        
        private void LevelComplete(object sender, EventArgs eventArgs)
        {
            PlayBgm(AudioManager.BgmTracks.VictoryTheme);            
            SceneManager.UnloadSceneAsync(GameUI);
            SceneManager.UnloadSceneAsync(MinimapUI);

            selectedLevels.GetCurrentLevel().CompleteLevel();
            victoryScreen.SetActive(true);
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