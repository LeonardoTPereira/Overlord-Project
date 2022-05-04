using System;
using Game.Audio;
using Game.DataCollection;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonManager;
using Game.LevelSelection;
using Game.MenuManager;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LevelManager.DungeonLoader
{
    public class DungeonSceneManager : MonoBehaviour, ISoundEmitter
    {
        public GameObject gameOverScreen, victoryScreen;
        private bool isCompleted;
        private bool isInGame;
        [field: Scene] public string GameUI { get; set; } = "GameUI";
        public static event EventHandler NewLevelLoadedEventHandler;
        public static event FinishMapEvent FinishMapEventHandler;
        public bool createMaps = false; //If true, runs the AE to create maps. If false, loads the ones on the designated folders

        public bool survivalMode;
        public QuestLine currentQuestLine;
        public int maxTreasure;
        private DungeonFileSo currentDungeonSo;
        [field: SerializeReference] private SelectedLevels _selectedLevels;
        private Map _map;
        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBhv.GotTriforceEventHandler += LevelComplete;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler += SetCurrentLevelQuestLine;
            DungeonSceneLoader.LoadLevelEventHandler += SetCurrentLevelQuestLine;
            FormBHV.PostTestFormQuestionAnsweredEventHandler += EndDungeon;
            LevelSelectManager.LoadLevelEventHandler += SetCurrentLevelQuestLine;
        }

        private void Start()
        {
            var dungeonLoader = GetComponent<DungeonLoader>();
            _map = dungeonLoader.LoadNewLevel(currentDungeonSo, currentQuestLine);
            EnemyLoader.LoadEnemies(currentQuestLine.EnemySos);
            PlayBgm(AudioManager.BgmTracks.DungeonTheme);
            gameOverScreen.GetComponent<GameOverPanelBhv>().currentLevel = _selectedLevels.GetCurrentLevel();
        }

        private void OnDisable()
        {
            LevelLoaderBHV.loadLevelButtonEventHandler -= SetCurrentLevelQuestLine;
            DungeonSceneLoader.LoadLevelEventHandler -= SetCurrentLevelQuestLine;
            FormBHV.PostTestFormQuestionAnsweredEventHandler -= EndDungeon;
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBhv.GotTriforceEventHandler -= LevelComplete;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            LevelSelectManager.LoadLevelEventHandler -= SetCurrentLevelQuestLine;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name is not ("Level" or "LevelWithEnemies")) return;
            isInGame = true;
            isCompleted = false;
            currentDungeonSo = _selectedLevels.GetCurrentLevel().Dungeon;
            currentQuestLine = _selectedLevels.GetCurrentLevel().Quests;
            SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
            OnLevelLoadedEvents();
            maxTreasure = currentQuestLine.ItemParametersForQuestLine.TotalItems;
        }

        private void PlayBgm(AudioManager.BgmTracks bgmTrack)
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(bgmTrack));
        }

        //TODO display something about the player losing and call a continue screen os something like this.
        private void GameOver(object sender, EventArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(GameUI);
            gameOverScreen.SetActive(true);
        }
        
        private void LevelComplete(object sender, EventArgs eventArgs)
        {
            PlayBgm(AudioManager.BgmTracks.VictoryTheme);            
            SceneManager.UnloadSceneAsync(GameUI);

            _selectedLevels.GetCurrentLevel().CompleteLevel();
            victoryScreen.SetActive(true);
        }
        
        public void OnLevelLoadedEvents()
        {
            NewLevelLoadedEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void SetCurrentLevelQuestLine(object sender, LevelLoadEventArgs args)
        {
            currentQuestLine = args.LevelQuestLine;
        }
        
        private void EndDungeon(object sender, EventArgs eventArgs)
        {
            FinishMapEventHandler?.Invoke(this, new FinishMapEventArgs(_map));
        }
    }
}