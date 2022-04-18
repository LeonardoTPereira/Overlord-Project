using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.GameManager.Player;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager;
using Game.LevelSelection;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.GameManager.DungeonManager
{
    public class DungeonSceneManager : MonoBehaviour
    {
        public GameObject gameOverScreen, victoryScreen;
        private bool isCompleted;
        private bool isInGame;
        [field: Scene] public string GameUI { get; set; } = "GameUI";
        public static event EventHandler NewLevelLoadedEventHandler;
        public static event FinishMapEvent FinishMapEventHandler;
        public bool createMaps = false; //If true, runs the AE to create maps. If false, loads the ones on the designated folders
        public AudioSource audioSource;
        public AudioClip bgMusic, fanfarreMusic;

        public bool survivalMode, enemyMode;
        public GameObject introScreen;
        private Map map;
        public QuestLine currentQuestLine;
        public int maxTreasure;
        public EnemyLoader enemyLoader;
        public int mapFileMode;
        public ProjectileTypeSO projectileType;
        public ProjectileTypeRuntimeSetSO projectileSet;
        private DungeonFileSo currentDungeonSo;

        private void Awake()
        {
            enemyLoader = gameObject.GetComponent<EnemyLoader>();
            audioSource = GetComponent<AudioSource>();
        }
        

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBhv.GotTriforceEventHandler += LevelComplete;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler += SetCurrentLevelQuestLine;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler += SetProjectileSO;
            DungeonSceneLoader.LoadLevelEventHandler += SetCurrentLevelQuestLine;
            FormBHV.PostTestFormQuestionAnsweredEventHandler += EndDungeon;
            LevelSelectManager.LoadLevelEventHandler += SetCurrentLevelQuestLine;
        }

        private void OnDisable()
        {
            LevelLoaderBHV.loadLevelButtonEventHandler -= SetCurrentLevelQuestLine;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler -= SetProjectileSO;
            DungeonSceneLoader.LoadLevelEventHandler -= SetCurrentLevelQuestLine;
            FormBHV.PostTestFormQuestionAnsweredEventHandler -= EndDungeon;
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBhv.GotTriforceEventHandler -= LevelComplete;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            LevelSelectManager.LoadLevelEventHandler -= SetCurrentLevelQuestLine;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Level" || scene.name == "LevelWithEnemies")
            {
                isInGame = true;
                isCompleted = false;

                SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
                OnLevelLoadedEvents();
                ChangeMusic(bgMusic);
                maxTreasure = currentQuestLine.ItemParametersForQuestLine.TotalItems;
                EnemyLoader.LoadEnemies(currentQuestLine.EnemySos);
                DungeonLoader.LoadNewLevel(currentDungeonSo, currentQuestLine, map.Dimensions);
            }
            if (scene.name == "Main")
            {
                introScreen.SetActive(true);
            }
        }

        //TODO display something about the player losing and call a continue screen os something like this.
        private void GameOver(object sender, EventArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(GameUI);
            gameOverScreen.SetActive(true);
        }
        
        private void LevelComplete(object sender, EventArgs eventArgs)
        {
            ChangeMusic(fanfarreMusic);
            SceneManager.UnloadSceneAsync(GameUI);
            //TODO save every gameplay data
            //TODO make it load a new level

            //Analytics for the level
            if (!createMaps && !survivalMode)
                victoryScreen.SetActive(true);
        }
        
        public void ChangeMusic(AudioClip music)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            if (music == fanfarreMusic)
            {
                audioSource.volume = 0.3f;
            }
            else
                audioSource.volume = 0.7f;
            audioSource.clip = music;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        public void StopMusic()
        {
            audioSource.Stop();
        }
        
        public void OnLevelLoadedEvents()
        {
            NewLevelLoadedEventHandler?.Invoke(null, EventArgs.Empty);
        }

        private void SetProjectileSO(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            projectileType = eventArgs.ProjectileSO;
        }
        
        public void SetCurrentLevelQuestLine(object sender, LevelLoadEventArgs args)
        {
            currentQuestLine = args.LevelQuestLine;
        }
        
        private void EndDungeon(object sender, EventArgs eventArgs)
        {
            FinishMapEventHandler?.Invoke(this, new FinishMapEventArgs(map));
        }
    }
}