using System;
using Game.Audio;
using Game.EnemyGenerator;
using Game.Events;
using Game.GameManager;
using Game.GameManager.Player;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.LevelSelection;
using Game.MenuManager;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformGame.Dungeon.DungeonLoader
{
    public class PlatformDungeonSceneManager : MonoBehaviour
    {
        public GameObject gameOverScreen, victoryScreen;
        [field: Scene] public string GameUI { get; set; } = "GameUI";
        public static event EventHandler NewLevelLoadedEventHandler;
        public bool createMaps = false; //If true, runs the AE to create maps. If false, loads the ones on the designated folders

        public bool survivalMode;
        public QuestLineList currentQuestLines;
        public int maxTreasure;
        private DungeonFileSo currentDungeonSo;
        [field: SerializeReference] private SelectedLevels _selectedLevels;
        private Map _map;
        private Game.LevelManager.DungeonLoader.DungeonLoader _dungeonLoader;
        private EnemyGeneratorManager _enemyGenerator;

        private void Start()
        {
            OnLevelFinishedLoading();
            
            _enemyGenerator = GetComponent<EnemyGeneratorManager>();
            currentQuestLines.EnemySos = _enemyGenerator.EvolveEnemies(DifficultyLevels.Hard);
            
            _dungeonLoader = GetComponent<Game.LevelManager.DungeonLoader.DungeonLoader>();
            Debug.Log("Got Dungeon Loader: "+_dungeonLoader);
            EnemyLoader.LoadEnemies(currentQuestLines.EnemySos);
            _map = _dungeonLoader.LoadNewLevel(currentDungeonSo, currentQuestLines);
            Debug.Log("Loading Enemies");
            Debug.Log("Loaded Enemies");
            
            StartCoroutine(_dungeonLoader.OnStartMap(currentDungeonSo.BiomeName));
        }

        
        private void OnLevelFinishedLoading()
        {
            Debug.Log("Finished Loading Dungeon Scene in Dungeon Scene Manager");
            Debug.Log("Selected Levels Amount: " + _selectedLevels.Levels?.Count);
            currentDungeonSo = _selectedLevels.GetCurrentLevel().Dungeon;
            currentQuestLines = _selectedLevels.GetCurrentLevel().QuestLines;
            Debug.Log("Dungeons: " + currentDungeonSo.BiomeName);
            Debug.Log("QuestLine Elements: " + currentQuestLines.QuestLines.Count);
            maxTreasure = currentQuestLines.ItemParametersForQuestLines.TotalItems;
        }

        private void GameOver(object sender, EventArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(GameUI);
            gameOverScreen.SetActive(true);
        }

    }
}