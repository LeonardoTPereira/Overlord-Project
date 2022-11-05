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
using PlatformGame.Dungeon.DungeonLoader;

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
        private Game.LevelManager.DungeonLoader.DungeonLoader _dungeonLoader;
        private EnemyGeneratorManager _enemyGenerator;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        
        private void Start()
        {
            _dungeonLoader = GetComponent<Game.LevelManager.DungeonLoader.DungeonLoader>();
            Debug.Log("Got Dungeon Loader: "+_dungeonLoader);
            EnemyLoader.LoadEnemies(currentQuestLines.EnemySos);
            _dungeonLoader.LoadNewLevel(currentDungeonSo, currentQuestLines);
            Debug.Log("Loading Enemies");
            Debug.Log("Loaded Enemies");
            
            StartCoroutine(_dungeonLoader.OnStartMap(currentDungeonSo.BiomeName));
        }

        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name is not ("Dungeon")) return;
            Debug.Log("Finished Loading Dungeon Scene in Dungeon Scene Manager");
            Debug.Log("Selected Levels Amount: " + _selectedLevels.Levels?.Count);
            currentDungeonSo = _selectedLevels.GetCurrentLevel().Dungeon;
            currentQuestLines = _selectedLevels.GetCurrentLevel().QuestLines;
            Debug.Log("Dungeons: " + currentDungeonSo.BiomeName);
            Debug.Log("QuestLine Elements: " + currentQuestLines.QuestLines.Count);
            maxTreasure = currentQuestLines.ItemParametersForQuestLines.TotalItems;
        }
        

    }
}